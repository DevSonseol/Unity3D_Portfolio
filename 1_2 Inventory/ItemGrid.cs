using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 64;
    public const float tileSizeHeight = 64;

    private InventoryItem[,] inventoryItemSlot;
    private RectTransform rectTransform;

    [SerializeField] private int gridSizeWidth = 10;
    [SerializeField] private int gridSizeHeight = 10;
    [SerializeField] private GameObject inventoryItemPrefab;

    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);

        rectTransform.sizeDelta = size;

        Main main = Main.Instance;
        main.SelectedItemGrid = this;
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x =(int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y =(int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        if(x < 0 || x >= gridSizeWidth || y < 0 || y >= gridSizeHeight) return null;

        return inventoryItemSlot[x, y];
    }

    public bool PlaceItem(InventoryItem inventoryItem , int posX , int posY , ref InventoryItem overlapItem)
    {
        if (false == BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT)) return false;

        if (false == OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform itemrectTransform = inventoryItem.GetComponent<RectTransform>();
        itemrectTransform.SetParent(this.rectTransform);
        inventoryItemSlot[posX, posY] = inventoryItem;

        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPosiontX = posX;
        inventoryItem.onGridPosiontY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        itemrectTransform.localPosition = position;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;

        for(int y = 0; y < gridSizeHeight; y++)
        {
            for (int x = 0; x < gridSizeWidth; x++)
            {
                if(CheckAvailbleSpace(x, y,itemToInsert.WIDTH,itemToInsert.HEIGHT) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    private bool CheckAvailbleSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //범위 넘어가는지 체크
                if (posX + x >= gridSizeWidth || posY + y >= gridSizeHeight) 
                    return false;

                //아이템 있는지 체크
                if (inventoryItemSlot[posX+x,posY+y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        if(x < 0 || x >= gridSizeWidth || y < 0 || y >= gridSizeHeight)
            return null;

        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) return null;

        CleanGridReference(toReturn);

        return toReturn;
    }

    public void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPosiontX + ix, item.onGridPosiontY + iy] = null;
            }
        }
    }

    bool PositionCheck(int posX,int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundryCheck(int posX,int posY , int width , int height)
    {
        if(PositionCheck(posX,posY) == false) return false;

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false) return false;

        return true;
    }
}
