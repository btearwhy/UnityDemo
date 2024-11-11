using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_Duration_Dot : Effect_Duration
{
    public float damage;
    public float interval;
    private bool over = false;
    public Effect_Duration_Dot() { }

    public Effect_Duration_Dot(string name) : base(name){ 
        
    }

    public override void Initialize()
    {
        base.Initialize();
        Effect_Duration_Dot_Data effect_data = (Effect_Duration_Dot_Data)this.effect_data;
        damage = effect_data.interval;
        interval = effect_data.interval;

    }

    public override void Added()
    {
        base.Added();

        IEnumerator Dot(float inter)
        {
            while (!over)
            {
                Apply();
                yield return new WaitForSeconds(inter);
            }
        }

        Context.Instigator.GetComponent<BattleSystem>().StartCoroutine(Dot(interval));
        
    }

    public override void Apply()
    {
        base.Apply();

        if (Context.Target.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
        {
            battleSystem.DealDamage(Context.Instigator, damage);
        }
        
    }

    public override void Removed()
    {
        base.Removed();

        over = true;
    }
}
