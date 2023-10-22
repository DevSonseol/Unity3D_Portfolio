using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Skill : MonoBehaviour
{
    public GameObject parentUI; // panel_Ingame
    public GameObject skillContentRowPrefab;
    public ScrollRect skillSroll;
    public DragSkill skillDragIcon;

    private void OnDisable()
    {
        skillDragIcon.gameObject.SetActive(false);
    }

    private void Start()
    {
        AddSkillContentRows();
    }

    void AddSkillContentRows()
    {
        Main main = Main.Instance;
        foreach(SkillData data in main.SkillDatas)
        {
            GameObject row = main.Instantiate(skillContentRowPrefab);
            row.transform.parent = skillSroll.content.transform;
            SkillContentRow skillContentRow = row.GetComponent<SkillContentRow>();
            skillContentRow.Init(data);
        }
    }
}
