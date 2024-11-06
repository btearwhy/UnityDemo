using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public string data;
    public List<Buff> buffs;
    public Effect() { }

    public Effect(string data) {
        this.data = data;
        Initialize();
    }

    public virtual void Initialize()
    {
        buffs = new List<Buff>();
        Effect_Data effect_Data = (Effect_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("effects", data);
        effect_Data.buffs_data.ForEach((buff_data) => buffs.Add(buff_data.CreateInstance()));
    }

    public Effect(Buff buff)
    {
        this.buffs = new List<Buff>();
        this.buffs.Add(buff);
    }

    public virtual void Apply(GameObject instigator, GameObject target)
    {
        if(instigator.TryGetComponent<BattleSystem>(out BattleSystem battleSystemInstigator))
        {
            foreach (Buff buff in buffs)
            {
                if (buff.effectOnSelf)
                {
                    buff.Init(instigator, instigator);
                    battleSystemInstigator.GetComponent<BattleSystem>().AddBuff(buff);
                }
            }
        }
        if (target.TryGetComponent<BattleSystem>(out BattleSystem battleSystemTarget))
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.effectOnSelf)
                {
                    buff.Init(instigator, target);
                    battleSystemTarget.AddBuff(buff);
                }
            }
        }
    }
}