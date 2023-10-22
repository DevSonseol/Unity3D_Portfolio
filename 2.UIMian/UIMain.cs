using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 설명 : UI 관리자 클래스 , UI 상태 변환, UI 인스턴스 생성/삭제 등
public enum UIState
{
    None = -1,
    Ready = 0,
    Intro,
    Lobby,
    Loading,
    InGame = 4,
    MsgBox,
    Count
}

public enum UIMouseOverState
{
    None,
    //PanelGame_EditArea
}

public enum UIFoldState
{
    Fold = 0,
    Unfold,
    Count
}

public enum Button_GroupState
{
    Normal = 0,
    MouseOver,
    Selected,
    Disabled,
    Count
}

public partial class UIMain : MonoBehaviour
{
    private static UIMain instance;
    public static UIMain Instance
    {
        get
        {
            return instance;
        }
    }

    private Panel_Ready panel_Ready;
    private Panel_Intro panel_Intro;
    private Panel_Lobby panel_Lobby;
    private Panel_Loading panel_Loading;
    private Panel_InGame panel_InGame;
    public Panel_InGame Panel_InGame => panel_InGame;
    private Panel_MsgBox panel_MsgBox;
    public Panel_MsgBox Panel_MsgBox => panel_MsgBox;

    public Canvas canvas;
    public RectTransform canvasRT;

    [NonSerialized] public UIState uiState;
    private UIMouseOverState uiMouseOverState;

