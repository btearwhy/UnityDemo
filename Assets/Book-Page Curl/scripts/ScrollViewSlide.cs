using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewSlide : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public float minRequiredSpeed;




    public ScrollRect ScrollRect;
    private bool isLerping = false;
    private bool isDragging = false;
    private float reservedSpace;
    private Vector3 velocity;
    private bool isUpdated;

    private Vector3 lerpingRemains;
    public float targetHorizontalNorm;
    public List<RectTransform> items;
    private bool stopped = true;

    public delegate void ValueChangeHandler(int chosenNr);
    public event ValueChangeHandler OnValueChanged;
    
    private void Start()
    {
        ScrollRect = GetComponent<ScrollRect>();
        
    }

    private void Update()
    {
        if (!isDragging)
        {
            if (isLerping && !stopped)
            {
                Vector3 remainBefore = lerpingRemains;
                lerpingRemains = Vector3.Lerp(lerpingRemains, Vector3.zero, 10 * Time.deltaTime);
                if(lerpingRemains == Vector3.zero)
                {
                    OnValueChanged?.Invoke(Index2Pos(GetCurIndex()));
                    stopped = true;
                }
                ScrollRect.content.localPosition += remainBefore - lerpingRemains;
            }
            else if (Mathf.Abs(ScrollRect.velocity.x) < minRequiredSpeed)
            {
                lerpingRemains = CalculateLerpRemains();
                isLerping = true;
                ScrollRect.velocity = Vector3.zero;
                
            }
        }

        if (isUpdated)
        {
            isUpdated = false;
            ScrollRect.velocity = velocity;
        }
        if (ScrollRect.content.localPosition.x > 0)
        {
            velocity = ScrollRect.velocity;
            isUpdated = true;
            ScrollRect.content.localPosition -= new Vector3(reservedSpace * items.Count, 0, 0);
        }
        else if (ScrollRect.content.localPosition.x < 0 - reservedSpace * items.Count)
        {
            velocity = ScrollRect.velocity;
            isUpdated = true;
            ScrollRect.content.localPosition += new Vector3(reservedSpace * items.Count, 0, 0);
        }

    }

    private Vector3 CalculateLerpRemains()
    {
        float posx = ScrollRect.content.localPosition.x;
        float distance2Right = reservedSpace - (-posx) % reservedSpace;
        float resx;
        if (ScrollRect.velocity.x < 0)
        {
            resx = -distance2Right;
        }
        else
        {
            resx = reservedSpace - distance2Right;
        }
        return new Vector3(resx, 0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isLerping = false;
        stopped = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }


    public void NextItem()
    {
        isLerping = true;
        lerpingRemains -= new Vector3(reservedSpace, 0, 0);
    }

    public void LastItem()
    {
        isLerping = true;
        lerpingRemains += new Vector3(reservedSpace, 0, 0);
    }


    public void JumpTo(int nr)
    {
        int index = Pos2Index(nr);
        Vector3 des = new Vector3(0 - index * reservedSpace, 0, 0);
        lerpingRemains = des - ScrollRect.content.localPosition;
        isLerping = true;
    }

    public int GetCurIndex()
    {
        return (int)(-ScrollRect.content.localPosition.x) / (int)reservedSpace;
    }

    public int Pos2Index(int pos)
    {
        return pos + (ScrollRect.content.childCount - items.Count) / 2;
    }

    public int Index2Pos(int index)
    {
        int duplicate = (ScrollRect.content.childCount - items.Count) / 2;
        int pos;
        if (index < duplicate)
        {
            pos = items.Count - duplicate + index;
        }
        else if (index >= duplicate + items.Count)
        {
            pos = index - duplicate - items.Count;
        }
        else pos = index;
        return pos;
    }

    internal void AddItem(RectTransform rect)
    {
        items.Add(rect);
    }

    public void Init()
    {
        RectTransform rect = GetComponent<RectTransform>();
        foreach(RectTransform rec in items)
        {
            rec.SetParent(ScrollRect.content);
            rec.localScale = Vector3.one;
            rec.localPosition = new Vector3(rec.localPosition.x, rec.localPosition.y, 0);
            rec.sizeDelta = rect.sizeDelta;
        }
        reservedSpace = items[0].rect.width + ScrollRect.content.GetComponent<HorizontalLayoutGroup>().spacing;
        int ItemsToDuplicate = Mathf.CeilToInt(ScrollRect.viewport.rect.width / (reservedSpace));


        for (int i = 0; i < ItemsToDuplicate; i++)
        { 
            int indexHead = (items.Count - i - 1) % items.Count;
            int indexTail = i % items.Count;
            RectTransform duplicateHead = Instantiate(items[indexHead], ScrollRect.content);
            RectTransform duplicateTail = Instantiate(items[indexTail], ScrollRect.content);
            duplicateHead.SetAsFirstSibling();
            duplicateTail.SetAsLastSibling();
        }


        ScrollRect.content.localPosition = new Vector3(0 - reservedSpace * ItemsToDuplicate, ScrollRect.content.localPosition.y, ScrollRect.content.localPosition.z);
    }

    internal void Clear()
    {
        items.Clear();
        for(int i = 0; i < ScrollRect.content.childCount; i++)
        {
            Destroy(ScrollRect.content.GetChild(i).gameObject);
        }
    }
}
