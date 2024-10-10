using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float turnSpeed;
    public float speed;
    public float maxSpeed;
    public float accelerate;

    private Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 moveDirection)
    {
        
        speed += accelerate * Time.deltaTime;
        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        }

        targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.Translate(moveDirection * speed, Space.World);
        GetComponent<Animator>().SetFloat("Speed", speed);
    }

    public void Deccelerate()
    {
        if (speed > 0)
        {
            speed -= accelerate * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            transform.Translate(transform.forward * speed, Space.World);
            GetComponent<Animator>().SetFloat("Speed", speed);
        }
        else
        {
            speed = 0;
        }
    }
}
