using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public enum FlipRegion
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
    FlipRegion? flipStart;
    float circleRadius;
    float circleRadius2;

    public RectTransform LeftTopPage;
    public RectTransform LeftBottomPage;
    public RectTransform RightTopPage;
    public RectTransform RightBottomPage;

    public RectTransform FlippingPlane;

    public RectTransform CurrentPlane;
    private RectTransform FlippingBackPlane;
    private RectTransform NextPlane;

    public RectTransform ClippingPlaneFlipPage;

    public RectTransform ClippingPlaneNewPage;

    public GameObject mark;
    public Canvas canvas;

    public RectTransform BookPanel;

    public InputActions inputActions;
    private Vector3 mouseFocusInBookSpace;
    private Vector3 actualFlippingPoint;
    private Finger FlippingBookFinger;

    private bool stillFinishing = false;
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void Start()
    {
        Init();
    }
    private void HandleFingerMove(Finger TouchedFinger)
    {
        if (TouchedFinger == FlippingBookFinger)
        {
            mouseFocusInBookSpace = transformPoint(TouchedFinger.screenPosition);
            UpdateBookWithLocalFocus();
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        Debug.Log("???");
        if (FlippingBookFinger == null && !stillFinishing)
        {
            bool canFlip = true;
            Vector2 cursorOnBook = transformPoint(TouchedFinger.screenPosition);
            flipStart = GetStartRegion(cursorOnBook);
            
            if (flipStart == FlipRegion.LeftTop && LeftTopPage != null)
            {
                NextPlane = LeftTopPage;
            }
            else if (flipStart == FlipRegion.LeftBottom && LeftBottomPage != null)
            {
                NextPlane = LeftBottomPage;
            }
            else if (flipStart == FlipRegion.RightBottom && RightBottomPage != null)
            {
                NextPlane = RightBottomPage;
            }
            else if (flipStart == FlipRegion.RightTop && RightTopPage != null)
            {
                NextPlane = RightTopPage;

            }
            else canFlip = false;
            if (flipStart.HasValue && canFlip)
            {
                
                StartCoroutine(TakeShotOfRect(NextPlane));

                FlippingBookFinger = TouchedFinger;
            }

        }
    }

    public IEnumerator TakeShotOfRect(RectTransform page)
    {
        yield return new WaitForEndOfFrame();

        Vector2 temp = page.position;
        int width = System.Convert.ToInt32(page.rect.width);
        int height = System.Convert.ToInt32(page.rect.height);

        float startX = temp.x - page.rect.width / 2;
        float startY = temp.y - page.rect.height / 2;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        FlippingPlane.GetComponent<Image>().overrideSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

        NextPlane.gameObject.SetActive(false);
    }
    private void HandleFingerUp(Finger TouchedFinger)
    {
        if (TouchedFinger == FlippingBookFinger)
        {
            FlippingBookFinger = null;
            Vector2 cursorOnBook = transformPoint(TouchedFinger.screenPosition);
            if ((flipStart == FlipRegion.LeftBottom || flipStart == FlipRegion.LeftTop) && cursorOnBook.x < 0 
                || (flipStart == FlipRegion.RightBottom || flipStart == FlipRegion.RightTop) && cursorOnBook.x > 0)
            {

                DropSlip();
            }
            else
            {
                FinishSlip();
            }
            stillFinishing = true;
        }
    }

    private void DropSlip()
    {
        StartCoroutine(Finish(flipStart.Value, false));
    }

    private void FinishSlip()
    {
        FlipRegion flipTarget;
        if(flipStart == FlipRegion.LeftBottom)
        {
            flipTarget = FlipRegion.RightBottom;
        }
        else if (flipStart == FlipRegion.LeftTop)
        {
            flipTarget = FlipRegion.RightTop;
        }
        else if (flipStart == FlipRegion.RightTop)
        {
            flipTarget = FlipRegion.LeftTop;
        }
        else
        {
            flipTarget = FlipRegion.LeftBottom;
        }
        StartCoroutine(Finish(flipTarget, true));
    }

    public IEnumerator Finish(FlipRegion targetRegion, bool flipped)
    {
        EnhancedTouchSupport.Disable();
        Vector3 target;
        if (targetRegion == FlipRegion.RightBottom)
        {
            target = bottomRight;
        }
        else if (targetRegion == FlipRegion.RightTop)
        {
            target = topRight;
        }
        else if (targetRegion == FlipRegion.LeftBottom)
        {
            target = bottomLeft;
        }
        else target = topLeft;
        while (Vector3.Distance(actualFlippingPoint, target) > 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            mouseFocusInBookSpace = target;
            UpdateBookWithLocalFocus();
        }
        stillFinishing = false;
        if (flipped)
        {
            CurrentPlane.gameObject.SetActive(false);
            CurrentPlane = NextPlane;
            CurrentPlane.SetParent(BookPanel);
        }
        NextPlane = null;
/*        Image flipImage = FlippingPlane.GetComponent<Image>();
        Texture2D.Destroy(flipImage.sprite.texture);
        Sprite.Destroy(flipImage.sprite);*/
        EnhancedTouchSupport.Enable();    
    }



    public FlipRegion? GetStartRegion(Vector2 local)
    {
        FlipRegion? flip = null;
        if (local.x > 0.8 * bottomRight.x && local.x < bottomRight.x)
        {
            if (local.y > 0 && local.y < topSpine.y)
            {
                flip = FlipRegion.RightTop;
            }
            else if (local.y < 0 && local.y > bottomSpine.y)
            {
                flip = FlipRegion.RightBottom;
            }
        }
        else if (local.x < 0.8 * bottomLeft.x && local.x > bottomLeft.x)
        {
            if (local.y > 0 && local.y < topSpine.y)
            {
                flip = FlipRegion.LeftTop;
            }
            else if (local.y < 0 && local.y > bottomSpine.y)
            {
                flip = FlipRegion.LeftBottom;
            }
        }
        return flip;
    }





    public void Init()
    {
        bottomSpine = new Vector3(0, -BookPanel.rect.height / 2);
        bottomRight = new Vector3(BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        bottomLeft = new Vector3(-BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        topSpine = new Vector3(0, BookPanel.rect.height / 2);
        topLeft = new Vector3(-BookPanel.rect.width / 2, BookPanel.rect.height / 2);
        topRight = new Vector3(BookPanel.rect.width / 2, BookPanel.rect.height / 2);

        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        circleRadius = pageWidth;
        circleRadius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);

        //ClippingPlaneNewPage.GetComponent<RectTransform>().sizeDelta = new Vector2(pageWidth, pageHeight + pageHeight * 2);
        //ClippingPlaneFlipPage.sizeDelta = new Vector2(pageWidth, pageHeight);

        ClippingPlaneFlipPage.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);
        ClippingPlaneNewPage.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);
    }

    public Vector3 transformPoint(Vector3 mouseScreenPos)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 mouseWorldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, canvas.planeDistance));
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseWorldPos);

            return localPos;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 globalEBR = transform.TransformPoint(bottomRight);
            Vector3 globalEBL = transform.TransformPoint(bottomLeft);
            Vector3 globalSt = transform.TransformPoint(topSpine);
            Plane p = new Plane(globalEBR, globalEBL, globalSt);
            float distance;
            p.Raycast(ray, out distance);
            Vector2 localPos = BookPanel.InverseTransformPoint(ray.GetPoint(distance));
            return localPos;
        }
        else
        {
            //Screen Space Overlay
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseScreenPos);
            return localPos;
        }
    }

    public void UpdateBookWithLocalFocus()
    {
        actualFlippingPoint = Vector3.Lerp(actualFlippingPoint, getActualPoint(), Time.deltaTime * 10);
        Vector3 startPoint;
        Vector3 targetPoint;
        if (flipStart == FlipRegion.LeftBottom)
        {
            startPoint = bottomLeft;
            targetPoint = bottomRight;
        }
        else if(flipStart == FlipRegion.LeftTop)
        {
            startPoint = topLeft;
            targetPoint = topRight;
        }
        else if(flipStart == FlipRegion.RightBottom)
        {
            startPoint = bottomRight;
            targetPoint = bottomLeft;
        }
        else
        {
            startPoint = topRight;
            targetPoint = topLeft;
        }

        Vector3 displacement = actualFlippingPoint - startPoint;
        float angle = Mathf.Atan(displacement.y / displacement.x) * Mathf.Rad2Deg;
        //positive = counter clockwise



        FlippingPlane.rotation = Quaternion.AngleAxis(2 * angle, BookPanel.forward);
        Vector3 widthSideNormal = Quaternion.AngleAxis(angle, BookPanel.forward) * (-displacement).normalized;
        
        Vector3 heightSideNormal = Quaternion.AngleAxis(angle + 90, BookPanel.forward) * (-displacement).normalized;
        if(flipStart == FlipRegion.LeftBottom || flipStart == FlipRegion.RightTop)
        {
            heightSideNormal = Quaternion.AngleAxis(angle - 90, BookPanel.forward) * (-displacement).normalized;
        }
        Vector3 newCenter = widthSideNormal * BookPanel.rect.width / 2.0f + heightSideNormal * BookPanel.rect.height / 2.0f + actualFlippingPoint;
       /* Instantiate(mark, BookPanel.TransformPoint(newCenter), Quaternion.identity);*/
        FlippingPlane.position = BookPanel.TransformPoint(newCenter);

        Vector3 midPoint = (startPoint + actualFlippingPoint) / 2f;
        ClippingPlaneFlipPage.transform.localPosition = displacement.normalized * ClippingPlaneFlipPage.sizeDelta.y / 2.0f + midPoint;
        ClippingPlaneFlipPage.transform.localEulerAngles =  new Vector3(0, 0, angle - 90);
        FlippingPlane.SetParent(ClippingPlaneFlipPage);

        NextPlane.gameObject.SetActive(true);


        ClippingPlaneNewPage.transform.localPosition = -displacement.normalized * ClippingPlaneNewPage.sizeDelta.y / 2.0f + midPoint;
        ClippingPlaneNewPage.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
        NextPlane.SetParent(ClippingPlaneNewPage);
        NextPlane.position = BookPanel.position;
        NextPlane.rotation = BookPanel.rotation;

        ClippingPlaneNewPage.SetAsLastSibling();
        ClippingPlaneFlipPage.SetAsLastSibling();
    }

    private Vector3 getActualPoint()
    {
        Vector3 revisedCursor = mouseFocusInBookSpace;
        Vector3 circleCenterNear = bottomSpine;
        Vector3 circleCenterFar = topSpine;
        Vector3 rightNear = bottomRight;
        if(flipStart == FlipRegion.LeftBottom || flipStart == FlipRegion.RightBottom)
        {
            circleCenterNear = bottomSpine;
            circleCenterFar = topSpine;
            rightNear = bottomRight;
        }
        else if(flipStart == FlipRegion.LeftTop || flipStart == FlipRegion.RightTop)
        {
            circleCenterNear = topSpine;
            circleCenterFar = bottomSpine;
            rightNear = topRight;
        }
        Vector3 centerNear2Cursor = revisedCursor - circleCenterNear;

        if(centerNear2Cursor.magnitude > circleRadius)
        {
            revisedCursor = circleCenterNear + centerNear2Cursor.normalized * circleRadius;
        }
        Vector3 centerFar2Cursor = revisedCursor - circleCenterFar;
        if(centerFar2Cursor.magnitude > circleRadius2)
        {
            revisedCursor = circleCenterFar + centerFar2Cursor.normalized * circleRadius2;
        }
        return revisedCursor;
    }

   /* private Vector3 Calc_C_Position(Vector3 followLocation)
    {
        Vector3 c;
        mouseFocusInBookSpace = followLocation;
        float F_SB_dy = mouseFocusInBookSpace.y - bottomSpine.y;
        float F_SB_dx = mouseFocusInBookSpace.x - bottomSpine.x;
        float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
        Vector3 r1 = new Vector3(circleRadius * Mathf.Cos(F_SB_Angle), circleRadius * Mathf.Sin(F_SB_Angle), 0) + sb;

        float F_SB_distance = Vector2.Distance(mouseFocusInBookSpace, bottomSpine);
        if (F_SB_distance < circleRadius)
            c = mouseFocusInBookSpace;
        else
            c = r1;
        float F_ST_dy = c.y - topSpine.y;
        float F_ST_dx = c.x - topSpine.x;
        float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);
        Vector3 r2 = new Vector3(circleRadius2 * Mathf.Cos(F_ST_Angle),
           circleRadius2 * Mathf.Sin(F_ST_Angle), 0) + topSpine;
        float C_ST_distance = Vector2.Distance(c, topSpine);
        if (C_ST_distance > circleRadius2)
            c = r2;
        return c;
    }*/
}
