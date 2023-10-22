using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.DebugUI;

public partial class Boss : Character
{
    [Tooltip("dropTable , itemData = dropableItem , float = percent : 100 = 100% drop , 50 = 50% dropable")]
    [SerializeField] private Dictionary<ItemData, float> itemDropTable = new Dictionary<ItemData, float>();
    [SerializeField] private QuestReporter questReporter;

    private BossSpawner baseSpawner;
    public SkillData testSkillData;
    [SerializeField] private SkillData castingSkillData;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private SphereCollider sphereCollider;

    private int _mask = (1 << (int)GlobalEnum.Layer.Player) | (1 << (int)GlobalEnum.Layer.NPC) | (1 << (int)GlobalEnum.Layer.Wall);

    private float delayTargetAccess = 3.0f;
    private bool IsRushReady = false;
    private Vector3 RushDir;
    private float originSpeed;
    private float rushSpeedUp = 2.5f;
    private float rushDist = 20.0f;

    private bool IsStunEnd = false;

    [SerializeField ] private float rushDamage;

    void Awake()
    {
        base.Awake();
    }

    public void Init(BossSpawner spawner)
    {
        baseSpawner = spawner;
        base.InitCharacter();
        ChangeState(CharacterState.Idle);

        //skill 
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = this.transform;
    }

    void Start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    protected override void BeginIdle()
    {
        nma.ResetPath();
        this.animator.Play("Idle");
    }

    protected override void UpdateIdle(float dt)
    {
        Main main = Main.Instance;
        GameObject player = main.Player.gameObject;
        if (player == null) { return; }
        target = player;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= GetStat(Stat.ScanRange)) 
        {
            ChangeState(CharacterState.Move);
            return;
        }
    }

    protected override void EndIdle()
    {
    }

    protected override void BeginMove()
    {
        Main main = Main.Instance;
        Character player = main.Player;
        this.animator.Play("Move");
        if (player.GetStat(Stat.HP) <= 0)
        {
            target = null;
            destPos = baseSpawner.transform.position;
        }
    }

    protected override void UpdateMove(float dt)
    {
        if (target != null)
        {
            if(delayTargetAccess > .0f)
                delayTargetAccess -= dt;
            destPos = target.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if (distance <= GetStat(Stat.AttackRange))
            {
                ChangeState(CharacterState.Attack);
                return;
            }
            else if (delayTargetAccess < .0f)
            {
                delayTargetAccess = 3.0f;
                ChangeState(CharacterState.Spell);
                return;
            }
        }

        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.5f)
        {
            ChangeState(CharacterState.Idle);
        }
        else
        {
            nma.SetDestination(destPos);
        }
    }

    protected override void EndMove()
    {
    }

    protected override void BeginAttack()
    {
        this.animator.Play("Attack");
        this.animator.speed = GetStat(Stat.AttackSpeed);
        this.animator.SetBool("Attacking", true);

        nma.ResetPath();
        this.gameObject.transform.LookAt(target.gameObject.transform);

        timeToCount = 1 / GetStat(Stat.AttackSpeed);
        if (timeToCount == float.MaxValue)
        {
            timeToCount = 1f;
        }
        countingTime = timeToCount;

        StartCoroutine("RunAttack1");
    }

    protected override void UpdateAttack(float dt)
    {
        Main main = Main.Instance;
        destPos = target.transform.position;

        if (target == null) ChangeState(CharacterState.Idle);

        if (countingTime <= 0)
        {
            countingTime = timeToCount + countingTime;
            ChangeState(CharacterState.Move);
        }
        else
        {
            countingTime -= dt;
        }
    }

    protected override void EndAttack()
    {
        this.animator.SetBool("Attacking", false);
    }

    protected override void BeginSpell()
    {
        RushDir = (target.transform.position - this.transform.position).normalized;
        nma.ResetPath();
        StartCoroutine("RunAttack2");

        originSpeed = GetStat(Stat.Speed);
        //nma.speed = originSpeed * rushSpeedUp;
    }

    protected override void UpdateSpell(float dt)
    {
        if (IsRushReady)
        {
            transform.position += RushDir * originSpeed * rushSpeedUp * dt;
            transform.LookAt(transform.position + RushDir);
        }

        //Collider [] colliders = Physics.OverlapCapsule(pos1, pos2, capsuleCollider.radius, _mask);
        Collider[] colliders =  Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, _mask);

        if (colliders.Length != 0)
        {
            foreach (Collider col in colliders)
            {
                // 캐릭터랑 벽 나누기
                if (col.gameObject.layer == (int)LayerMask.NameToLayer("Wall"))
                {
                    ChangeState(CharacterState.Stun);
                        return;
                }
                Character character = col.GetComponent<Character>();

                if (character == null || character.GetStat(Stat.HP) <= 0) continue;

                character.TakeDamage(rushDamage, this);
                Debug.Log(character.name + ": TakeDamaged");
                ChangeState(CharacterState.Stun);
                return;
            }
      
        }

        //Damage(colliders);
    }

    protected override void EndSpell()
    {
        IsRushReady = false;
        nma.speed = originSpeed;
        this.animator.speed = GetStat(Stat.AttackSpeed);
    }

    protected override void BeginStun()
    {
        this.animator.Play("MonRushCol");
        StartCoroutine("MonRushCol");
    }

    protected override void UpdateStun(float dt)
    {
        if(IsStunEnd)
            ChangeState(CharacterState.Move);
    }

    protected override void EndStun()
    {
        IsStunEnd = false;
    }

    protected override void BeginDie()
    {
        StopAllCoroutines();
        projectorAtt1.gameObject.SetActive(false);
        projectorAtt2.gameObject.SetActive(false);
        this.gameObject.layer = (int)GlobalEnum.Layer.Ground;
        SpawnItemByDropTable();
        this.animator.Play("Die");

        nma.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        countingTime = 5;

        questReporter.Report();
    }

    protected override void UpdateDie(float dt)
    {
        if (countingTime <= 0)
        {
            Main.Instance.Destroy(this.gameObject);
        }
        else
        {
            countingTime -= dt;
        }
    }

    protected override void EndDie()
    {
    }

    void BossSkill()
    {
        Main main = Main.Instance;

        GameObject SkillPrefab = main.Instantiate(testSkillData.SkillPrefab);
        Skill Skill = SkillPrefab.GetComponent<Skill>();
        Skill.transform.position = this.transform.position;
        Skill.transform.rotation = this.transform.rotation;
        Skill.InitSkill(testSkillData, this.gameObject, LayerMask.GetMask("Player"), target);
    }

    void SpawnItemByDropTable()
    {
        foreach (KeyValuePair<ItemData, float> item in itemDropTable)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random <= item.Value)
            {
                GameObject toCreateItem = Main.Instance.Instantiate(PrefabContainer.Instance.DropItem);
                DropItem dropItem = toCreateItem.GetComponent<DropItem>();
                dropItem.InitDropItem(this.transform.position, item.Key);
            }
        }
    }
}
