using System;
using UnityEngine;


public class Ability : ScriptableObject
{


    public Sprite icon;

    public GameObject character { get; set; }
    public Movement movement;
    public Animator animator;
    public virtual void Fire(Transform transform, Transform trans_projectileSpawnSocket) { }

    public virtual void Init(GameObject go)
    {
        character = go;
        movement = go.GetComponent<Movement>();
        animator = go.GetComponentInChildren<Animator>();
    }


    internal virtual void Pressed()
    {
        
    }

    internal virtual void Held()
    {
        
    }

    internal virtual void Released()
    {
        
    }

    internal virtual void HandleAnimationEvent(string dispatch)
    {
        
    }

    internal virtual void End() { }
}
