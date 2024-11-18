using System;
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

    public Page LastPage;

    public bool isClonedForViewOnly = false;
    private void Start()
    {

    }
    public virtual void InitialOperation()
    {
        
    }

    public virtual void FlipBack()
    {
        TurnPage.AutoFlip(FlipRegion.LeftTop);
    }

    public virtual void LeaveLeftBottom()
    {

    }

    public virtual void LeaveLeftTop()
    {

    }

    public virtual void LeaveRightBottom()
    {

    }

    public virtual void LeaveRightTop()
    {

    }

    internal virtual void SetFlippedFrom(Page page)
    {
        LastPage = page;
    }

    internal void Leave(FlipRegion startRegion)
    {
        if(startRegion == FlipRegion.LeftBottom)
        {
            LeaveLeftBottom();
        }
        else if(startRegion == FlipRegion.RightBottom)
        {
            LeaveRightBottom();
        }
        else if(startRegion == FlipRegion.LeftTop)
        {
            LeaveLeftTop();
        }
        else
        {
            LeaveRightTop();
        }
    }
}
