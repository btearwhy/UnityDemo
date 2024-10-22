using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Kine : MonoBehaviour, Movement
{

    public float maxSpeed;
    public float speed;
    public float accelerate;
    public float decelerate;
    public float turnSpeed;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<Animator>().SetFloat("Forward", speed / maxSpeed);
        Vector3 crossResult = Vector3.Cross(transform.forward, direction);
        transform.GetChild(0).GetComponent<Animator>().SetFloat("Turn", Vector3.Dot(Vector3.up, crossResult) * crossResult.magnitude);
    }

    public void FixedUpdate()
    {
        

        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), turnSpeed * Time.fixedDeltaTime);
    }

    public void Decelerate()
    {
        speed -= decelerate * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    public void Move(Vector3 moveDirection)
    {
        speed += accelerate * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        direction = moveDirection;



    }

}
