using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;

// 설명 : Main 은 여러 매니저들의 기능을 하나로 모은 중앙 집권형 매니저라고 보면 된다. UI 관리를 제외한 대부분을 처리한다.

public enum GameState
{
    None  = -1,
    Ready = 0,
    Intro,
    Lobby,
    Loading,
    InGame,
    Count
}

public enum IngameMap
{
    None,
    Town,
    Dungeon,
    Count
}

public partial class Main : MonoBehaviour
{
    private GameState befGameState = GameState.None;
    [NonSerialized] private GameState gameState = GameState.None;

    private IngameMap _ingameMap = IngameMap.None;
    public IngameMap curIngameMap { get { return _ingameMap; } }

    [HideInInspector] public LobbyObjectsController lobbyObjectsController;
    private static Main instance;
    public static Main Instance
    {
        get
        {
            return instance;
        }
    }

    //data관련은 MainData에 정의 하여 사용 할 것
    public virtual void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
        InitDatas();
        InitSound();
        InitPooling();
        InitInventory();
        InitQuest();
    }

    void ActivateDisplays()
    {
        for (int i = 1; i < UnityEngine.Display.displays.Length; ++i)
        {
            // Activate the display i (i-th monitor connected to the system).            
            UnityEngine.Display.displays[i].Activate();
        }
    }

    private void OnDestroy()
    {
        EndInput();

    }

    void Start()
    {
        ChangeGameState(GameState.Ready);
    }

    //update관련 함수들을 update아래 호출 할 것.
    void Update()
    {
        float dt = Time.deltaTime;

        if(gameState == GameState.InGame)
        {
            OnUpdateInput(dt);
            OnUpdateCamera(dt);
            OnUpdateInventory(dt);
        }
    }

    // 게임 상태 변환에 따른 처리 Scene을 변화 시키고 싶을때는 ChangeGameState를 호출 할 것.
    public bool ChangeGameState(GameState _state , IngameMap ingameMap = IngameMap.None)
    {
        if (_state == gameState) return true;

        UIMain uiMain = UIMain.Instance;

        if (gameState == GameState.Ready)
        {

        }
        else if (gameState == GameState.Intro)
        {

        }
        else if (gameState == GameState.Lobby)
        {

        }
        else if (gameState == GameState.Loading)
        {

        }
        else if (gameState == GameState.InGame)
        {
            
        }
 
        befGameState = gameState;
        gameState = _state;
        ClearPool();

        if (gameState == GameState.Ready)
        {
            uiMain.ChangeUIState(UIState.Ready);
        }
        else if (gameState == GameState.Intro)
        {
            uiMain.ChangeUIState(UIState.Intro);
        }
        else if (gameState == GameState.Lobby)
        {
            uiMain.ChangeUIState(UIState.Lobby);
            lobbyObjectsController = FindAnyObjectByType<LobbyObjectsController>();
        }
        else if (gameState == GameState.Loading)
        {
            _ingameMap = ingameMap;
            uiMain.ChangeUIState(UIState.Loading);
            lobbyObjectsController = null;
            if (_ingameMap == IngameMap.Town)
            {
                StartCoroutine(_LoadSceneAsync("InGame", false));
            }
            else if (_ingameMap == IngameMap.Dungeon)
            {
                StartCoroutine(_LoadSceneAsync("Dungeon", false));
            }

        }
        else if(gameState == GameState.InGame)
        {
            uiMain.ChangeUIState(UIState.InGame);
            ChangeCameraMode((int)CameraMode.Ingame);
        }

        return true;
    }

}
