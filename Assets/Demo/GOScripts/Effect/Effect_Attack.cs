using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Effect_Attack : Effect
{
    public List<Buff> buffsOnSelf;
    public List<Buff> buffsOnTarget;
    // Start is called before the first frame update

    public override void Apply(GameObject instigator, GameObject target)
    {


        foreach(Buff buff in buffsOnSelf)
        {
            buff.Target = instigator;
            buff.Instigator = instigator;
            buff.OnAdded();
        }
        foreach(Buff buff in buffsOnTarget)
        {
            buff.Target = target;
            buff.Instigator = instigator;
            buff.OnAdded();
        }

        base.Apply(instigator, target);

        foreach (Buff buff in buffsOnSelf)
        {
            buff.OnRemoved();
        }
        foreach (Buff buff in buffsOnTarget)
        {
            buff.Target = target;
            buff.Instigator = instigator;
            buff.OnAdded();
        }
    }
}
