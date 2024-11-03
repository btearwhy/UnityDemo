using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_Damage : Effect
{

    public override void Apply(GameObject instigator, GameObject target)
    {
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