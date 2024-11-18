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

    

    public RectTransform FlippingPlane;
    public RectTransform CurrentPlane;
    private RectTransform FlippingBackPlane;
    private RectTransform NextPlane;
    private RectTransform NextPlaneCopy;
    public RectTransform ClippingPlaneFlipPage;

    public RectTransform ClippingPlaneNewPage;
    public RectTransform CanvasPanelLists;
    public GameObject mark;
    public Canvas canvas;
    public RenderTexture renderTexture;
    public RectTransform BookPanel;
    public Camera ShotCamera;
    public InputActions inputActions;
    public AudioClip TurnPageSound;
    private Vector3 mouseFocusInBookSpace;
    private Vector3 actualFlippingPoint;
    private Finger FlippingBookFinger;

    private bool stillFinishing = false;

    private AudioSource audioSource;
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
        if (FlippingBookFinger == null && !stillFinishing)
        {
            Vector2 cursorOnBook = transformPoint(TouchedFinger.screenPosition);
            flipStart = GetStartRegion(cursorOnBook);
            Page page = CurrentPlane.GetComponent<Page>();
            if (flipStart.HasValue && GetNextPlane(flipStart.Value, page) != null)
            {
                /*                ShotCamera.enabled = true;
                                NextPlane.gameObject.SetActive(true);
                                NextPlane.SetAsLastSibling();*/
                FlippingBookFinger = TouchedFinger;
                mouseFocusInBookSpace = transformPoint(TouchedFinger.screenPosition);
                PrepareClipAfterCursorSet();

            }

        }
    }

    private void PrepareClipAfterCursorSet()
    {
        actualFlippingPoint = getActualPoint();

        FlippingPlane.SetParent(ClippingPlaneFlipPage);
        NextPlane.SetParent(ClippingPlaneNewPage);

        NextPlaneCopy = Instantiate(NextPlane, FlippingPlane);
        NextPlaneCopy.GetComponent<Page>().isClonedForViewOnly = true;
    }

    private RectTransform GetNextPlane(FlipRegion flipStart, Page page)
    {
        if (flipStart == FlipRegion.LeftTop && page.LeftTopPage != null)
        {
            NextPlane = page.LeftTopPage;
        }
        else if (flipStart == FlipRegion.LeftBottom && page.LeftBottomPage != null)
        {
            NextPlane = page.LeftBottomPage;
        }
        else if (flipStart == FlipRegion.RightBottom && page.RightBottomPage != null)
        {
            NextPlane = page.RightBottomPage;
        }
        else if (flipStart == FlipRegion.RightTop && page.RightTopPage != null)
        {
            NextPlane = page.RightTopPage;
        }
        return NextPlane;
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

                DropFlip();
            }
            else
            {
                FinishFlip();
            }
            stillFinishing = true;
        }
    }

    private void DropFlip()
    {
        StartCoroutine(Finish(flipStart.Value, flipStart.Value));
    }

    private void FinishFlip()
    {
        FlipRegion flipTarget;
        Page page = CurrentPlane.GetComponent<Page>();
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
        StartCoroutine(Finish(flipStart.Value, flipTarget));
    }
    
    public IEnumerator Finish(FlipRegion startRegion, FlipRegion targetRegion)
    {
        EnhancedTouchSupport.Disable();
        bool flipped = startRegion != targetRegion;
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

        if (flipped)
        {
            //CurrentPlane.SetParent(CanvasPanelLists);
            CurrentPlane.localPosition = Vector3.zero;
            CurrentPlane.GetComponent<Page>().Leave(startRegion);
            CurrentPlane.gameObject.SetActive(false);
            NextPlane.SetParent(BookPanel);
            NextPlane.gameObject.SetActive(true);
            Page p = NextPlane.GetComponent<Page>();
            p.SetFlippedFrom(CurrentPlane.GetComponent<Page>());
            CurrentPlane = NextPlane;

            p.InitialOperation();
        }
        else
        {
            NextPlane.SetParent(BookPanel);
            NextPlane.localPosition = Vector3.zero;
            NextPlane.gameObject.SetActive(false);
        }
        ClippingPlaneFlipPage.gameObject.SetActive(false);
        ClippingPlaneNewPage.gameObject.SetActive(false);
        NextPlane = null;
        Destroy(NextPlaneCopy.gameObject);
        FlippingPlane.SetParent(BookPanel);
        FlippingPlane.gameObject.SetActive(false);
        NextPlaneCopy = null;
        /*Image flipImage = FlippingPlane.GetComponent<Image>();*/
        //Texture2D.Destroy(flipImage.sprite.texture);
        //Sprite.Destroy(flipImage.sprite);
        stillFinishing = false;
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
        audioSource = GetComponent<AudioSource>();

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
        NextPlaneCopy.anchoredPosition = Vector3.zero;
        NextPlaneCopy.localRotation = Quaternion.identity;
        Vector3 midPoint = (startPoint + actualFlippingPoint) / 2f;
        ClippingPlaneFlipPage.transform.localPosition = displacement.normalized * (ClippingPlaneFlipPage.sizeDelta.y / 2.0f) + midPoint;
        ClippingPlaneFlipPage.transform.localEulerAngles =  new Vector3(0, 0, angle - 90);






        ClippingPlaneNewPage.transform.localPosition = -displacement.normalized * (ClippingPlaneNewPage.sizeDelta.y / 2.0f - 5) + midPoint;
        ClippingPlaneNewPage.transform.localEulerAngles = new Vector3(0, 0, angle - 90);

        NextPlane.position = BookPanel.position;
        NextPlane.rotation = BookPanel.rotation;
        NextPlane.localScale = Vector3.one;
        ClippingPlaneNewPage.SetAsLastSibling();
        ClippingPlaneFlipPage.SetAsLastSibling();

        ClippingPlaneFlipPage.gameObject.SetActive(true);
        ClippingPlaneNewPage.gameObject.SetActive(true);
        FlippingPlane.gameObject.SetActive(true);
        NextPlaneCopy.gameObject.SetActive(true);
        NextPlane.gameObject.SetActive(true);

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

    public void AutoFlip(FlipRegion StartRegion)
    {
        stillFinishing = true;
        FlipRegion EndRegion;
        if(StartRegion == FlipRegion.LeftBottom)
        {
            mouseFocusInBookSpace = bottomLeft;
            EndRegion = FlipRegion.RightBottom;
        }
        else if(StartRegion == FlipRegion.LeftTop)
        {
            mouseFocusInBookSpace = topLeft;
            EndRegion = FlipRegion.RightTop;
        }
        else if (StartRegion == FlipRegion.RightBottom)
        {
            mouseFocusInBookSpace = bottomRight;
            EndRegion = FlipRegion.LeftBottom;
        }
        else
        {
            mouseFocusInBookSpace = topRight;
            EndRegion = FlipRegion.LeftTop; 
        }

        if (GetNextPlane(StartRegion, CurrentPlane.GetComponent<Page>()) != null)
        {
            flipStart = StartRegion;
            PrepareClipAfterCursorSet();
            audioSource.PlayOneShot(TurnPageSound, AudioManager.soundRatio);
            StartCoroutine(Finish(StartRegion, EndRegion));
        }
    }
}
