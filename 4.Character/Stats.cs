using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector;

//캐릭터 stats은 각 캐릭터

public enum Stat
{
    Level,
    HP,
    MaxHP,
    MP,
    MaxMP,
    Speed,
    Damage,
    AttackSpeed, //FPS Per Second
    Range,
    Armor,
    /// <summary>
    /// 아래는 몬스터만 쓰는 스탯
    /// </summary>
    ScanRange,
    AttackRange,
    Count
}

[CreateAssetMenu(menuName = ("Stats"))]
public class Stats : SerializedScriptableObject
{
    public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();
}