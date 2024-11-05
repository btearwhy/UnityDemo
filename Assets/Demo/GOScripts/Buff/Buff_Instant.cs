using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff_Instant : Buff
{
    
    public int Times { get; set; }

    public List<Effect> EffectsOnEnemy { get; set; }

    public List<Effect> EffectsOnSelf { get; set; }


    public Buff_Instant() { }

    public Buff_Instant(int times, List<Effect> effectsOnEnemy, List<Effect> effectsOnSelf)
    {
        Times = times;
        EffectsOnEnemy = effectsOnEnemy;
        EffectsOnSelf = effectsOnSelf;
    }

    public Buff_Instant(int times, Effect effect, bool self) {
        effectOnSelf = self;
        Times = times;
        EffectsOnEnemy = new List<Effect>();
        EffectsOnSelf = new List<Effect>();
        if (self)
        {
            EffectsOnSelf.Add(effect);
        }
        else
        {
            EffectsOnEnemy.Add(effect);
        }

    }

    public override void Added()
    {
        base.Added();

        AbilitySystem abilitySystem = Target.GetComponent<AbilitySystem>();
        Ability_Attack ability = abilitySystem.GetAttackAbility();
        ability.OnEnded += () => Target.GetComponent<BattleSystem>().RemoveBuff(this);
    }

    public override void Removed()
    {
        base.Removed();
    }
}