using System;
using UnityEngine;


public class Ability_Data : ScriptableObject
{
    public Sprite icon;

    public string animStartStateName;
    public string animHeldStateName;
    public string animReleaseStateName;

    public virtual Ability CreateInstance()
    {
        return new Ability(name);
    }
}
