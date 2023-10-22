using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Monster : Character
{
    [Tooltip("dropTable , itemData = dropableItem , float = percent : 100 = 100% drop , 50 = 50% dropable")]
    [SerializeField] private Dictionary<ItemData, float> itemDropTable = new Dictionary<ItemData, float>();
    [SerializeField] private int exp;

    private MonsterSpawner baseSpawner;
    private float wandercooltime;
    public GameObject[] Weapons;
    [SerializeField] private QuestReporter questReporter;

    private bool IsGoBack = false;

    void Awake()
    {
        base.Awake();
    }

    void start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    public void Init(MonsterSpawner spawner)
    {
        baseSpawner = spawner;
        base.InitCharacter();

        ChangeState(CharacterState.Idle);
        RandomWeapon();
    }

    void RandomWeapon()
    {
        if (Weapons.Length > 0)
        {
            int randomWeaponIndex = Random.Range(0, Weapons.Length);
            for (int i = 0; i < Weapons.Length; ++i)
            {
                if (randomWeaponIndex != i)
                    Weapons[i].SetActive(false);
                else
                    Weapons[i].SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        target = null;
        nma.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        this.gameObject.layer = (int)GlobalEnum.Layer.Monster;
    }

    protected override void BeginIdle()
    {
        this.animator.Play("Idle");
        wandercooltime = Random.Range(2, 6);

    }

    protected override void UpdateIdle(float dt)
    {
        Main main = Main.Instance;
        Player player = main.Player;

        if (player == null) return;

        float scanDist = (player.transform.position - transform.position).magnitude;
        if (scanDist <= GetStat(Stat.ScanRange)) //추적범위
        {
            target = player.gameObject;
            ChangeState(CharacterState.Move);
        }
        else
        {
            wandercooltime -= dt;
            if (wandercooltime < 0)
            {
                destPos = WanderPos();
                ChangeState(CharacterState.Move);
            }
        }
    }

    protected override void EndIdle()
    {
        
    }

    protected override void BeginMove()
    {
        this.animator.Play("Move");
    }

    protected override void UpdateMove(float dt)
    {
        Main main = Main.Instance;
        Player player = main.Player;

        if (player == null) return;

        float scanDist = (player.transform.position - transform.position).magnitude;

        if ((baseSpawner.transform.position - transform.position).magnitude > 10.0f)
        {
            destPos = baseSpawner.transform.position;
            target = null;
            IsGoBack = true;
        }
        else if (!IsGoBack &&scanDist <= GetStat(Stat.ScanRange))
        {
            target = player.gameObject;
        }

        if (target != null)
        {
            destPos = target.transform.position;
            float attackDist = (destPos - transform.position).magnitude;
            if (attackDist <= GetStat(Stat.AttackRange))  //공격범위안에 있을경우
            {
                ChangeState(CharacterState.Attack);
                return;
            }
        }
        
        Vector3 dir = destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.2f)
        {
            if (IsGoBack) { IsGoBack = false; }
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

        nma.ResetPath(); // 움직임 중지

        timeToCount = 1 / GetStat(Stat.AttackSpeed);
        if (timeToCount == float.MaxValue)
        {
            timeToCount = 1f;
        }

        countingTime = timeToCount;
    }

    protected override void UpdateAttack(float dt)
    {
        destPos = target.transform.position;

        if (target == null) ChangeState(CharacterState.Idle);

        this.gameObject.transform.LookAt(target.gameObject.transform);

        float targetDist = (target.transform.position - transform.position).magnitude; 

        if (countingTime <= 0)
        {
            if(targetDist < GetStat(Stat.AttackRange) && target.GetComponent<Character>().GetStat(Stat.HP) > 0)
                target.GetComponent<Character>().TakeDamage(GetStat(Stat.Damage), this);

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
        this.animator.speed = 1f;
    }

    protected override void BeginSpell()
    {
        
    }

    protected override void UpdateSpell(float dt)
    {
        
    }

    protected override void EndSpell()
    {
        
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
        this.gameObject.layer = (int)GlobalEnum.Layer.Ground;
        DropReward();
        this.animator.Play("Die");

        nma.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        countingTime = 5;

        nma.ResetPath();

        questReporter.Report();
    }

    protected override void UpdateDie(float dt)
    {
        if (countingTime <= 0)
        {
            baseSpawner.SubMosnterCount(1);
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

    void DropReward()
    {
        SpawnItemByDropTable();
        DropGold();
        DropExp();
    }
    
    void SpawnItemByDropTable()
    {
        foreach (KeyValuePair<ItemData, float> item in itemDropTable)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if(random <= item.Value)
            {
                GameObject toCreateItem = Main.Instance.Instantiate(PrefabContainer.Instance.DropItem);
                DropItem dropItem = toCreateItem.GetComponent<DropItem>();
                dropItem.InitDropItem(this.transform.position,item.Key);
            }
        }
    }

    void DropGold()
    {

    }

    void DropExp()
    {
        

    }

    Vector3 WanderPos()
    {
        while (true)
        {
            int randDest = Random.Range(0, baseSpawner.wanderPos.Length);

            if ((baseSpawner.wanderPos[randDest] - transform.position).magnitude > .1f)
            {
                return baseSpawner.wanderPos[randDest];
            }
            
        }
    }
}
