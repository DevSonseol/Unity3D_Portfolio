using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTargetUI : MonoBehaviour
{
    public Slider targetHP_Slider;
    public TMPro.TMP_Text targetName;

    // Update is called once per frame
    void Update()
    {
        Main main = Main.Instance;
        
        if (main.Player.target == null || main.Player.target.GetComponent<Character>() ==null)
        {
            HideUI();
            return;
        }else
        {
            ShowUI();
        }

        targetName.text = main.Player.target.name;
        float hp = main.Player.target.GetComponent<Character>().GetStat(Stat.HP);
        float maxHP =  main.Player.target.GetComponent<Character>().GetStat(Stat.MaxHP);
        targetHP_Slider.value = hp / maxHP;
    }

    void HideUI()
    {
        targetHP_Slider.gameObject.SetActive(false);
        targetName.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        targetHP_Slider.gameObject.SetActive(true);
        targetName.gameObject.SetActive(true);
    }
}
