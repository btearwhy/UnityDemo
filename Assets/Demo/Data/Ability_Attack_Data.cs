using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[System.Serializable]
public class Ability_Attack_Data : Ability_Data
{


    public string fireSocket;
    public AudioSource fireAudio;

    public GameObject projectilePrefab;
    public bool projectileGravity;
    public float projectileSpeed;

    public float initialAngle;
    public float minAttackRange;
    public float maxAttackRange;
    public float attackRangeChargeSpeed;
    public Effect_Data effect_Data;
    
    public GameObject lineRenderer;

    public override Ability CreateInstance()
    {
        return new Ability_Attack(name);
    }
}
