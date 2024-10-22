using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ability : ScriptableObject
{
    public string tiggerName;

    public virtual void StartAction() { }

    public virtual void FinishAction() { }
    // Start is called before the first frame update

    public virtual void Fire(Transform transform, Transform trans_projectileSpawnSocket) { }
}
