using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Effect_Attack : Effect_Instant
{

    public Effect_Attack() { }

    public Effect_Attack(string name) :base(name){ }
    public override void Initialize()
    {
        base.Initialize();
    }
    // Start is called before the first frame update


    public override void Apply()
    {
        base.Apply();

        if (Context.Target.TryGetComponent<AttributeSet>(out AttributeSet targetAttributeSet) && Context.Instigator.TryGetComponent<AttributeSet>(out AttributeSet instigatorAttributeSet))
        {
            float damage = instigatorAttributeSet.GetCurrentValue(AttributeType.Attack) - targetAttributeSet.GetCurrentValue(AttributeType.Defense);
            if (damage > 0)
            {
                if (Context.Target.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
                {
                    battleSystem.HitFlash(Color.white);
                    battleSystem.DealDamage(Context.Instigator, damage);
                }
            }
        }
    }
}
