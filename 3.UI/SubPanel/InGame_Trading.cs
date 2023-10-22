using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Trading : MonoBehaviour
{
    public GameObject tradeItemPrefab;
    public ScrollRect scrollRect;

    public List<TradeItemSlot> slots;

    public void OpenTradeUI()
    {
        UIMain uimain = UIMain.Instance;
        NPC npc = uimain.Panel_InGame.IngameUIDialogue.Interact_NPC;

        NPC_Trading trading = npc.GetComponent<NPC_Trading>();

        if (trading == null)
        {
            Debug.Log(npc.name + " : no npc_trading");
            CloseCloseTradeUI();
            return;
        }

        InitSlots(trading);
    }

    public void InitSlots(NPC_Trading trading)
    {
        Main main = Main.Instance;
        for(int i = 0; i < trading.tradingItemDatas.Count; ++i)
        {
            GameObject slot = Instantiate<GameObject>(tradeItemPrefab,scrollRect.content);
            TradeItemSlot tradeSlot = slot.GetComponent<TradeItemSlot>();
            tradeSlot.Init(trading.tradingItemDatas[i]);
            slots.Add(tradeSlot);
        }
    }

    public void CloseCloseTradeUI()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            Destroy(slots[i].gameObject);
        }
        slots.Clear();

        UIMain uimain = UIMain.Instance;
        uimain.Panel_InGame.IngameUITrading.gameObject.SetActive(false);
        uimain.Panel_InGame.uiInventory.gameObject.SetActive(false);
    }
}
