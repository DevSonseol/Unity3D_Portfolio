using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Reward : SerializedScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    [SerializeField] private int quantity;

    public Sprite Icon => icon;
    public string Description => description;
    public int Quantity => quantity;

    public abstract void Give(Quest quest);
}

