using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TaskAction : SerializedScriptableObject
{
    public abstract int Run(Task task, int currentSuccess, int successCount);
}
