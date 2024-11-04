using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Decelerate_Data : Buff_Duration_Data
{
    public float percentage;

    public override Buff CreateInstance()
    {
        return new Buff_Decelerate(duration, percentage);
    }
}
