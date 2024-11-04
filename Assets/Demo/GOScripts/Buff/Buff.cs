using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Buff
{
    public delegate void EffectHandler();
    public EffectHandler OnEffect;


    [field:NonSerialized]
    public GameObject Target { get; set; }

    [field: NonSerialized]
    public GameObject Instigator { get; set; }

    public Buff() { }

    public Buff(Buff_Data buff_data) { }

    public virtual void Init(GameObject target)
    {
        Target = target;
    }

    public virtual void OnAdded()
    {
        
    }

    public virtual void OnRemoved()
    {

    }
}
