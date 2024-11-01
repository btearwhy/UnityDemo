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
    public float defense;
    public float currentAttack;
    public float currentDefense;

    public delegate void LeathalHandler(GameObject instigator);
    public event LeathalHandler OnLeathal;

    public delegate void CurrentHealthHandler(float health);
    public event CurrentHealthHandler OnCurrentHealthChanged;

    public delegate void KillHandler(int killerID, int victimID);
    public event KillHandler OnKilled;

    public delegate void DeathHandler();
    public event DeathHandler OnDied;


    private void Start()
    {
        health = maxHealth;
        currentHealth = health;
        currentAttack = attack;
        currentDefense = defense;
    }

    internal void DealDamage(int actorNr, float damage)
    {

        float prediectedCurrentHealth = currentHealth - damage;
/*        if (prediectedCurrentHealth <= 0)
        {
            OnLeathal?.Invoke(gameObject);
        }*/
        if (prediectedCurrentHealth <= 0)
        {
            if(TryGetComponent<PhotonView>(out PhotonView photonview))
            {
                OnKilled?.Invoke(actorNr, photonview.ControllerActorNr);
            }
        }
        SetCurrentHealth(prediectedCurrentHealth);
    }

    internal void DealDamage(GameObject instigator, float damage)
    {
        
        float prediectedCurrentHealth = currentHealth - damage;
        if (prediectedCurrentHealth <= 0)
        {
            OnLeathal?.Invoke(gameObject);
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



    private void Die()
    {
        GetComponentInChildren<Animator>().Play("Death");
        OnDied?.Invoke();

        PlayerState.GetInstance().GetController().enabled = false;
        if (photonView.IsMine)
        {
            IEnumerator DestroyAfterSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
                PhotonNetwork.Destroy(gameObject);
            }
            StartCoroutine(DestroyAfterSeconds(5));
        }
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string characterName = (string)info.photonView.InstantiationData[0];
        Character character = (Character)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("characters", characterName);
        maxHealth = character.maxHealth;
        attack = character.attack;
        defense = character.defense;
    }
}
