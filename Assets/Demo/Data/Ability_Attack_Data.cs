using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using VolumetricLines;
using System;

[System.Serializable]
public class Ability_Attack_Data : Ability_Data
{


    public string fireSocket;
    public AudioSource fireAudio;

    public GameObject projectilePrefab;

    public float initialAngle;
    public float minAttackRange;
    public float maxAttackRange;
    public float attackRangeChargeSpeed;
    public List<Effect_Data> effects;

    public GameObject lineRenderer;

    public override Ability CreateInstance()
    {
        List<Effect> effects = new List<Effect>();
        this.effects.ForEach((effect_data)=>effects.Add(effect_data.CreateInstance()));
        return new Ability_Attack(animStartStateName, animHeldStateName, animReleaseStateName, fireSocket, projectilePrefab.name, initialAngle, minAttackRange, maxAttackRange, attackRangeChargeSpeed, effects, lineRenderer);
    }
}
