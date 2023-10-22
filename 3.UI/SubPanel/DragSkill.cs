using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSkill : MonoBehaviour
{
    public SkillData skillData;
    public Image skillIcon;

    public void UpdateDragSkill(SkillData data)
    {
        skillData = data;
        skillIcon.sprite = skillData.Skill_Icon;
        this.gameObject.SetActive(true);
    }

    public void DropedOnQuickSlot()
    {
        skillData = null;
        skillIcon.sprite = null;
        this.gameObject.SetActive(false);
    }

    public void Update()
    {
        this.gameObject.transform.position = Input.mousePosition;
    }
}
