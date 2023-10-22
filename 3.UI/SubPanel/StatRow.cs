using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatRow : MonoBehaviour
{
    public TMP_Text statText;
    public TMP_Text valueText;

    public void Init(string stat, float value)
    {
        statText.text = stat;
        valueText.text = value.ToString();
    }
}
