using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff_Decelerate : Buff_Duration
{
    public float Percentage { get; set; }
    private float delta = 0.0f;
    public Buff_Decelerate() { }

    public Buff_Decelerate(float duration, float percentage) : base(duration)
    {
        Percentage = percentage;
    }

   /* public override void Added()
    {
        base.Added();

        if (Target.TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            delta = attributeSet.maxSpeed * Percentage;
            attributeSet.SetMaxSpeed(attributeSet.currentMaxSpeed - delta);
        }
    }

    public override void Removed()
    {
        base.Removed();

        if (Target.TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            attributeSet.SetMaxSpeed(attributeSet.currentMaxSpeed + delta);
        }
    }*/
}
