using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FlipStart
{
    LeftTop,
    LeftBottom,
    RightTop,
    RightBottom
}
public class TurnPage : MonoBehaviour
{
    Vector3 bottomLeft;
    Vector3 bottomRight;
    Vector3 bottomSpine;
    Vector3 topLeft;
    Vector3 topRight;
    Vector3 topSpine;

    float circleRadius;
    float circleRadius2;

    public GameObject Left;
    public GameObject Right;
    public GameObject FlippingLeft;
    public GameObject FlippingRight;

    public RectTransform BookPanel;

    public void Init()
    {
        bottomSpine = new Vector3(0, -BookPanel.rect.height / 2);
        bottomRight = new Vector3(BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        bottomLeft = new Vector3(-BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        topSpine = new Vector3(0, BookPanel.rect.height / 2);
        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        circleRadius = pageWidth;
        circleRadius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
    }
}
