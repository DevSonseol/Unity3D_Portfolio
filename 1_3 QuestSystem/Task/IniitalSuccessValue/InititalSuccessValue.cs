using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class InititalSuccessValue : SerializedScriptableObject
{
    public abstract int GetValue(Task task);
}
