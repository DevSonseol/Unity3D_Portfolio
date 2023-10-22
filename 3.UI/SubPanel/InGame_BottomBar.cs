using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGame_BottomBar : MonoBehaviour
{
    private const int MaxQuickSlotCount = 6;

    [SerializeField] private Image globeHP;
    [SerializeField] private TMPro.TMP_Text hpText;
    [SerializeField] private Image globeMP;
    [SerializeField] private TMPro.TMP_Text mpText;
    [SerializeField] private Scrollbar xpBar;

    //[SerializeField] QuickSlot[MaxQuickSlotCount] quickSlots ;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Main main = Main.Instance;
        main.Player.onHPChanged += OnChangePlayerHP;
        main.Player.onMPChanged += OnChangePlayerMP;
        main.Player.onHPChanged?.Invoke();
        main.Player.onMPChanged?.Invoke();
    }

    private void OnDisable()
    {
        Main main = Main.Instance;
        main.Player.onHPChanged -= OnChangePlayerHP;
        main.Player.onMPChanged -= OnChangePlayerMP;
    }

    void OnChangePlayerHP()
    {
        Main main = Main.Instance;

        float currentHP = main.Player.GetStat(Stat.HP);
        if(currentHP <= 0) currentHP = 0;
        hpText.text = currentHP.ToString();
        float hpAmount = main.Player.GetStat(Stat.HP) / main.Player.GetStat(Stat.MaxHP);
        globeHP.fillAmount = hpAmount;
    }

    void OnChangePlayerMP()
    {
        Main main = Main.Instance;

        float currentMP = main.Player.GetStat(Stat.MP);
        if (currentMP <= 0) currentMP = 0;
        mpText.text = ((int)main.Player.GetStat(Stat.MP)).ToString();
        float mpAmount = main.Player.GetStat(Stat.MP) / main.Player.GetStat(Stat.MaxMP);
        globeMP.fillAmount = mpAmount;
    }
}
