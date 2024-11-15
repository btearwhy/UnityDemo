using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : Page
{
    public Button Button_Start;
    public Button Button_Setting;

    private void Start()
    {
        Button_Start.GetComponent<ButtonController>().OnComplete += () => TurnPage.AutoFlip(FlipRegion.RightBottom);
        Button_Setting.GetComponent<ButtonController>().OnComplete += () => TurnPage.AutoFlip(FlipRegion.RightTop);
    }
}


