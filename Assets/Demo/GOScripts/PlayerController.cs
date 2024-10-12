using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float ArmLength;

    public float CameraVertical = 45;
    public float CameraHorizontal = 180;
    public float CameraTilt = 0;

    public float RotateSpeed;
    public float ZoomSpeed;

    public GameObject character;

    public InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnDisable()
    {
        inputActions.KeyboardandMouse.Disable();
    }

    private void OnEnable()
    {
        inputActions.KeyboardandMouse.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = character.transform.position;
        transform.Rotate(CameraVertical, CameraHorizontal, CameraTilt);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * ArmLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
        OperationControl();
    }

    private void OperationControl()
    {
        Vector2 moveVector2f = inputActions.KeyboardandMouse.Movement.ReadValue<Vector2>();
        if(moveVector2f != Vector2.zero)
        {
            Debug.Log("second");
            Vector3 fowardDirection = Vector3.Normalize(Vector3.Scale(-transform.up, new Vector3(1, 0, 1)));
            Vector3 rightDirection = Vector3.Cross(fowardDirection, Vector3.up);

            Vector3 moveDirection = Vector3.Normalize(fowardDirection * moveVector2f.y + rightDirection * -moveVector2f.x);

            character.GetComponent<Movement>().Move(moveDirection);
            Rigidbody rb = character.GetComponent<Rigidbody>();
        }
        else
        {
            character.GetComponent<Movement>().Deccelerate();
        }
    }

    private void CameraControl()
    {
        Vector2 cameraVector2f = inputActions.KeyboardandMouse.CameraControl.ReadValue<Vector2>();
        transform.position = character.transform.position;
        CameraHorizontal += cameraVector2f.x * RotateSpeed * Time.deltaTime;
        ArmLength -= cameraVector2f.y * ZoomSpeed * Time.deltaTime;
        ArmLength = Mathf.Clamp(ArmLength, 1.0f, 10.0f);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * ArmLength;
        }
        transform.localRotation = Quaternion.Euler(CameraVertical, CameraHorizontal, CameraTilt);
    }
}
