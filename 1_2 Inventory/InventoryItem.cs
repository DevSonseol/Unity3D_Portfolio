using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private ItemData itemData;
    public ItemData ItemData => itemData;   

    public int HEIGHT
    {
        get { 
            if(isRotated == false)
            {
                return itemData.height;
            }
            else
            {
                return itemData.width;
            }
        }
    }

    public int WIDTH
    {
        get
        {
            if (isRotated == false)
            {
                return itemData.width;
            }
            else
            {
                return itemData.height;
            }
        }
    }

    public int onGridPosiontX;
    public int onGridPosiontY;

    public bool isRotated = false;

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = WIDTH * ItemGrid.tileSizeWidth;
        size.y = HEIGHT* ItemGrid.tileSizeHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
    public void Rotate()
    {
        isRotated = !isRotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated == true ? 90f : 0f);
    }
}
