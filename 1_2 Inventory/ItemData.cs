using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
    Equipment,
    Consumables,
    Coin,
    ETC,
    Quest,
    Count
    //필요한거 있으면 추가할 것 ItemData
}

[CreateAssetMenu(fileName = "item", menuName = "ScriptableObjects/ItemDataSO", order = 2)]
public class ItemData : SerializedScriptableObject
{
    public int itemID;
    public string itemName;
    public ItemType itemTpye;
    public int width = 1;
    public int height = 1;
    public GameObject itemPrefab;

    public Sprite itemIcon;
    public string decription;

    [SerializeField] private Dictionary<Stat, float> itemStats;//실제 사용하는 값
    public IReadOnlyDictionary<Stat, float> ItemStats => itemStats;

    public int value;

    public virtual bool Interract()
    {
        Main main = Main.Instance;
        switch(itemTpye)
        {
            case ItemType.Equipment:
                main.EquipItem(this);
                return true;
            case ItemType.Consumables:
                //아이템 사용
                ConsumeItem();
                return true;
        }
        return false;
    }

    private void ConsumeItem()
    {
        Main main = Main.Instance;
        Player player = main.Player;

        //itemStats딕셔너리 순회
        foreach (KeyValuePair<Stat, float> stat  in itemStats)
        {
            float curStatValue = player.GetStat(stat.Key); 
            float maxValue = player.GetStat(stat.Key+1);

            float value = curStatValue + stat.Value > maxValue ? maxValue : curStatValue + stat.Value;
            player.SetStat(stat.Key, value);
            player.onHPChanged?.Invoke();
        }

    }
}
