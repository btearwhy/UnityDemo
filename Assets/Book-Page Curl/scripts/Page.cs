using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public RectTransform LeftTopPage;
    public RectTransform LeftBottomPage;
    public RectTransform RightTopPage;
    public RectTransform RightBottomPage;

    public TurnPage TurnPage;
    public RectTransform BookPanel;
    private void Start()
    {
        TurnPage = BookPanel.GetComponent<TurnPage>();
    }
    public virtual void InitialOperatiion()
    {

    }

    public virtual void Back()
    {
        TurnPage.AutoFlip(FlipRegion.LeftTop, FlipRegion.RightTop);
    }
}
