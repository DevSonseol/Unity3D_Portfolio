using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Ready : Panel
{
    public float ReadytTime = 4f;

    private void Start()
    {
        Main main = Main.Instance;
    }

    public override void InitUI()
    {

    }
    public override void UpdateUI(int type)
    {
        
    }

    public void StartIntro()
    {
        Main main = Main.Instance;
        main.ChangeGameState(GameState.Intro);
    }
}
