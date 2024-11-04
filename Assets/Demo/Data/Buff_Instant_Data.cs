using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff_Instant_Data : Buff_Data
{
    public readonly int times;

    public readonly List<Effect_Data> effectsOnEnemy;
    public readonly List<Effect_Data> effectsOnSelf;

    public override Buff CreateInstance()
    {
        List<Effect> effectsOnEnemy = new List<Effect>();
        this.effectsOnEnemy.ForEach(effect => effectsOnEnemy.Add(effect.CreateInstance()));
        List<Effect> effectsOnSelf = new List<Effect>();
        this.effectsOnSelf.ForEach(effect => effectsOnSelf.Add(effect.CreateInstance()));
        return new Buff_Instant(times, effectsOnEnemy, effectsOnSelf);
    }
}

