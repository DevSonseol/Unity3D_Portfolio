using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EquipmentType
{
    None = -1,
    Helmet,
    Armor,
    Boots,
    Ring,
    Weapon,
    Shield,
    Count
}

public class EquipmentSlot : MonoBehaviour  ,IPointerClickHandler
{
    [SerializeField] private ItemData equipedItemData;
    public ItemData EquipmentData => equipedItemData;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultSprite;

    public bool canInterract = true;

    public void EquipItem(ItemData equipment)
    {
        if (equipment.itemTpye != ItemType.Equipment) return;

        equipedItemData = equipment;
        icon.sprite = equipedItemData.itemIcon;
    }

    public void UnEquipItem()
    {
        equipedItemData = null;
        icon.sprite = defaultSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canInterract) return;

        Main main = Main.Instance;
        main.UnEquipItem(equipedItemData);
    }


}
