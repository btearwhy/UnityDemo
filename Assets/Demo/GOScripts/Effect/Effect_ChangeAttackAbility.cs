using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_ChangeAttackAbility : Effect_Duration
{
    public string attackName;

    private Ability ability;
    public Effect_ChangeAttackAbility()
    {
    }

    public Effect_ChangeAttackAbility(string name):base(name)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        Effect_Duration_AttackAbility_Data effect_data = (Effect_Duration_AttackAbility_Data)this.effect_data;
        
        attackName = effect_data.Ability_Attack_Data.name;
    }
    public override void Added()
    {
        base.Added();

        Ability ability = AbilitySystem.Instantiate<Ability>(attackName);
        ability.Init(Context.Target);
        AbilitySystem abilitySystem = Context.Target.GetComponent<AbilitySystem>();
        this.ability = abilitySystem.GetAttackAbility();
        abilitySystem.SetAttackAbility(ability);
        ability.OnFired += () => Context.Target.GetComponent<BattleSystem>().RemoveEffect(this);
    }

    public override void Removed()
    {
        base.Removed();

        Context.Target.GetComponent<AbilitySystem>().SetAttackAbility(this.ability);
    }


}
