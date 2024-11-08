using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviourPun
{
    public bool canMove;
    public bool canRotate;
    public float maxSpeed;
    public float speed;
    public float accelerate;
    public float decelerate;
    public float turnSpeed;
    public bool shouldStop;

    public void Awake()
    {
        if(TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            attributeSet.OnCurrentMaxSpeedChanged += (maxSpeed) =>
            {
                this.maxSpeed = maxSpeed;
                this.speed = Mathf.Clamp(speed, 0, maxSpeed);
            };

        }
    }



    private Vector3 direction;
    // Start is called before the first frame update
    private void Start()
    {
        canMove = true;
        canRotate = true;
        shouldStop = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (photonView.IsMine)
        {
            transform.GetChild(0).GetComponent<Animator>().SetFloat("Forward", speed / maxSpeed);
            Vector3 crossResult = Vector3.Cross(transform.forward, direction);
            //transform.GetChild(0).GetComponent<Animator>().SetFloat("Turn", Vector3.Dot(Vector3.up, crossResult) * crossResult.magnitude);
            if (canRotate)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), turnSpeed * Time.deltaTime);
            }

            if (shouldStop)
            {
                Decelerate();
            }
        }
    }


    public void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
        }
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), turnSpeed * Time.fixedDeltaTime);
    }

    public void Decelerate()
    {
        speed -= decelerate * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    public void Move(Vector3 moveDirection)
    {
        if (canMove)
        {
            speed += accelerate * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
        }
        direction = moveDirection;



    }

    public void StopTranslation()
    {
        shouldStop = true;
        canMove = true;
    }

    public void ResetStatus()
    {
        shouldStop = false;
        canMove = true;
        canRotate = true;
    }
}
