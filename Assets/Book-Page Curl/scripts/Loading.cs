using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Sprite LoadingSprite;
    public Image LoadingProgress;
    public TMP_Text Text_Fail;
    public TMP_Text Text_Loading;
    private void Start()
    {

    }


    public IEnumerator JoinOrFail(float Timeout, float TimeStart, Action SucceedAction, Action FailAction, Func<bool> Satisfied, Func<float> Progress)
    {
        
        gameObject.SetActive(true);
        Text_Loading.gameObject.SetActive(true);
        Text_Fail.gameObject.SetActive(false);
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
            Text_Loading.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            Text_Fail.gameObject.SetActive(false);
            gameObject.SetActive(false);
            FailAction();
        }
    }
}
