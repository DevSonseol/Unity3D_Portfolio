using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Main : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private List<ItemData> itemDatas;
    [SerializeField] private GameObject itemPrefab;
    private Transform canvasTransform;

    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
        }
    }

    private InventoryItem selectedItem;
    public InventoryItem SelectedItem => selectedItem;

    private InventoryItem overlapItem;
    private RectTransform rectTransform;
    private InventoryItem itemToHighLight;

    void InitInventory()
    {
        canvasTransform = UIMain.Instance.canvasRT;
    }

    private void OnUpdateInventory(float dt)
    {
        if (gameState != GameState.InGame) return;

        if (UIMain.Instance.Panel_InGame.IngameUIState != IngameUIState.Inventory) return;

        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
                CreateRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            ShowHighlighter(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }

        if(Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) return;

        selectedItem.Rotate();
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        if (selectedItemGrid == null) return;

        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
        {
            GameObject itemToDrop = Instantiate(PrefabContainer.Instance.DropItem);
            DropItem dropitem = itemToDrop.GetComponent<DropItem>();
            dropitem.InitDropItem(mainPlayer.transform.position, itemToInsert.ItemData);
            Destroy(itemToInsert.gameObject);
            itemToInsert = null;
            Debug.Log("아이템을 삽입 할 수 없습니다");
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid) return;

        if (selectedItem == null)
        {
            itemToHighLight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighLight != null)
            {
                ShowHighlighter(true);
                SetSizeHighlighter(itemToHighLight);
                SetPositionHighlighter(selectedItemGrid, itemToHighLight);
            }
            else
            {
                ShowHighlighter(false);
            }
        }
        else
        {
            ShowHighlighter(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y,
                selectedItem.WIDTH, selectedItem.HEIGHT));
            SetSizeHighlighter(selectedItem);
            SetPositionHighlighter(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        int selectedItemID = UnityEngine.Random.Range(0, itemDatas.Count);
        PutItemToInventory(itemDatas[selectedItemID]);
    }

    private void CreateItem(ItemData item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        inventoryItem.Set(item);
        selectedItem = inventoryItem;
    }

    public void PutItemToInventory(ItemData itemData)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        inventoryItem.Set(itemData);

        InsertItem(inventoryItem);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void RightMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            InterractItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void InterractItem(Vector2Int tileGridPosition)
    {
        InventoryItem interactItem = selectedItemGrid.GetItem(tileGridPosition.x, tileGridPosition.y);
        if (interactItem == null) return;

        if(true == interactItem.ItemData.Interract())
        {
            selectedItemGrid.CleanGridReference(interactItem);
            Destroy(interactItem.gameObject);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        if (position.x < 0)
            position.x = 0;
        if (position.y < 0)
            position.y = 0;

        return selectedItemGrid.GetGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
        else
        {
            GameObject itemToDrop = Instantiate(PrefabContainer.Instance.DropItem);
            DropItem dropitem = itemToDrop.GetComponent<DropItem>();
            dropitem.InitDropItem(mainPlayer.transform.position, selectedItem.ItemData);
            selectedItemGrid.CleanGridReference(selectedItem);
            Destroy(selectedItem.gameObject);
            selectedItem = null;
            Debug.Log("아이템을 버립니다");
            return;
        }
    }
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    #region HighLight
    [HideInInspector]public RectTransform highlighter;

    public void SetSizeHighlighter(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPositionHighlighter(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem, targetItem.onGridPosiontX, targetItem.onGridPosiontY);

        highlighter.localPosition = pos;
    }
    public void SetPositionHighlighter(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
    }

    public void SetParentHighlighter(ItemGrid targetGrid)
    {
        if (targetGrid == null) return;
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }


    public void ShowHighlighter(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    #endregion
}
