using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewSlide : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public float dragSpeed;

    public float widthOfAnItem;

    private bool isDragging = false;

    public ScrollRect Rect;

    private float startPosition;
    private void Start()
    {
        Rect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        startPosition = Rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
