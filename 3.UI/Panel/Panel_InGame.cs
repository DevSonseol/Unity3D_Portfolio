using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngameUIState
{
    None,
    Inventory,
    Dialogue,
    Skill,
    Shop,
    Character,
    Die,
    Count
}

public partial class Panel_InGame : Panel
{
    public GameObject[] activeSubPanels;

    public InGame_Inventory uiInventory;
    private IngameUIState ingameUIState = IngameUIState.None;
    public IngameUIState IngameUIState => ingameUIState;

    [SerializeField] private Ingame_Character ingameUICharacter;
    public Ingame_Character IngameUICharacter => ingameUICharacter;

    [SerializeField] private Ingame_Dialogue ingameUIDialogue;
    public Ingame_Dialogue IngameUIDialogue => ingameUIDialogue;

    [SerializeField] private InGameTargetUI targetUI;
    public InGameTargetUI InGameTargetUI => targetUI;

    [SerializeField] private InGame_Trading ingameUITrading;
    public InGame_Trading IngameUITrading => ingameUITrading;

    [SerializeField] private InGame_Skill ingameUISkill;
    public InGame_Skill IngameUISkill => ingameUISkill;

    [SerializeField] private List<InGame_QuickSlot> quickSlots;
    public List<InGame_QuickSlot> QuickSlots => quickSlots;

    [SerializeField] GameObject IngameDieUI;

    private void Start()
    {
        UpdateUI((int)IngameUIState.None);
    }

    public override void InitUI()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(ingameUIState == IngameUIState.None) 
                UpdateUI((int)IngameUIState.Inventory);
            else if(ingameUIState == IngameUIState.Inventory)
                UpdateUI((int)IngameUIState.None);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ingameUIState == IngameUIState.None)
                UpdateUI((int)IngameUIState.Skill);
            else if (ingameUIState == IngameUIState.Skill)
                UpdateUI((int)IngameUIState.None);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (ingameUIState == IngameUIState.None)
                UpdateUI((int)IngameUIState.Character);
            else if (ingameUIState == IngameUIState.Character)
                UpdateUI((int)IngameUIState.None);

            //임시
            if (ingameUIState == IngameUIState.Inventory)
            {
                ingameUICharacter.gameObject.SetActive(true);
                ingameUICharacter.transform.SetAsLastSibling();
            }
        }
    }

    public override void UpdateUI(int type)
    {
        foreach (GameObject subpanel in activeSubPanels)
            subpanel.SetActive(false);

        if (type == (int)IngameUIState.None)
        {
            ingameUIState = IngameUIState.None;
            uiInventory.gameObject.SetActive(false);
            IngameDieUI.gameObject.SetActive(false);
            ingameUISkill.gameObject.SetActive(false);
            IngameUIDialogue.gameObject.SetActive(false);
        }
        else if (type == (int)IngameUIState.Inventory)
        {
            ingameUIState = IngameUIState.Inventory;
            uiInventory.gameObject.SetActive(true);
            uiInventory.transform.SetAsLastSibling();
        }
        else if (type == (int)IngameUIState.Die)
        {
            ingameUIState = IngameUIState.Die;
            IngameDieUI.gameObject.SetActive(true);
            IngameDieUI.transform.SetAsLastSibling();
        }
        else if (type == (int)IngameUIState.Dialogue)
        {
            ingameUIState = IngameUIState.Dialogue;
            IngameUIDialogue.gameObject.SetActive(true);
            IngameUIDialogue.transform.SetAsLastSibling();
        }else if (type == (int)IngameUIState.Shop)
        {
            ingameUIDialogue.gameObject.SetActive(true);
            ingameUIState = IngameUIState.Shop;
            ingameUITrading.gameObject.SetActive(true);
            ingameUITrading.transform.SetAsLastSibling();
            ingameUITrading.OpenTradeUI();
            uiInventory.gameObject.SetActive(true);
            uiInventory.transform.SetAsLastSibling();
        }
        else if (type == (int)IngameUIState.Character)
        {
            ingameUIState = IngameUIState.Character;
            ingameUICharacter.gameObject.SetActive(true);
            ingameUICharacter.transform.SetAsLastSibling();
        }
        else if(type == (int)IngameUIState.Skill)
        {
            ingameUIState = IngameUIState.Skill;
            ingameUISkill.gameObject.SetActive(true);
            ingameUISkill.transform.SetAsLastSibling();
        }
    }

    public void start()
    {
        Main main = Main.Instance;

    }
}
