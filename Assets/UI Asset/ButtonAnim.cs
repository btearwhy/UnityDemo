using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    public Button button;
    public Image image_HaflCircle;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            image_HaflCircle.DOFillAmount(1, 1).OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby"));
            
        });

        /*        transform.DOMoveX(100, 1);
                transform.DORestart();
                Tween myTween = new Tweener();
                myTween.SetLoops(4, LoopType.Yoyo).SetSpeedBased();
                DOTween.Play(gameObject);*/
    }

    
}
