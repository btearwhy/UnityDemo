using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Attack_Data : Effect_Data
{
    public override Effect CreateInstance()
    {
        return new Effect_Attack();
    }
}
