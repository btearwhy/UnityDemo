using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSet : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public float health;
    public float currentHealth;

    public float maxHealth;
    public float currentMaxHealth;


    public float attack;
    public float currentAttack;

    public float defense;
    public float currentDefense;

    public float maxSpeed;
    public float currentMaxSpeed;

    public delegate void LeathalHandler(GameObject instigator);
    public event LeathalHandler OnLeathal;

    public delegate void FloatAttributeHandler(float value);
    public event FloatAttributeHandler OnCurrentHealthChanged;
    public event FloatAttributeHandler OnCurrentMaxSpeedChanged;

    public delegate void KillHandler(int killerID, int victimID);
    public event KillHandler OnKilled;

    public delegate void DeathHandler();
    public event DeathHandler OnDied;


    private void Start()
    {
        currentHealth = health;
        currentAttack = attack;
        currentDefense = defense;
        currentMaxSpeed = maxSpeed;
        SetMaxSpeed(maxSpeed);
        SetCurrentHealth(maxHealth);
    }


    internal void DealDamage(GameObject instigator, float damage)
    {
        
        float prediectedCurrentHealth = currentHealth - damage;
        if(TryGetComponent<BattleSystem>(out BattleSystem battleSystem) && instigator.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            battleSystem.LastHitActorNr = photonView.ControllerActorNr;
        }
        if (prediectedCurrentHealth <= 0)
        {
            if (TryGetComponent<BattleSystem>(out BattleSystem battleSystemSelf) && TryGetComponent<PhotonView>(out PhotonView photonviewSelf))
            {
                if (battleSystemSelf.WasDamaged())
                {
                    OnKilled?.Invoke(battleSystemSelf.LastHitActorNr,  photonviewSelf.ControllerActorNr);
                }
            }
        }
        SetCurrentHealth(prediectedCurrentHealth);

    }

    private void SetCurrentHealth(float currentHealth)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetCurrentHealth_RPC", RpcTarget.All, currentHealth);
        }
    }

    [PunRPC]
    private void SetCurrentHealth_RPC(float currentHealth)
    {
        this.currentHealth = currentHealth;
        OnCurrentHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0) Die();
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetMaxSpeed_RPC", RpcTarget.All, maxSpeed);
        }
    }

    [PunRPC]
    private void SetMaxSpeed_RPC(float maxSpeed)
    {
        this.currentMaxSpeed = maxSpeed;
        OnCurrentMaxSpeedChanged?.Invoke(maxSpeed);
    }

    private void Die()
    {
        GetComponentInChildren<Animator>().Play("Death");
        OnDied?.Invoke();

        if (photonView.IsMine)
        {
            PlayerState.GetInstance().GetController().enabled = false;
            IEnumerator DestroyAfterSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
                PlayerState.GetInstance().Respawn();
                PhotonNetwork.Destroy(gameObject);
            }
            StartCoroutine(DestroyAfterSeconds(5));
        }
        
    }
    
    private void OnDestroy()
    {
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string characterName = (string)info.photonView.InstantiationData[0];
        Character character = (Character)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("characters", characterName);
        maxHealth = character.maxHealth;
        attack = character.attack;
        defense = character.defense;
        maxSpeed = character.maxSpeed;
    }
}
