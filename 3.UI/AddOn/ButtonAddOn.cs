using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAddOn : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler 
{
    public GameObject focus;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        focus.SetActive(true);
    }

    public virtual  void OnPointerExit(PointerEventData eventData)
    {
        focus.SetActive(false);
    }

    public virtual void SetActive(bool b)
    {
        focus.SetActive(b);
    }

}
