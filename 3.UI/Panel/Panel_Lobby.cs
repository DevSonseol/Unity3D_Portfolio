using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public enum LobbyState
{
    Default,
    CharacterSelect,
    Continue,
    Option,
    Count
}

enum LobbyDefaultBts
{
    New,
    Continue,
    Option,
    Quit,
    Count
}

enum LobbySubButtons
{
    Cancel,
    OK,
    Count
}


public partial class Panel_Lobby : Panel
{
    private LobbyState lobbyState = default;

    public ButtonAddOn[] defaultBtns = new ButtonAddOn[(int)LobbyDefaultBts.Count];
    public GameObject[] subPanels = new GameObject[(int)LobbyState.Count];

    [Header("SubButtons")]
    public GameObject subButtonParent;
    public ButtonAddOn[] subButtons = new ButtonAddOn[(int)LobbySubButtons.Count];
    public Button[] characterSlots = new Button[(int)CharacterClassType.Count];

    CharacterClassType newStartClasstype = CharacterClassType.None;
    public TMP_InputField nameField;

    bool canStart = false;

    public Slider bgmSlider;

    private void Awake()
    {
        InitUI();
    }

    public void Start()
    {
        Main main = Main.Instance;
        main.PlaySound(SoundType.BGM, "LobbyBGM");

        UpdateUI((int)LobbyState.Default);
    }

    public override void InitUI()
    {
        Main main = Main.Instance;

        //메인에 플레이어 버튼 있고 없고에 따라 continue 버튼 활성화
    }

    public override void UpdateUI(int type)
    {
        //subPanel
        for (int i = 0; i < subPanels.Length; ++i)
        {
            if (i != type)
                subPanels[i].SetActive(false);
            else
                subPanels[i].SetActive(true);
        }

        //subbutton
        if(lobbyState == LobbyState.Default)
            subButtonParent.SetActive(false);
        else
            subButtonParent.SetActive(true);

        ResetButtons();

        if(type == (int)LobbyState.Option)
        {
            subButtons[(int)LobbySubButtons.OK].gameObject.SetActive(false);
        }
        else
        {
            subButtons[(int)LobbySubButtons.OK].gameObject.SetActive(true);
        }

    }

    void ResetButtons()
    {
        foreach (ButtonAddOn button in defaultBtns)
            button.SetActive(false);

        foreach (ButtonAddOn button in subButtons)
            button.SetActive(false);
    }

    public void ChangeLobbyState(LobbyState _state)
    {
        canStart = false;
        lobbyState = _state;
        newStartClasstype = CharacterClassType.None;
        UpdateUI((int)lobbyState);
        OnNoneFocusCamera();
    }

}
