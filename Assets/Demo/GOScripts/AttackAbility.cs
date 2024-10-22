using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackAbility : Ability
{
    public string triggerName = "attack";
    public bool fired;
    public AudioSource fireAudio;

    public string projectilePrefabName = "cannon_attack";
    public override void StartAction()
    {
        base.StartAction();
        
    }

    public override void Fire(Transform transform, Transform trans_projectileSpawnSocket)
    {
        base.Fire(transform, trans_projectileSpawnSocket);

        PhotonNetwork.Instantiate(projectilePrefabName, trans_projectileSpawnSocket.position, transform.rotation);

    }
}
