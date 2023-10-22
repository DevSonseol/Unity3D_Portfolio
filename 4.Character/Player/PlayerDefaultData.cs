using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDefaultData
{
    public string playerName = "";
    public IngameMap map = IngameMap.Town;
    public Vector3 pos = Vector3.zero;
}
