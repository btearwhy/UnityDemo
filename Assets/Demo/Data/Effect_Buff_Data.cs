using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effect_Buff_Data : Effect_Data
{
    public readonly List<Buff_Data> buffs;
}


[System.Serializable]
public class Effect_Buff : Effect
{
    public List<Buff> Buffs { get; set; }


    public override void Apply(GameObject instigator, GameObject target)
    {
        foreach (Buff buff in Buffs)
        {
            buff.Instigator = instigator;
            target.GetComponent<BattleSystem>().AddBuff(buff);
        }
    }
}