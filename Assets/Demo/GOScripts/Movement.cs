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
    public virtual void Move(Vector3 Direction)
    {

    }

    public virtual void Decelerate()
    {

    }



    void Update()
    {
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
