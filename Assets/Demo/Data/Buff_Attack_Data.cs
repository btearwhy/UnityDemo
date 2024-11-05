using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack_Data : Buff_Data
{
    public string attackName;
    // Start is called before the first frame update
    public override Buff CreateInstance()
    {
        return new Buff_Attack(attackName);
    }
}
