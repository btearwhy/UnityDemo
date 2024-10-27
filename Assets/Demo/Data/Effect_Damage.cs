using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Damage : Effect
{
    public override void Apply(GameObject instigator, GameObject target)
    {
        if (target.TryGetComponent<AttributeSet>(out AttributeSet targetAttributeSet) && instigator.TryGetComponent<AttributeSet>(out AttributeSet instigatorAttributeSet))
        {
            float damage = instigatorAttributeSet.attack - targetAttributeSet.defense;
            if(damage > 0)
            {
                damage /= (instigator.transform.position - target.transform.position).magnitude; 
            }
            targetAttributeSet.DealDamage(instigator, damage); 
        }
    }
}
