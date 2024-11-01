using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : ScriptableObject
{
    GameObject target;

    public void Init(GameObject target)
    {
        this.target = target;
    }

    public virtual void OnAdded()
    {
        
    }

    public virtual void OnRemoved()
    {

    }
}
