using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Duration_Dot_Data : Effect_Duration_Data
{
    public float damage;
    public float interval;
    public override Effect CreateInstance()
    {
        return new Effect_Duration_Dot(name);
    }
}
