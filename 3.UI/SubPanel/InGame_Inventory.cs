using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGame_Inventory : MonoBehaviour , IDragHandler , IBeginDragHandler
{
    public GameObject parentUI; // panel_Ingame
    public ItemGrid inventoryGrid;
    public RectTransform highlighter;

    private RectTransform rectTransform;

    public EquipmentSlot[] uiEquipmentSlots = new EquipmentSlot[(int)EquipmentType.Count];

    public TMPro.TMP_Text goldText;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Main main = Main.Instance;
        main.highlighter = highlighter;
    }

    void Update()
    {
        Main main = Main.Instance;
        if (main.Player == null) return;
        goldText.text = main.Player.gold.ToString();
    }

    public void UpdateUI()
    {
        //main inventory list 
        Main main = Main.Instance;

        //아이템 UI Bt 정렬

    }

    public void OnDrag(PointerEventData eventData)
    {
        //마우스 위치에 따라 이동하게 모든 서브 패널은 이게 가능하게 하면 좋을듯 부모 패널 만들어서
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //rectTransform.SetAsLastSibling();
    }
}
