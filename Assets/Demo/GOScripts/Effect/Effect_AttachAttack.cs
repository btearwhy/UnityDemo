using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_AttachAttack : Effect_Duration
{
    
    public Effect AttachedEffect { get; set; }

    public Effect_AttachAttack() { }
    public Effect_AttachAttack(string name):base(name){}

    public override void Initialize()
    {
        base.Initialize();

        Effect_Duration_AttackAttached_Data effect_data = (Effect_Duration_AttackAttached_Data)this.effect_data;
        Effect_Data attachedEffectData = (Effect_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("effects", effect_data.AttachedEffect.name);
        AttachedEffect = attachedEffectData.CreateInstance();
    }


}
