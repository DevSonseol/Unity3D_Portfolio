using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIPanelType
{
    Ready = 0,
    Intro,
    Lobby,
    Loading,
    InGame,
    Count
}

public class UIPrefabContainer : MonoBehaviour
{
    public GameObject panelReady;
    public GameObject panelIntro;
    public GameObject panelLobby;  
    public GameObject panelLoading;  
    public GameObject panelInGame;    
    public GameObject panelMsgBox;
    public GameObject panelVirtualKeyboard;

    private static UIPrefabContainer instance;
    public static UIPrefabContainer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }
}