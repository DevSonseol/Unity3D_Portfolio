using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector;
using UnityEngine.AI;

//캐릭터는 모든 character의 상위클래스이다
public enum CharacterState
{
    Idle,
    Move,
    Attack,
    Spell,
    Stun,
    Die,
    Count
}
public enum CharacterClassType
{
    None = -1,
    WarriorFemale,
    WarriorMale,
    Count
}

public enum CharacterType
{
    Unclassified = -1,
    Player = 0,
    Monster ,
    NPC,
    Count
}

// 캐릭터 클래스
public abstract class Character : SerializedMonoBehaviour
{
    //stat
    public Stats statData; //기본 설정 값
    public Dictionary<Stat, float> instanceStats; //실제 사용하는 값
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent nma;

    public GameObject target;
    [SerializeField] protected Vector3 destPos;

    [SerializeField] protected CharacterState curState = CharacterState.Idle;

    protected float timeToCount = 0;
    protected float countingTime = 0;

    public void Awake()
    {
        InitCharacter();
        ChangeState(CharacterState.Idle);
    }

    //캐릭터가 가지고 있어야하는 최소한의 데이터들을 체크
    protected void InitCharacter() 
    {
        if (statData == null)
        {
            Debug.LogError(this.gameObject.name + "has not StatData");
            return;
        }

        instanceStats = new Dictionary<Stat, float>(statData.stats);

        //모든 캐릭터 기본적으로 HP , MaxHP , MP , MaxMP , LV 를 가진다. 없으면 채워줄 것
        //위 값이 float.MaxValue 이면 데이타 오류인것
        if (instanceStats.TryGetValue(Stat.Level, out float valueLV) == false)
            instanceStats.Add(Stat.Level, float.MaxValue);
        if (instanceStats.TryGetValue(Stat.HP,out float valueHP) == false)
            instanceStats.Add(Stat.HP, float.MaxValue);
        if (instanceStats.TryGetValue(Stat.MaxHP, out float valueMaxHP) == false)
            instanceStats.Add(Stat.MaxHP, float.MaxValue);
        if (instanceStats.TryGetValue(Stat.MP, out float valueMP) == false)
            instanceStats.Add(Stat.MP, float.MaxValue);
        if (instanceStats.TryGetValue(Stat.MaxMP, out float valueMaxMP) == false)
            instanceStats.Add(Stat.MaxHP, float.MaxValue);

    }

    public void Start()
    {
    }

    public void Update()
    {
        float dt = Time.deltaTime;

        //쿨타임 관리

        switch (curState)
        {
            case CharacterState.Idle:
                UpdateIdle(dt);
                break;
            case CharacterState.Move:
                UpdateMove(dt);
                break;
            case CharacterState.Attack:
                UpdateAttack(dt);
                break;
            case CharacterState.Spell:
                UpdateSpell(dt);
                break;
            case CharacterState.Stun:
                UpdateStun(dt);
                break;
            case CharacterState.Die:
                UpdateDie(dt);
                break;
        }
    }

    public void OnDestroy()
    {
        instanceStats.Clear();
    }

    public float GetStat(Stat stat)
    {
        if (instanceStats.TryGetValue(stat, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"No stat value {stat} : {this.name}");
            return float.MaxValue;
        }
    }

    public void SetStat(Stat stat , float newValue)
    {
        if (instanceStats.ContainsKey(stat))
        {
            instanceStats[stat] = newValue;
        }
        else
        {
            Debug.LogError($"No stat value {stat} : {this.name}");
        }
    }

    // 대화 인터페이스
    public interface IConversation
    {
        void TalkToNPC(string npcName);
        void AcceptQuest(string questName);
        void CompleteQuest(string questName);
    }

    protected abstract void BeginIdle();
    protected abstract void UpdateIdle(float dt);
    protected abstract void EndIdle();

    protected abstract void BeginMove();
    protected abstract void UpdateMove(float dt);
    protected abstract void EndMove();

    protected abstract void BeginAttack();
    protected abstract void UpdateAttack(float dt);

    public virtual void TakeDamage(float damage, UnityEngine.Object fromObj)
    {
        Main main = Main.Instance;

        if (fromObj is GameObject)
            target = fromObj as GameObject;

        float curHP = GetStat(Stat.HP);
        curHP -= damage;
        SetStat(Stat.HP, curHP);

        GameObject damagetext = main.Instantiate(PrefabContainer.Instance.DamageText);
        DamageText dt = damagetext.GetComponent<DamageText>();
        Vector3 pos = main.mainCam.WorldToScreenPoint(this.transform.position);
        dt.InitText(damage, 1, this.transform , Color.white);

        if (curHP <= 0)
            ChangeState(CharacterState.Die);
    }

    protected abstract void EndAttack();

    protected abstract void BeginSpell();
    protected abstract void UpdateSpell(float dt);
    protected abstract void EndSpell();

    protected abstract void BeginStun();
    protected abstract void UpdateStun(float dt);
    protected abstract void EndStun();

    protected abstract void BeginDie();
    protected abstract void UpdateDie(float dt);
    protected abstract void EndDie();

    public void ChangeState(CharacterState nextState , SkillData skillData = null)
    {
        if (animator == null) return;

        switch (curState)
        {
            case CharacterState.Idle:
                EndIdle();
                break;
            case CharacterState.Move:
                EndMove();
                break;
            case CharacterState.Attack:
                EndAttack();
                break;
            case CharacterState.Spell:
                EndSpell();
                break;
            case CharacterState.Stun:
                EndStun();
                break;
            case CharacterState.Die:
                EndDie();
                break;
        }

        switch (nextState)
        {
            //nextState begin
            case CharacterState.Idle:
                BeginIdle();
                break;
            case CharacterState.Move:
                BeginMove();
                break;
            case CharacterState.Attack:
                BeginAttack();
                break;
            case CharacterState.Spell:
                BeginSpell();
                break;
            case CharacterState.Stun:
                BeginStun();
                break;
            case CharacterState.Die:
                BeginDie();
                break;
        }

        this.curState = nextState;
    }
}

