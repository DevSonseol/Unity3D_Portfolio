using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingText : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected bool CanClick = false;

    [SerializeField] protected TMPro.TMP_Text text;
    [SerializeField] private Color color;
    public Transform unit;
    public Vector3 offSet;

    void Awake()
    {
        color = text.color;
    }

    protected void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Main.Instance.mainCam.transform.position);
    }

    public void InitText(string name ,Transform target, Color color)
    {
        unit = target;
        text.text = name;
        text.color = color;
        transform.SetParent(target);
        transform.position = unit.position + offSet;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanClick) return;
        Debug.Log("DropItemTest Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanClick) return;
        Debug.Log("DropItemTest Up");
    }
}
