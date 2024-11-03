using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public Buff() { }

    public Buff(Buff_Data buff_data) { }

    public GameObject Target { get; set; }

    public GameObject Instigator { get; set; }

    public void Init(GameObject target)
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
