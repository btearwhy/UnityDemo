using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public List<Buff> buffs;
    public bool isDamage;


    // Start is called before the first frame update

    public virtual void Apply(GameObject instigator, GameObject target)
    {

    }
}
