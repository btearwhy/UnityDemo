using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff_Duration_Data : Buff_Data
{
    public float duration;

    public override Buff CreateInstance()
    {
        return new Buff_Duration(duration);
    }
}


