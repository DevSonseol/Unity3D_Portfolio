using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTimer : MonoBehaviour
{
    [SerializeField] private InGame_QuickSlot quickSlot;
    [HideInInspector] public SkillData skillData;
    [SerializeField] protected GameObject casterObject;
    [SerializeField] protected bool canCast;
    [HideInInspector] public bool CanCast => canCast;
    [SerializeField] private float countCoolTime;
    [HideInInspector] public float CountCoolTime => countCoolTime;

    public void InitSkillTimer(GameObject caster, SkillData data , InGame_QuickSlot slot)
    {
        casterObject = caster;
        skillData = data;
        canCast = true;
        countCoolTime = skillData.CoolTime;
        quickSlot = slot;
    }

    public void EndSkillTimer()
    {
        canCast = false;
        countCoolTime = skillData.CoolTime;
    }

    void ReadySkillTimer()
    {
        canCast = true;
        countCoolTime = skillData.CoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCast) return;

        countCoolTime -= Time.deltaTime;
        quickSlot.slotCoolTime.fillAmount = countCoolTime/ skillData.CoolTime;
        if (countCoolTime < 0)
            ReadySkillTimer();
    }
}
