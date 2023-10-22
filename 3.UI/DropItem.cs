using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData => itemData;
    [SerializeField] private GameObject itemGroup;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private FloatingText floatingText;

    [SerializeField] private bool isDone = false;
    [SerializeField] private bool onGround = false;
    [SerializeField] private bool isUp = false;


    [SerializeField] private float jumpforce = 5f;
    [SerializeField] private float limitHeight = 2f;

    void OnEnable()
    {
        if(floatingText == null)
            floatingText = GetComponentInChildren<FloatingText>();

        isDone = false;
        onGround = false;
        isUp = false;
    }

    private void OnDisable()
    {
        if (itemPrefab != null)
            Main.Instance.Destroy(itemPrefab);
    }

    void Start()
    {
        //////임시
        //InitAndShowItemText(Color.blue);
        //itemPrefab = Instantiate<GameObject>(itemData.itemPrefab);
        //itemPrefab.transform.SetParent(this.transform);
        //itemPrefab.transform.position = this.transform.position;
        //floatingText.InitText(itemData.itemName, this.transform, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone) return;

        if(!onGround)
        {
            if (itemGroup.transform.localPosition.y > limitHeight)
                isUp = true;

            float y = jumpforce * Time.deltaTime;

            if(!isUp)
                itemGroup.transform.localPosition = new Vector3(0, itemGroup.transform.localPosition.y + y, 0);
            else
                itemGroup.transform.localPosition = new Vector3(0, itemGroup.transform.localPosition.y - y, 0);

            itemPrefab.transform.Rotate(0, 0, jumpforce);

            if (itemGroup.transform.localPosition.y < 0)
                onGround = true;
        }
        else
        {
            itemGroup.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefab.transform.rotation = Quaternion.identity;
            InitAndShowItemText(Color.white);
            isDone = true;
        }
    }

    public void InitDropItem(Vector3 groundPos,ItemData item)
    {
        itemData = item;
        floatingText.gameObject.SetActive(false);
        SetItemMesh();
        this.transform.position = groundPos;
    }

    private void SetItemMesh()
    {
        Main main = Main.Instance;
        itemPrefab = main.Instantiate(itemData.itemPrefab);
        itemPrefab.transform.SetParent(this.itemGroup.transform);
        itemPrefab.transform.localPosition = Vector3.zero;
        itemGroup.transform.localPosition = Vector3.zero;

        //ex 방패는 x축 180도.아이템마다 다 다름
    }

    void InitAndShowItemText(Color textColor)
    {
        floatingText.gameObject.SetActive(true);
        floatingText.InitText(itemData.itemName, this.transform, textColor);
    }

    public void GetDropItem()
    {
        Main main = Main.Instance;
        main.Destroy(itemPrefab);
        itemPrefab = null;

        main.Destroy(this.gameObject);
    }
}
