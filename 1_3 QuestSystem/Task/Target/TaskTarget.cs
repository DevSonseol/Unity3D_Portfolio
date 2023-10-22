using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TaskTarget : SerializedScriptableObject
{
    public abstract object Value { get; }

    public abstract bool IsEqual(object target);
}
