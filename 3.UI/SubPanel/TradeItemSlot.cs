using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TradeItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image slotBG;
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemValue;
    public ItemData slotItemData;

    public void Init(ItemData itemdata)
    {
        slotItemData = itemdata;
        itemImage.sprite = slotItemData.itemIcon;
        itemName.text = slotItemData.name;
        itemValue.text = slotItemData.value.ToString() + "G";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotBG.color = Color.green;
        this.gameObject.GetComponent<Image>().color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotBG.color = Color.white;
        this.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnClickSlot()
    {
        //플레이어 돈 체크
        Main main = Main.Instance;
        int playerGold = main.Player.gold;

        //있으면 돈 차감하고
        if (playerGold >= slotItemData.value)
        {
            main.Player.gold -= slotItemData.value;
            main.PutItemToInventory(slotItemData);
        }
        //돈없음
    }
}
