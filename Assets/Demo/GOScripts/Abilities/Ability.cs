using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public delegate void EventHandler();
    public EventHandler OnEnded;
    public EventHandler OnFired;

    public string animStartStateName;
    public string animHeldStateName;
    public string animReleaseStateName;

    [field: NonSerialized]
    public GameObject character;

    [field: NonSerialized]
    public Movement movement;
    [field: NonSerialized]
    public Animator animator;

    public string data;
    public Ability() { }

    public Ability(string data) {
        this.data = data;
        Initialize();
    }

    public virtual void Initialize()
    {
        Ability_Data ability_data = (Ability_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
        this.animStartStateName = ability_data.animStartStateName;
        this.animHeldStateName = ability_data.animHeldStateName;
        this.animReleaseStateName = ability_data.animReleaseStateName;
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

    internal virtual Ability_Data GetProtoData()
    {
        return (Ability_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
    }
}
