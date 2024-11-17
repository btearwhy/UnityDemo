using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private RectTransform Self;
    public Sprite LoadingSprite;
    public Image LoadingProgress;
    public TMP_Text Text_Fail;
    public TMP_Text Text_Loading;

    private void Awake()
    {
        Self = GetComponent<RectTransform>();
    }
    private void Start()
    {
        
    }


    public IEnumerator JoinOrFail(float Timeout, float TimeStart, Action SucceedAction, Action FailAction, Func<bool> Satisfied, Func<float> Progress)
    {

        gameObject.SetActive(true);
        Self.SetAsLastSibling();
        Text_Loading.gameObject.SetActive(true);
        Text_Fail.gameObject.SetActive(false);
        while (!Satisfied() && Timeout > Time.time - TimeStart)
        {
            LoadingProgress.DOFillAmount(Progress(), 0.5f);
            yield return new WaitForSeconds(0.2f);
        }
        if (Satisfied())
        {
            LoadingProgress.DOFillAmount(1.0f, 0.5f).OnComplete(Reset);
            yield return new WaitForSeconds(0.5f);
            SucceedAction();
            Debug.Log("succeed");
        }
        else
        {
            LoadingProgress.DOFillAmount(0f, 0.5f).OnComplete(Reset);
            Text_Fail.gameObject.SetActive(true);
            Text_Loading.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            Text_Fail.gameObject.SetActive(false);
            FailAction();
        }
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        LoadingProgress.fillAmount = 0.0f;
    }
}
