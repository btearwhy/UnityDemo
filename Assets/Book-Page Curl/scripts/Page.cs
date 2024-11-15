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



    private void Start()
    {

    }
    public virtual void InitialOperation()
    {
        
    }

    public void FlipBack()
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
}
