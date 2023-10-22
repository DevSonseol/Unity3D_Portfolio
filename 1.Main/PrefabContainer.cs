using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContainer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    [Header("pooling")]
    public GameObject MonsterPrefab;
    public GameObject DamageText;
    public GameObject DropItem;
    public GameObject MouseMarker;

    private static PrefabContainer instance;
    public static PrefabContainer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }
}
