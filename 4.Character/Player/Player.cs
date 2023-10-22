using GlobalEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine.AI;

public partial class Player : Character
{
    public PlayerDefaultData defaultData;

    public Transform weaponParent;
    public GameObject equipedWeaponObject;

    private int _mask = (1 << (int)GlobalEnum.Layer.Ground) | (1 << (int)GlobalEnum.Layer.Monster) | (1 << (int)GlobalEnum.Layer.Item) |1 << (int)GlobalEnum.Layer.NPC;

    [HideInInspector] public Action onHPChanged;
    [HideInInspector] public Action onMPChanged;
    [HideInInspector] public Action onXPGained;

    [SerializeField] bool _moveToDest = false;

    [SerializeField] private BoxCollider hitCollider;
    [SerializeField] private SkillTimer castingSkillTimer;
    [SerializeField] private GameObject spellMarker;
    private GameObject targetItem;
    private GameObject targetNPC;

    public TMPro.TextMeshPro nameText;
    public int gold = 10000;

    private void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        Main main = Main.Instance;
        nameText.text = main.playerName;

        if (defaultData == null)
        {
            defaultData = new PlayerDefaultData();
        }
        Main.KeyAction -= OnKeyActionEvent;
        Main.KeyAction += OnKeyActionEvent;

