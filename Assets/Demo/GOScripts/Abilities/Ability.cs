using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public string animStartStateName;
    public string animHeldStateName;
    public string animReleaseStateName;

    [field: NonSerialized]
    public GameObject character;

    [field: NonSerialized]
    public Movement movement;
    [field: NonSerialized]
    public Animator animator;

    
    public Ability() { }
    public Ability(string animStartStateName, string animHeldStateName, string animReleaseStateName)
    {
        this.animStartStateName = animStartStateName;
        this.animHeldStateName = animHeldStateName;
        this.animReleaseStateName = animReleaseStateName;
    }
    
    public virtual void Fire(Transform transform, Transform trans_projectileSpawnSocket) { }

    public virtual void Init(GameObject go)
    {
        character = go;
        movement = go.GetComponent<Movement>();
        animator = go.GetComponentInChildren<Animator>();
    }


    internal virtual void Pressed()
    {
        animator.Play(animStartStateName);
    }

    internal virtual void Held()
    {
        animator.Play(animHeldStateName);
    }

    internal virtual void Released()
    {
        animator.Play(animReleaseStateName);
    }

    internal virtual void HandleAnimationEvent(string dispatch)
    {

    }

    internal virtual void End() { }
}