    public virtual void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
    }

    void Start()
    {


    }

    void Update()
    {

    }

    public GameObject SpawnUIObject(GameObject uiPrefab)
    {
        GameObject goUIPrefab = Instantiate(uiPrefab);
        goUIPrefab.transform.SetParent(transform);
        (goUIPrefab.transform as RectTransform).offsetMin = Vector2.zero;
        (goUIPrefab.transform as RectTransform).offsetMax = Vector2.zero;
        goUIPrefab.transform.localScale = Vector3.one;
        return goUIPrefab;
    }

    public GameObject SpawnUIObject(GameObject uiPrefab, Transform parentTR)
    {
        GameObject goUIPrefab = Instantiate(uiPrefab);
        goUIPrefab.transform.SetParent(parentTR);
        (goUIPrefab.transform as RectTransform).offsetMin = Vector2.zero;
        (goUIPrefab.transform as RectTransform).offsetMax = Vector2.zero;
        goUIPrefab.transform.localScale = Vector3.one;
        return goUIPrefab;
    }
    public bool ChangeUIState(UIState _state)
    {
        Main main = Main.Instance;

        if (_state == UIState.None) return false;

        UIPrefabContainer uiPrefabContainer = UIPrefabContainer.Instance;

        if (uiState == UIState.Ready)
        {
            if (panel_Ready != null)
            {
                Destroy(panel_Ready.gameObject);
                panel_Ready = null;
            }
        }
        else if (uiState == UIState.Intro)
        {
            if (panel_Intro != null)
            {
                Destroy(panel_Intro.gameObject);
                panel_Intro = null;
            }
        }
        else if (uiState == UIState.Lobby)
        {
            if (panel_Lobby != null)
            {
                Destroy(panel_Lobby.gameObject);
                panel_Lobby = null;
            }
        }
        else if (uiState == UIState.Loading)
        {
            if (panel_Loading != null)
            {
                Destroy(panel_Loading.gameObject);
                panel_Loading = null;
            }
        }
        else if (uiState == UIState.InGame)
        {
           
        }

        uiState = _state;

        if (uiState == UIState.Ready)
        {
            panel_Ready = SpawnUIObject(uiPrefabContainer.panelReady).GetComponent<Panel_Ready>();
        }
        else if (uiState == UIState.Intro)
        {
            panel_Intro = SpawnUIObject(uiPrefabContainer.panelIntro).GetComponent<Panel_Intro>();
        }
        else if (uiState == UIState.Lobby)
        {
            panel_Lobby = SpawnUIObject(uiPrefabContainer.panelLobby).GetComponent<Panel_Lobby>();
            panel_Lobby.UpdateUI((int)LobbyState.Default);
        }
        else if (uiState == UIState.Loading)
        {
            panel_Loading = SpawnUIObject(uiPrefabContainer.panelLoading).GetComponent<Panel_Loading>();

        }
        else if (uiState == UIState.InGame)
        {
            if(panel_InGame == null)
                panel_InGame = SpawnUIObject(uiPrefabContainer.panelInGame).GetComponent<Panel_InGame>();
            //panel_InGame.UpdateUI();
        }

        if (panel_MsgBox != null)
        {
            panel_MsgBox.transform.SetAsLastSibling();
        }

        return true;
    }

    public bool Show_PopUpMsg(int popupMsgType, int param = 0)
    {
        //if (base.Show_PopUpMsg(popupMsgType, param) == false)
        //{
        //    return false;
        //}

        //RSWUIPrefabContainer uiPrefabContainer = RSWUIPrefabContainer.Instance;
        //if (panel_RSW_PopUpMsg == null)
        //{
        //    panel_RSW_PopUpMsg = SpawnUIObject(uiPrefabContainer.panelPopUpMsg).GetComponent<Panel_RSW_PopUpMsg>();
        //}
        ////panel_RSW_PopUpMsg.SetMsgType(popupMsgType);
        //panel_RSW_PopUpMsg.param = param;
        //panel_RSW_PopUpMsg.SetMsgType((RSWPopUpMsgType)popupMsgType);

        return true;
    }

    public bool Close_PopUpMsg()
    {
        //if (base.Close_PopUpMsg() == false)
        //{
        //    return false;
        //}

        //if (panel_RSW_PopUpMsg != null)
        //{
        //    Destroy(panel_RSW_PopUpMsg.gameObject);
        //    panel_RSW_PopUpMsg = null;
        //}

        return true;
    }

    //public void Show_RSC_PopUpMsg(RSCPopUpMsgType popupMsgType, int param = 0)
    //{
    //    RGUIPrefabContainer uiPrefabContainer = RGUIPrefabContainer.Instance;
    //    if (panel_RSC_PopUpMsg == null)
    //    {
    //        panel_RSC_PopUpMsg = SpawnUIObject(uiPrefabContainer.panelPopUpMsg).GetComponent<Panel_RSC_PopUpMsg>();
    //    }
    //    panel_RSC_PopUpMsg.SetMsgType(popupMsgType);
    //    panel_RSC_PopUpMsg.param = param;
    //}

    //public void Close_RSC_PopUpMsg()
    //{
    //    if (panel_RSC_PopUpMsg != null)
    //    {
    //        Destroy(panel_RSC_PopUpMsg.gameObject);
    //        panel_RSC_PopUpMsg = null;
    //    }
    //}

    public Vector3 WorldToScreenPoint(Vector3 worldPos)
    {
        Camera cam = Camera.main;
        Vector3 scrPos = cam.WorldToScreenPoint(worldPos);
        return scrPos / canvas.scaleFactor;
    }

    public Vector3 WorldToScreenPoint(Vector3 worldPos, Canvas canvasParam)
    {
        Camera cam = Camera.main;
        Vector3 scrPos = cam.WorldToScreenPoint(worldPos);
        return scrPos / canvasParam.scaleFactor;
    }

    public Vector2 AdjustScreenPoint(Vector2 scrPos)
    {
        return scrPos / canvas.scaleFactor;
    }

    public void UpdateCurrentUIMouseOverState(Vector2 mousePos)
    {
        uiMouseOverState = UIMouseOverState.None;
        Camera worldCamera = canvas.worldCamera;
        Vector2 localPoint = Vector2.zero;

        //if (panelGame != null)
        //{
        //    RectTransform rtEditArea = panelGame.rtEditArea;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(rtEditArea, mousePos, worldCamera, out localPoint);

        //    if (localPoint.x >= rtEditArea.rect.xMin &&
        //        localPoint.x <= rtEditArea.rect.xMax &&
        //        localPoint.y >= rtEditArea.rect.yMin &&
        //        localPoint.y <= rtEditArea.rect.yMax)
        //    {
        //        uiMouseOverState = RGUIMouseOverState.PanelGame_EditArea;
        //    }
        //}
    }

    public void UpdateLocalization()
    {

    }

    public void SpawnVirtualKeyboard()
    {
        //if (uiVirtualKeyboard == null)
        //{
        //    RGUIPrefabContainer uiPrefabContainer = RGUIPrefabContainer.BaseInstance as RGUIPrefabContainer;
        //    uiVirtualKeyboard = SpawnUIObject(uiPrefabContainer.panelVirtualKeyboard, this.transform).GetComponent<RG_VirtualKeyboard>();
        //}
    }

}
