using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    private InventoryController inventoryController;
    private ItemGrid itemGrid;
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Main main = Main.Instance;
        main.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //inventoryController.SelectedItemGrid = null;
    }
}
