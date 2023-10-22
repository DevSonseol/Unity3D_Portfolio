using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Ingame_Character : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text subText;

    public EquipmentSlot[] uiEquipmentSlots = new EquipmentSlot[(int)EquipmentType.Count];

    public ScrollRect scrollRect;
    public GameObject statRowPrefab;
    [SerializeField] private List<GameObject> statRows = new List<GameObject>();

    void Start()
    {
        Init();
    }

    void Init()
    {
        Main main = Main.Instance;
        nameText.text = main.playerName;
        subText.text = "여전사";
        UpdateUIChacracter();
    }

    public void UpdateUIChacracter()
    {
        Main main = Main.Instance;
        UpdateStat();
    }

    public void UpdateStat()
    {
        Main main = Main.Instance;
        foreach(GameObject go in statRows)
        {
            main.Destroy(go);
        }

        foreach(KeyValuePair<Stat, float> kv in main.playerIngameStats)
        {
            GameObject statRow = main.Instantiate(statRowPrefab);
            statRows.Add(statRow);
            statRow.GetComponent<StatRow>().Init(kv.Key.ToString(), kv.Value);
            statRow.transform.parent = scrollRect.content.transform;
        }
    }

    public  void UpdateEquipmentSlot()
    {
        Main main = Main.Instance;
    }
}
