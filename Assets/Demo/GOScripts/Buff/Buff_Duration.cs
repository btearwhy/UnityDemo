using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff_Duration : Buff
{
    public float Duration { get; set; }

    public Buff_Duration() { }
    public Buff_Duration(float duration)
    {
        Duration = duration;
    }

    public override void Added()
    {
        base.Added();
        BattleSystem battleSystem = Target.GetComponent<BattleSystem>();
        battleSystem.StartCoroutine(TimeElapse(Duration));

    }

    IEnumerator TimeElapse(float time)
    {
        yield return new WaitForSeconds(time);
        BattleSystem battleSystem = Target.GetComponent<BattleSystem>();
        //battleSystem.RemoveEffect(this);
    }
}