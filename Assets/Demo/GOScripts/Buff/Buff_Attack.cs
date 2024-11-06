using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack : Buff
{
    public string attackName;

    private Ability ability;
    public Buff_Attack()
    {
    }

    public Buff_Attack(string attackName)
    {
        this.attackName = attackName;
    }
    public override void Added()
    {
        base.Added();

        Ability ability = AbilitySystem.Instantiate<Ability>(attackName);
        ability.Init(Target);
        AbilitySystem abilitySystem = Target.GetComponent<AbilitySystem>();
        this.ability = abilitySystem.GetAttackAbility();
        abilitySystem.SetAttackAbility(ability);
        ability.OnFired += () => Target.GetComponent<BattleSystem>().RemoveBuff(this);
    }

    public override void Removed()
    {
        base.Removed();

        Target.GetComponent<AbilitySystem>().SetAttackAbility(this.ability);
    }
}
