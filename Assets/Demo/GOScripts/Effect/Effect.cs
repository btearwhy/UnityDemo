using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public Effect() { }
    public Effect(Effect_Data effect_data)
    {

    }
    public virtual void Apply(GameObject instigator, GameObject target)
    {

    }
}