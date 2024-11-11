using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Duration_AttributePercentageChange_Data : Effect_Duration_Data
{
    public float percentage;
    public AttributeType attrbiuteType;

    public override Effect CreateInstance()
    {
        return new Effect_Duration_AttributeChangePercentage(name);
    }
}
