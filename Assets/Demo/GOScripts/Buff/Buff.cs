using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Buff
{
    public delegate void EffectHandler();
    public EffectHandler OnEffect;
    public EffectHandler OnAdded;
    public EffectHandler OnRemoved;

    public bool effectOnSelf;

    [field:NonSerialized]
    public GameObject Target { get; set; }

    [field: NonSerialized]
    public GameObject Instigator { get; set; }

    public Buff() { }

    public Buff(bool effectOnSelf) {
        this.effectOnSelf = effectOnSelf;
    }


    public virtual void Apply()
    {
        OnEffect?.Invoke();
    }

    public virtual void Init(GameObject instigator, GameObject target)
    {
        Instigator = instigator;
        if (effectOnSelf)
            Target = instigator;
        else
            Target = target;
    }

    public virtual void Added()
    {
        OnAdded?.Invoke();
    }

    public virtual void Removed()
    {
        OnRemoved?.Invoke();
    }
}
