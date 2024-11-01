using Photon.Pun;
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
                if (target.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
                {
                    battleSystem.HitFlash(Color.white);
                }
            }
            if(instigator.TryGetComponent<PhotonView>(out PhotonView photonView))
            {
                targetAttributeSet.DealDamage(photonView.ControllerActorNr, damage);
            }
            else
            {
                targetAttributeSet.DealDamage(instigator, damage);
            } 
        }
    }
}
