using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff_Data : ScriptableObject
{
    public Sprite icon;

    public bool effectOnSelf;

    public virtual Buff CreateInstance()
    {
        return new Buff(effectOnSelf);
    }
}

