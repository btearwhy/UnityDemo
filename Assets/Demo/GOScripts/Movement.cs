using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float turnSpeed;
    public float speed;
    public float maxSpeed;
    public float accelerate;

    public Vector3 movement;
    public Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", speed);

    }

    public void Move(Vector3 moveDirection)
    {
        movement = moveDirection;
        speed += accelerate * Time.deltaTime;
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }

        targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        /*if (GetComponent<Rigidbody>().velocity.magnitude <= 3)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }*/
        //transform.Translate(moveDirection * speed, Space.World);

    }

    private void FixedUpdate()
    {
        moveCharacter(movement);    
    }

    private void moveCharacter(Vector3 movement)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(movement == Vector3.zero)
        {
            float dampingFactor = 0.95f;
            rb.velocity = Vector3.Scale(rb.velocity, new Vector3(dampingFactor, dampingFactor, dampingFactor));
        }
        else 
            rb.AddForce(movement * speed);
        speed = rb.velocity.magnitude;
        //speed = GetComponent<Rigidbody>().velocity.magnitude;
        //rb.velocity = movement * speed  + new Vector3(0, rb.velocity.y, 0);
        
    }

    public void Deccelerate()
    {
        movement = Vector3.zero;
    }
}
