using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : Page
{
    public Sprite LoadingSprite;
    public Image LoadingProgress;
    public float Timeout;
    public GameLobby gameLobby;
    public TMP_Text Text_Fail;  
    private TurnPage TurnPage;
    private void Start()
    {
        TurnPage = GetComponent<TurnPage>();
    }

    public override void InitialOperatiion()
    {
        base.InitialOperatiion();
        
    }

    public IEnumerator JoinOrFail(float Timeout, float TimeStart, Action SucceedAction, Action FailAction, Func<bool> Satisfied, Func<float> Progress)
    {
        LeftBottomPage = FailedPage;
        LeftTopPage = FailedPage;
        RightBottomPage = SuccesPage;
        RightTopPage = SuccesPage;
        gameObject.SetActive(true);
        while (!Satisfied() && Timeout > Time.time - TimeStart)
        {
            LoadingProgress.fillAmount = Progress();
            yield return new WaitForSeconds(0.2f);
        }
        if (Satisfied())
        {
            gameObject.SetActive(false);
            SucceedAction();
        }
        else
        {
            Text_Fail.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            Text_Fail.gameObject.SetActive(false);
            gameObject.SetActive(false);
            FailAction();
        }
    }
}
