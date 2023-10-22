using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Minimap : MonoBehaviour
{
    [SerializeField] private Text miniMapText;

    public void UpdateMiniMap(string mapName)
    {
        miniMapText.text = mapName;
    }
}
