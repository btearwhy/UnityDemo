using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_RigidBody :Movement
{

    public float actualSpeed;


    public Vector3 movement;
    public Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

    private bool acc = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", actualSpeed);

    }

    public void Move(Vector3 moveDirection)
    {
        acc = true;
        movement = moveDirection;
        speed += accelerate * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

    }

    private void FixedUpdate()
    {
        moveCharacter(movement);    
    }

    private void moveCharacter(Vector3 movement)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(!acc)
        {
            speed -= accelerate;
            speed = Mathf.Clamp(speed, 0, maxSpeed);

            rb.AddForce(-rb.velocity.normalized * speed);
        }
        else
        {
            rb.AddForce(movement * speed);
        }
        actualSpeed = rb.velocity.magnitude;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

    }

    public void Decelerate()
    {
        acc = false;
    }
}
