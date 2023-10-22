using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillData : SerializedScriptableObject
{
    [SerializeField] protected int skill_ID;
    public int Skill_ID => skill_ID;
    [SerializeField] protected Sprite skill_Icon;
    public Sprite Skill_Icon => skill_Icon;
    [SerializeField] protected string skillName;
    public string SkillName => skillName;
    [SerializeField] protected string skillDesc;
    public string SkillDesc => skillDesc;
    [SerializeField] protected int skillDamage;
    public int SkillDamage => skillDamage;
    [SerializeField] protected Stat costType;
    public Stat CostType => costType;
    [SerializeField] protected int costAmount;
    public int CostAmount => costAmount;
    [SerializeField] protected float coolTime;
    public float CoolTime => coolTime;
    [SerializeField] protected float castingTime;
    public float CastingTime => castingTime;
    [SerializeField] protected float attackCounts;
    public float AttackCounts => attackCounts;
    [SerializeField] protected float attackDelayTimes;
    public float AttackDelayTimes => attackDelayTimes;
    [SerializeField] protected GameObject skillPrefab;
    public GameObject SkillPrefab => skillPrefab;
}
