using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class Effect_Damage_Data : Effect_Data
{

    public override Effect CreateInstance()
    {
        return new Effect_Damage();
    }

}



