using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Duration_AttackAttached_Data : Effect_Duration_Data
{
    public Effect_Data AttachedEffect;

    public override Effect CreateInstance()
    {
        return new Effect_AttachAttack(name);
    }
}
