using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Loading : Panel
{
    public Slider sliderLoading;

    public override void InitUI()
    {
    }

    public void Update()
    {
        Main main = Main.Instance;

        if (main.asyncOpType == AsyncOperationType.InGame)
        {
            sliderLoading.value = main.LoadingProcessValue;
        }
    }

    public override void UpdateUI(int type)
    {
        
    }

}
