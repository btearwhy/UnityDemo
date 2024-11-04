using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff_Duration : Buff
{
    public float Duration { get; set; }

    public Buff_Duration() { }
    public Buff_Duration(float duration)
    {
        Duration = duration;
    }
}