        Main.MouseLeftAction -= OnLeftMouseEvent;
        Main.MouseLeftAction += OnLeftMouseEvent;
        Main.MouseRightAction -= OnRightMouseEvent;
        Main.MouseRightAction += OnRightMouseEvent;
    }

    private void Start()
    {
        base.Start();
        onHPChanged?.Invoke();
        onMPChanged?.Invoke();
    }

    private void Update()
    {
        base.Update();

        if (target != null)
        {
            if (target.GetComponent<Character>() == null)
            {
                target = null;
                return;
            }
            float dist = (transform.position - target.transform.position).magnitude;
            if (dist > 10 || target.GetComponent<Character>().GetStat(Stat.HP) <= 0 )
                target = null;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Main.Instance.SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            InteractOBJ();
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Main.Instance.LoadPlayerData();
        //}

    }

    protected override void BeginIdle()
    {
        this.animator.Play("Idle");
    }

    protected override void UpdateIdle(float dt)
    {

    }

    protected override void EndIdle()
    {

    }

    protected override void BeginMove()
    {
        //this.animator.Play("Move");
        _moveToDest = true;
        this.animator.SetBool("isWalk", _moveToDest);
        //if (isDash)
        //    destPos = dashVec;
        nma.SetDestination(destPos);
    }

    protected override void UpdateMove(float dt)
    {
        Main main = Main.Instance;

        if (targetItem != null)
        {
            Vector3 dirToTarget = targetItem.transform.position - transform.position;
            dirToTarget.y = 0;
            if (dirToTarget.magnitude < 0.5f || !_moveToDest)
            {
                if (targetItem.layer == LayerMask.NameToLayer("Item"))
                {
                    QuestReporter questReporter = targetItem.GetComponent<QuestReporter>();
                    if (questReporter != null) questReporter.Report();

                    main.PutItemToInventory(targetItem.GetComponent<DropItem>().ItemData);
                    main.Destroy(targetItem.gameObject);
                    targetItem = null;
                    ChangeState(CharacterState.Idle);
                    return;
                }
            }
        }

        Vector3 dir = destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f || !_moveToDest )
        {
            if (!isDash)
                ChangeState(CharacterState.Idle);
        }

        //벽체크
        if (nma.remainingDistance > 15.0f)
        {
            // 충돌시 정지 조건 추가예정
            nma.ResetPath();
            ChangeState(CharacterState.Idle);
        }

    }

    protected override void EndMove()
    {
        _moveToDest = false;
        this.animator.SetBool("isWalk", _moveToDest);
    }

    protected override void BeginAttack()
    {
        this.animator.Play("Attack");
        this.animator.speed = GetStat(Stat.AttackSpeed);
    
        nma.ResetPath(); // 움직임 중지

        timeToCount = 1 / GetStat(Stat.AttackSpeed);
        if(timeToCount == float.MaxValue)
        {
            timeToCount = 1f;
        }

        countingTime = timeToCount;
    }

    protected override void UpdateAttack(float dt)
    {
        if (hitCollider == null) return;

        if (countingTime <= 0)
        {
            DamageInHitBox();

            countingTime = timeToCount + countingTime;
            
            ChangeState(CharacterState.Idle); 
        }
        else
        {
            countingTime -= dt;
        }
        
    }

    void DamageInHitBox()
    {
        Collider[] colliders = Physics.OverlapBox(hitCollider.transform.position, hitCollider.size, hitCollider.transform.rotation, LayerMask.GetMask("Monster"));

        foreach (Collider col in colliders)
        {
            Character character = col.GetComponent<Character>();

            if (character == null || character.GetStat(Stat.HP) <= 0) continue;

            character.TakeDamage(CalculateDamage(), this.gameObject);
            Debug.Log(character.name + ": TakeDamaged");
            target = character.gameObject;
        }
    }

    float CalculateDamage()
    {
        Main main = Main.Instance;
        float damage = main.playerIngameStats[Stat.Damage];

        return damage;
    }

    public override void TakeDamage(float damage, UnityEngine.Object fromObject)
    {
        base.TakeDamage(damage, fromObject);
        onHPChanged?.Invoke();
    }

    protected override void EndAttack()
    {
        this.animator.speed = 1;
    }

    protected override void BeginSpell()
    {
        if(castingSkillTimer == null)
        {
            ChangeState(CharacterState.Idle);
            return;
        }

        if (castingSkillTimer.CanCast)
        {
            this.animator.Play("Spell");
            nma.ResetPath(); // 움직임 중지
            spellMarker.SetActive(true);
            float radius = castingSkillTimer.skillData.SkillPrefab.GetComponent<SphereCollider>().radius;
            spellMarker.GetComponent<PlayerSpellMarker>().InitMarker(radius);
            CostSkillCost(castingSkillTimer.skillData);
        }
        else
        {
            ChangeState(CharacterState.Idle);
        }
    }

    protected override void UpdateSpell(float dt)
    {


    }

    protected override void EndSpell()
    {
        castingSkillTimer = null;

        spellMarker.SetActive(false);
    }

    protected override void BeginStun()
    {

    }

    protected override void UpdateStun(float dt)
    {

    }

    protected override void EndStun()
    {
    }

    protected override void BeginDie()
    {
        this.animator.Play("Die");
        //Die UI 띄우기
        UIMain uIMain = UIMain.Instance;
        uIMain.Panel_InGame.UpdateUI((int)IngameUIState.Die);
    }

    protected override void UpdateDie(float dt)
    {

    }

    protected override void EndDie()
    {

    }

    public void Equip(EquipmentData equipmentData)
    {
        Main main = Main.Instance;

        if (equipmentData.equipmentType == EquipmentType.Weapon)
        {
            main.Destroy(equipedWeaponObject);
            equipedWeaponObject = main.Instantiate(equipmentData.itemPrefab, weaponParent);
            equipedWeaponObject.layer = LayerMask.NameToLayer("Player");
        }

    }

    public void UnEquip(EquipmentData equipmentData)
    {
        Main main = Main.Instance;

        if (equipmentData.equipmentType == EquipmentType.Weapon)
        {
            main.Destroy(equipedWeaponObject);
        }
    }


    private void UseSkill()
    {
        RaycastHit hit;
        Main main = Main.Instance;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        if (raycastHit)
        {
            GameObject skillPrefab = main.Instantiate(castingSkillTimer.skillData.SkillPrefab);
            Skill skill = skillPrefab.GetComponent<Skill>();
            skill.transform.position = hit.point;
            skill.InitSkill(castingSkillTimer.skillData, this.gameObject, LayerMask.GetMask("Monster"));
            castingSkillTimer.EndSkillTimer();
        }

        ChangeState(CharacterState.Idle);
    }

    private bool CheckSkillCost(SkillData skilldata)
    {
        Stat stat = skilldata.CostType;
        int skillCost = skilldata.CostAmount;

        float curStat = GetStat(stat);

        if (curStat > skillCost)
        {
            return true;
        }

        return false;
    }

    private void CostSkillCost(SkillData skilldata)
    {
        Stat stat = skilldata.CostType;
        int skillCost = skilldata.CostAmount;

        float curStat = GetStat(stat);

        if (curStat > skillCost)
        {
            SetStat(stat, curStat - skillCost);
            onMPChanged?.Invoke();
        }
    }

    public void EndDialogue()
    {
        targetNPC = null;
        targetNPC.GetComponent<NPC>().EndDialogueNPC();
    }

    private void InteractOBJ()
    {
        Collider[] cols;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        //캡슐의 맨아래 위치
        Vector3 pos1 = new Vector3(capsuleCollider.transform.position.x, capsuleCollider.transform.position.y - capsuleCollider.height, capsuleCollider.transform.position.z);
        //캡슐의 맨위 위치
        Vector3 pos2 = new Vector3(capsuleCollider.transform.position.x, capsuleCollider.transform.position.y + capsuleCollider.height, capsuleCollider.transform.position.z);

        cols = Physics.OverlapCapsule(pos1, pos2, capsuleCollider.radius);
        
        foreach(Collider col in cols)
        {
            IngameObject obj = col.gameObject.GetComponent<IngameObject>();
            if (obj == null) continue;

            obj.InteractObject();
        }

    }
}

//ㄹ
//스킬 객체    1 2 3 4 
//스킬 발사체  1 2 3 4