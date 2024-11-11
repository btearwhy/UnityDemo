using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_Duration : Effect
{
    
    public float Duration { get; set; }

    public bool IsInfinite
    {
        get => Duration < 0;
        set
        {
            if (value)
            {
                Duration = -1;
            }
        }
    }

    public Effect_Duration() { }
    public Effect_Duration(string name):base(name)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Effect_Duration_Data effect_data = (Effect_Duration_Data)this.effect_data;
        IsInfinite = effect_data.isInfinite;
        if (!IsInfinite)
        {
            Duration =  effect_data.duration;
        }
    }

    public override void Added()
    {
        base.Added();
        if (!IsInfinite)
        {
            BattleSystem battleSystem = Context.Target.GetComponent<BattleSystem>();
            battleSystem.StartCoroutine(RemoveAfterSeconds(Duration));
        }
    }

    IEnumerator RemoveAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Context.Target.GetComponent<BattleSystem>().RemoveEffect(this);
    }
}
