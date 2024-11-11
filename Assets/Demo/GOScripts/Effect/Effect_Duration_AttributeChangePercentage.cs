using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect_Duration_AttributeChangePercentage :Effect_Duration
{
    private float delta;

    public float Percentage { get; set; }
    public AttributeType TargetAttribute { get; set; }
    public Effect_Duration_AttributeChangePercentage() { }

    public Effect_Duration_AttributeChangePercentage(string name) : base(name){
    }

    public override void Initialize()
    {
        base.Initialize();
        Effect_Duration_AttributePercentageChange_Data effect_data = (Effect_Duration_AttributePercentageChange_Data)this.effect_data;
        Percentage = effect_data.percentage;
        TargetAttribute = effect_data.attrbiuteType;
    }
    public override void Added()
    {
        base.Added();

        if (Context.Target.TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            delta = attributeSet.GetCurrentValue(TargetAttribute) * Percentage;
            attributeSet.SetAttribute(TargetAttribute, attributeSet.GetCurrentValue(TargetAttribute) - delta);
        }
    }

    public override void Removed()
    {
        base.Removed();

        if (Context.Target.TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            attributeSet.SetAttribute(TargetAttribute, attributeSet.GetCurrentValue(TargetAttribute) + delta);
        }
    }
}
