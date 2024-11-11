using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Duration_Data : Effect_Data
{
    public bool isInfinite;
    public float duration;
    // Start is called before the first frame update

    public override Effect CreateInstance()
    {
        return new Effect_Duration(name);
    }
}
