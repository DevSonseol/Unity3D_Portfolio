using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public abstract void InitUI();
    public abstract void UpdateUI(int type);
}
