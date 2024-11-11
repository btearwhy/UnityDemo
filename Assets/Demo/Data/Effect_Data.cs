using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class Effect_Data : ScriptableObject
{
    public virtual Effect CreateInstance()
    {
        return new Effect(name);
    }
}

