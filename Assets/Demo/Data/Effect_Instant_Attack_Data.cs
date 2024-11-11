using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Instant_Attack_Data : Effect_Instant_Data
{

    public override Effect CreateInstance()
    {
        return new Effect_Attack(name);
    }
}
