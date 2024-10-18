using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float armLength;
    public float minArmLength = 1.0f;
    public float maxArmLength = 15.0f;
    public float cameraVertical = 45;
    public float cameraHorizontal = 180;
    public float cameraTilt = 0;

    public float rotateSpeed = 40;
    public float zoomSpeed = 2;

    public GameObject character;

    public InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        armLength = 7.0f;
        minArmLength = 1.0f;
        maxArmLength = 15.0f;
        cameraVertical = 45;
        cameraHorizontal = 180;
        cameraTilt = 0;

        rotateSpeed = 10;
        zoomSpeed = 2;
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
        transform.Rotate(cameraVertical, cameraHorizontal, cameraTilt);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * armLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main != null && character != null)
        {
            CameraControl();
            OperationControl();
            
        }
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
        cameraHorizontal += cameraVector2f.x * rotateSpeed * Time.deltaTime;
        armLength -= cameraVector2f.y * zoomSpeed * Time.deltaTime;
        armLength = Mathf.Clamp(armLength, minArmLength, maxArmLength);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.up * armLength;
        }
        transform.localRotation = Quaternion.Euler(cameraVertical, cameraHorizontal, cameraTilt);
    }
}
