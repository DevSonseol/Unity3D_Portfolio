using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillContentRow : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private SkillData skilldata;
    public Image skill_Icon;
    public Text skill_Name;
    public Text skill_Description;

    public void Init(SkillData data)
    {
        skilldata = data;
        skill_Icon.sprite = skilldata.Skill_Icon;
        skill_Name.text = skilldata.SkillName;
        skill_Description.text = skilldata.SkillDesc;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIMain uimain = UIMain.Instance;

        uimain.Panel_InGame.IngameUISkill.skillDragIcon.UpdateDragSkill(skilldata);
    }
}
