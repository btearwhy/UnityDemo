using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Effect_Attack : Effect
{

    public Effect_Attack() { }
    public Effect_Attack(string data)
    {
        this.data = data;
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    // Start is called before the first frame update

    public override void Apply(GameObject instigator, GameObject target)
    {
        base.Apply(instigator, target);

        if (target.TryGetComponent<AttributeSet>(out AttributeSet targetAttributeSet) && instigator.TryGetComponent<AttributeSet>(out AttributeSet instigatorAttributeSet))
        {
            float damage = instigatorAttributeSet.attack - targetAttributeSet.defense;
            if (damage > 0)
            {
                if (target.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
                {
                    battleSystem.HitFlash(Color.white);
                }
            }
            targetAttributeSet.DealDamage(instigator, damage);
        }
    }
}
