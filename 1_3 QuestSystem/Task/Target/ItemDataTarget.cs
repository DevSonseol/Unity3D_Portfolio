using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/ItemData", fileName = "Target_")]
public class ItemDataTarget : TaskTarget
{
    [SerializeField] private ItemData value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        ItemData targetAsItemData = target as ItemData;
        if (targetAsItemData == null)
            return false;
        return value == targetAsItemData;
    }
}
