using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Movement 
{

    void Move(Vector3 Direction);

    void Decelerate();
}
