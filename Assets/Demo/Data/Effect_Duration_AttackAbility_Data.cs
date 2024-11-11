using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Duration_AttackAbility_Data : Effect_Duration_Data
{
    public Ability_Attack_Data Ability_Attack_Data;

    public override Effect CreateInstance()
    {
        return new Effect_ChangeAttackAbility(name);
    }
}
