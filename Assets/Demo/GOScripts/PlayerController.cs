using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float armLength;
    public float minArmLength;
    public float maxArmLength;
    public float cameraVertical;
    public float cameraHorizontal;
    public float cameraTilt;

    public float rotateSpeed;
    public float zoomSpeed;
    public Camera playerCamera;
    public GameObject character;

    public UI_Controller_BattleHUD HUD;
    public InputActions inputActions;

    public Vector2 MoveInput;

    
    public FloatingJoystick joyStick;

    private Finger MovementFinger;
    private Vector2 MovementAmount;

    private Finger CameraFinger;
    private Finger AnotherCameraFinger = null;
    private Vector2 CameraAmount;
    private Vector2 CameraFirstTouchedPos;
    private float CameraFingerMoveMaxDistance = 150;
    private float initialDisBetweenTwoFingers;
    private float distance = 0;
    private float TwoFingerMoveMaxDistance = 150;
    private void Awake()
    {
        armLength = 7;
        minArmLength = 3;
        maxArmLength = 8;
        rotateSpeed = 100;
        zoomSpeed = 3;

        inputActions = new InputActions();
        inputActions.KeyboardandMouse.Attack.performed += callBackContext =>
        {
            if (callBackContext.interaction is TapInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionPressed(0);
            }
            else if (callBackContext.interaction is HoldInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionHeld(0);
            }
        };
        inputActions.KeyboardandMouse.Attack.canceled += callbackContext =>
        {
            if (callbackContext.interaction is HoldInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionReleased(0);
            }
        };

        inputActions.KeyboardandMouse.Absorb.performed += callBackContext =>
        {
            if (callBackContext.interaction is TapInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionPressed(1);
            }
            else if (callBackContext.interaction is HoldInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionHeld(1);
            }
        };
        inputActions.KeyboardandMouse.Absorb.canceled += callbackContext =>
        {
            if (callbackContext.interaction is HoldInteraction)
            {
                character.GetComponent<AbilitySystem>().ActionReleased(1);
            }
        };
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;

        inputActions.KeyboardandMouse.Disable();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;

        inputActions.KeyboardandMouse.Enable();
    }

    private void HandleFingerMove(Finger TouchedFinger)
    {
        
        if(TouchedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joyStick.joyStickSize.x / 2f;
            ETouch.Touch currentTouch = TouchedFinger.currentTouch;

            if(Vector2.Distance(currentTouch.screenPosition, joyStick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joyStick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joyStick.RectTransform.anchoredPosition;
            }
            joyStick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }
        else if(CameraFinger != null && AnotherCameraFinger != null && (TouchedFinger == CameraFinger || TouchedFinger == AnotherCameraFinger))
        {
            float newDistance = Vector2.Distance(CameraFinger.screenPosition, AnotherCameraFinger.screenPosition);
            distance = newDistance - initialDisBetweenTwoFingers;
            if (distance > TwoFingerMoveMaxDistance) distance = TwoFingerMoveMaxDistance;
            else if (distance < -TwoFingerMoveMaxDistance) distance = -TwoFingerMoveMaxDistance;
            CameraAmount = new Vector2(CameraAmount.x, distance / TwoFingerMoveMaxDistance);
        }
        else if(TouchedFinger == CameraFinger)
        {
            Vector2 virtualPosition;
            ETouch.Touch currentTouch = TouchedFinger.currentTouch;
            if (Vector2.Distance(currentTouch.screenPosition, CameraFirstTouchedPos) > CameraFingerMoveMaxDistance)
            {
                virtualPosition = (currentTouch.screenPosition - CameraFirstTouchedPos).normalized * CameraFingerMoveMaxDistance;
            }
            else
            {
                virtualPosition = currentTouch.screenPosition - CameraFirstTouchedPos;
            }
            Vector2  displace = virtualPosition / CameraFingerMoveMaxDistance;
            CameraAmount = new Vector2(displace.x, CameraAmount.y);
        }
    }

    private void HandleFingerUp(Finger TouchedFinger)
    {
        if(TouchedFinger == MovementFinger)
        {
            MovementFinger = null;
            joyStick.RectTransform.anchoredPosition = Vector2.zero;
            joyStick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
        else if(TouchedFinger == CameraFinger)
        {
            CameraFinger = null;
            CameraFirstTouchedPos = Vector2.zero;
            CameraAmount = Vector2.zero;
        }
        else if(TouchedFinger == AnotherCameraFinger)
        {
            AnotherCameraFinger = null;
            distance = 0;
            CameraAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (IsClickOnUI(TouchedFinger.screenPosition))
        {
            return;
        }
        if (MovementFinger == null && TouchedFinger.screenPosition.x < Screen.width / 2.0f && TouchedFinger.screenPosition.y  < Screen.height * 2.0f / 3.0f)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            joyStick.gameObject.SetActive(true);
            joyStick.RectTransform.sizeDelta = joyStick.joyStickSize;
            joyStick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
        if(CameraFinger == null && TouchedFinger.screenPosition.x > Screen.width / 2.0f)
        {
            CameraFinger = TouchedFinger;
            CameraAmount = Vector2.zero;
            CameraFirstTouchedPos = TouchedFinger.screenPosition;
        }
        else if(CameraFinger != null && TouchedFinger.screenPosition.x > Screen.width / 2.0f)
        {
            AnotherCameraFinger = TouchedFinger;
            initialDisBetweenTwoFingers = Vector2.Distance(CameraFinger.screenPosition, AnotherCameraFinger.screenPosition);
        }
        
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if(startPosition.x < joyStick.joyStickSize.x / 2)
        {
            startPosition.x = joyStick.joyStickSize.x / 2;
        }

        if(startPosition.y < joyStick.joyStickSize.y / 2)
        {
            startPosition.y = joyStick.joyStickSize.y / 2;
        }
        else if(startPosition.y > Screen.height - joyStick.joyStickSize.y / 2)
        {
            startPosition.y = Screen.height - joyStick.joyStickSize.y / 2;
        }

        return startPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(cameraVertical, cameraHorizontal, cameraTilt);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * armLength;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(playerCamera != null && character != null)
        {
            CameraControl();
            MovementControl();
            OperationControl();
        }
    }

    private void OperationControl()
    {
/*        if (inputActions.KeyboardandMouse.Attack.inProgress)
        {
            Debug.Log("attack progress");
            character.GetComponent<AbilitySystem>().ActionHeld(0);
        }
        if (inputActions.KeyboardandMouse.Attack.triggered)
        {
            Debug.Log("attack trigger");
            character.GetComponent<AbilitySystem>().ActionPressed(0);
        }*/
/*        if (inputActions.KeyboardandMouse.Absorb.inProgress)
        {
            character.GetComponent<AbilitySystem>().ActionHeld(0) ;
        }*/
    }

    private void MovementControl()
    {
        Vector2 moveVector2f = inputActions.KeyboardandMouse.Movement.ReadValue<Vector2>();
        if(moveVector2f == Vector2.zero) moveVector2f = MovementAmount;
        if (moveVector2f != Vector2.zero)
        {
            Vector3 fowardDirection = Vector3.Normalize(Vector3.Scale(-transform.up, new Vector3(1, 0, 1)));
            Vector3 rightDirection = Vector3.Cross(fowardDirection, Vector3.up);

            Vector3 moveDirection = Vector3.Normalize(fowardDirection * moveVector2f.y + rightDirection * -moveVector2f.x);

            character.GetComponent<Movement>().Move(moveDirection);
        }
        else if (character.GetComponent<Movement>().speed > 0)
        {
            character.GetComponent<Movement>().Decelerate();
        }
    }

    private void CameraControl()
    {
        /*Vector2 cameraVector2f = inputActions.KeyboardandMouse.CameraControl.ReadValue<Vector2>();*/
        Vector2 cameraVector2f = CameraAmount;
        transform.position = character.transform.position;
        cameraHorizontal += cameraVector2f.x * rotateSpeed * Time.deltaTime;
        armLength -= cameraVector2f.y * zoomSpeed * Time.deltaTime;
        armLength = Mathf.Clamp(armLength, minArmLength, maxArmLength);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * armLength;
        }
        transform.localRotation = Quaternion.Euler(cameraVertical, cameraHorizontal, cameraTilt);
    }

    public bool IsClickOnUI(Vector2 ScreenPosition)
    {
        foreach(Button button in HUD.buttons)
        {
            Vector2[] polygon = new Vector2[4];
            Vector3 position = button.transform.position;
            Vector2 size = button.GetComponent<RectTransform>().sizeDelta / 2.0f ;
            polygon[0] = new Vector2(position.x - size.x, position.y - size.y);
            polygon[1] = new Vector2(position.x + size.x, position.y - size.y);
            polygon[0] = new Vector2(position.x + size.x, position.y + size.y);
            polygon[0] = new Vector2(position.x - size.x, position.y + size.y);

            if(ScreenPosition.x > position.x - size.x && ScreenPosition.x < position.x + size.x && ScreenPosition.y > position.y - size.y && ScreenPosition.y < position.y + size.y)
            {
                return true;
            }

            /*bool result = false;
            int j = polygon.Length - 1;
            for (int i = 0; i < polygon.Length; i++)
            {
                if (polygon[i].y < ScreenPosition.y && polygon[j].y >= ScreenPosition.y || polygon[j].y < ScreenPosition.y && polygon[i].y >= ScreenPosition.y)
                {
                    if (polygon[i].x + (ScreenPosition.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < ScreenPosition.x)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            if (result) return result;*/
        }
        return false;
    }

    private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);
    private static Vector4[] MakeUnitSphere(int len)
    {
        Debug.Assert(len > 2);
        var v = new Vector4[len * 3];
        for (int i = 0; i < len; i++)
        {
            var f = i / (float)len;
            float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
            float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
            v[0 * len + i] = new Vector4(c, s, 0, 1);
            v[1 * len + i] = new Vector4(0, c, s, 1);
            v[2 * len + i] = new Vector4(s, 0, c, 1);
        }
        return v;
    }
    public static void DrawSphere(Vector4 pos, float radius, Color color)
    {
        Vector4[] v = s_UnitSphere;
        int len = s_UnitSphere.Length / 3;
        for (int i = 0; i < len; i++)
        {
            var sX = pos + radius * v[0 * len + i];
            var eX = pos + radius * v[0 * len + (i + 1) % len];
            var sY = pos + radius * v[1 * len + i];
            var eY = pos + radius * v[1 * len + (i + 1) % len];
            var sZ = pos + radius * v[2 * len + i];
            var eZ = pos + radius * v[2 * len + (i + 1) % len];
            Debug.DrawLine(sX, eX, color, 5);
            Debug.DrawLine(sY, eY, color, 5);
            Debug.DrawLine(sZ, eZ, color, 5);
        }
    }
}
