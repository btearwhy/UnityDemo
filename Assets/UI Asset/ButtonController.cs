using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonEffect
{
    ImageFill,
    Move,
    Scale,
    ColorTint
}
public class ButtonController : MonoBehaviour
{
    public Image image_HaflCircle;
    public Tween tween;
    public Sequence sequence;
    public ButtonEffect buttonEffect;

    // Start is called before the first frame update

    public delegate void EventHandler();
    public event EventHandler OnComplete;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            getAction(buttonEffect)();
            tween.Play();
        });

        /*        transform.DOMoveX(100, 1);
                transform.DORestart();
                Tween myTween = new Tweener();
                myTween.SetLoops(4, LoopType.Yoyo).SetSpeedBased();
                DOTween.Play(gameObject);*/
    }

    public void Complete()
    {
        OnComplete.Invoke();
        tween.Rewind();
    }
    
    public Action getAction(ButtonEffect buttonEffect)
    {
        switch (buttonEffect)
        {
            case ButtonEffect.ImageFill:
                return ImageFillAction;
            case ButtonEffect.ColorTint:
                return ColorTintAction;
            case ButtonEffect.Move:
                return MoveAction;
            case ButtonEffect.Scale:
                return ScaleAction;
        }
        return MoveAction;
    }

    public void ImageFillAction()
    {
        tween = image_HaflCircle.DOFillAmount(1, 1).OnComplete(Complete);
    }

    public void MoveAction()
    {
        tween = GetComponent<RectTransform>().DOAnchorPos(Vector2.one, 1).OnComplete(Complete); ;
    }

    public void ScaleAction()
    {
        tween = GetComponent<RectTransform>().DOBlendableScaleBy(new Vector3(1.2f, 1.2f, 1.2f), 1).OnComplete(Complete); ;
    }
    public void ColorTintAction()
    {
        tween = GetComponent<Image>().DOBlendableColor(Color.grey, 0.7f).OnComplete(Complete);
    }
}
