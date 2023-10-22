using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Main : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private List<SkillData> skillDatas;
    public List<SkillData> SkillDatas => skillDatas;
}
