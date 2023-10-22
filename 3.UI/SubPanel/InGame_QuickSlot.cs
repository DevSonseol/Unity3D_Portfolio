using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGame_QuickSlot : MonoBehaviour ,IPointerClickHandler
{
    [SerializeField] private SkillData skillData;
    public SkillData SkillData => skillData;
    public Image slotIcon;
    public Image slotCoolTime;

    [SerializeField] private SkillTimer skillTimer;
    public SkillTimer SkillTimer => skillTimer;

    void InitSlot(SkillData data)
    {
        skillData = data;
        slotIcon.gameObject.SetActive(true);
        slotCoolTime.gameObject.SetActive(true);
        slotIcon.sprite = skillData.Skill_Icon;
        InitTimer();
    }

    void InitTimer()
    {
        GameObject gameObject = new GameObject();
        gameObject.name = skillData.SkillName + "_Timer";
        gameObject.transform.parent = this.gameObject.transform;
        gameObject.AddComponent<SkillTimer>();
        skillTimer = gameObject.GetComponent<SkillTimer>();
        skillTimer.InitSkillTimer(this.gameObject, skillData, this);
    }


    void DuplicateCheck(SkillData data)
    {
        if (skillData == data)
            EraseData();
    }

    void EraseData()
    {
        skillData = null;
        slotIcon.gameObject.SetActive(false);
        slotCoolTime.gameObject.SetActive(false);
        Destroy(skillTimer.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + this.gameObject.name);

        UIMain uimain = UIMain.Instance;
        if (uimain.Panel_InGame.IngameUIState != IngameUIState.Skill) return;

        InGame_Skill ingameSkill = uimain.Panel_InGame.IngameUISkill;
        if (ingameSkill.skillDragIcon.gameObject.activeSelf == false) return;

        foreach(InGame_QuickSlot slot in uimain.Panel_InGame.QuickSlots)
            slot.DuplicateCheck(ingameSkill.skillDragIcon.skillData);

        if (skillData != null) EraseData();
        InitSlot(ingameSkill.skillDragIcon.skillData);
        ingameSkill.skillDragIcon.DropedOnQuickSlot();
    }

}
