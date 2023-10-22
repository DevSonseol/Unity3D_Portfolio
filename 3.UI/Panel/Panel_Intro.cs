using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Intro : Panel
{
    public override void InitUI()
    {

    }
    public override void UpdateUI(int type)
    {
        
    }

    private void Update()
    {
        ClickAnyKey();
    }

    public void ClickAnyKey()
    {
        if (Input.anyKeyDown)
        {
            Main main = Main.Instance;
            main.ChangeGameState(GameState.Lobby);
        }
    }
}
