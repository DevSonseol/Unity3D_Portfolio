using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lobby_CharacterSlot : ButtonAddOn
{
    public bool isSelected = false;
    public uint slotID;

    public TMPro.TextMeshProUGUI textName;
    public TMPro.TextMeshProUGUI textClass;
    public TMPro.TextMeshProUGUI textLv;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
       
    }
}
