using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff_Instant_Data : Buff_Data
{
    public readonly int times;

    public readonly List<Effect_Data> effectsOnEnemy;
    public readonly List<Effect_Data> effectsOnSelf;
}

[System.Serializable]
public class Buff_Instant : Buff
{
    public List<Effect_Data> EffectsOnEnemy { get; set; }
    
    public List<Effect_Data> EffectsOnSelf { get; set; }
}