using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Main : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] ItemData[] equipmentItemData = new ItemData[(int)EquipmentType.Count];
    public ItemData[] EquipmentItemData => equipmentItemData;

    public void EquipItem(ItemData itemData)
    {
        if (itemData is EquipmentData == false) return;

        UIMain uimain = UIMain.Instance;
        if (uimain.Panel_InGame == null) return;

        if (mainPlayer == null) return;

        EquipmentData equipmentData = itemData as EquipmentData;
        equipmentItemData[(int)equipmentData.equipmentType] = equipmentData;

        Panel_InGame ingamePanel = uimain.Panel_InGame;
        ItemData equiped_Item = ingamePanel.uiInventory.uiEquipmentSlots[(int)equipmentData.equipmentType].EquipmentData;

        if (equiped_Item != null)
        {
            mainPlayer.UnEquip(equiped_Item as EquipmentData);
            CreateItem(equiped_Item);
            InventoryItem itemToInsert = selectedItem;
            selectedItem = null;
            InsertItem(itemToInsert);
            //stat 적용
            foreach (KeyValuePair<Stat, float> kv in equiped_Item.ItemStats)
            {
                playerIngameStats[kv.Key] -= kv.Value;
            }
        }
        ingamePanel.uiInventory.uiEquipmentSlots[(int)equipmentData.equipmentType].EquipItem(equipmentData);
        ingamePanel.IngameUICharacter.uiEquipmentSlots[(int)equipmentData.equipmentType].EquipItem(equipmentData);
        mainPlayer.Equip(equipmentData);

        //stat 적용
        foreach (KeyValuePair<Stat, float> kv in equipmentData.ItemStats)
        {
            playerIngameStats[kv.Key] += kv.Value; 
        }

        ingamePanel.IngameUICharacter.UpdateUIChacracter();
    }

    public void UnEquipItem(ItemData itemData)
    {
        if (itemData is EquipmentData == false) return;

        EquipmentData equipmentData = itemData as EquipmentData;
        CreateItem(equipmentData);
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);

        equipmentItemData[(int)equipmentData.equipmentType] = null;

        //플레이어 장비 mesh변경
        if (mainPlayer == null) return;

        mainPlayer.UnEquip(equipmentData);

        //UI 변경
        UIMain uimain = UIMain.Instance;
        if (uimain.Panel_InGame == null) return;

        Panel_InGame ingamePanel = uimain.Panel_InGame;
        ingamePanel.uiInventory.uiEquipmentSlots[(int)equipmentData.equipmentType].UnEquipItem();
        ingamePanel.IngameUICharacter.uiEquipmentSlots[(int)equipmentData.equipmentType].UnEquipItem();

        //stat 적용
        foreach (KeyValuePair<Stat, float> kv in equipmentData.ItemStats)
        {
            playerIngameStats[kv.Key] -= kv.Value;
        }

        ingamePanel.IngameUICharacter.UpdateUIChacracter();
    }

}
