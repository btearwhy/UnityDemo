using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviourPunCallbacks
{
    public delegate void KillHandler(int killerID, int victimID);
    public event KillHandler OnKilled;


    EffectContainer ActiveEffectContainer { get; set; }
    List<Color> _emisionColors = new List<Color>();
    SkinnedMeshRenderer[] meshRenderers;

    public int LastHitActorNr { get; internal set; }

    
    private void Awake()
    {
        ActiveEffectContainer = new EffectContainer();
        LastHitActorNr = -1;
        _emisionColors = new List<Color>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer meshRender in meshRenderers)
        {
            foreach (Material material in meshRender.materials)
            {
                material.EnableKeyword("_EmissionColor");
                _emisionColors.Add(material.GetColor("_EmissionColor"));
            }
        }

        GetComponent<AttributeSet>().OnHealthChanged += (value) =>
        {
            if (value <= 0.0f)
            {
                Die();
            }
        };
    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddEffect(Effect effect)
    {
        Debug.Log(gameObject + " acqurie buff " + effect);
        ActiveEffectContainer.Add(effect);
        effect.Added();
    }

    public void RemoveEffect(Effect effect)
    {
        Debug.Log(gameObject + " remove buff " + effect);
        ActiveEffectContainer.Remove(effect);
        effect.Removed();
    }



    public void HitFlash(Color color, float interval = 0.2f)
    {
        IEnumerator HitFlashCoroutine(Color color, float interval)
        {
            foreach (SkinnedMeshRenderer meshRender in meshRenderers)
            {
                foreach (Material material in meshRender.materials)
                {
                    material.SetColor("_EmissionColor", color);
                }
            }

            yield return new WaitForSeconds(interval);

            int index = 0;
            foreach (SkinnedMeshRenderer meshRender in meshRenderers)
            {
                foreach (Material material in meshRender.materials)
                {
                    material.SetColor("_EmissionColor", _emisionColors[index]);
                    index++;
                }
            }
        }

        StartCoroutine(HitFlashCoroutine(color, interval));
    }

    internal List<Effect_AttachAttack> GetAttackAttachedEffects()
    {
        List<Effect_AttachAttack> effects = new List<Effect_AttachAttack>();
        foreach(Effect effect in ActiveEffectContainer)
        {
            if(effect.GetType().IsAssignableFrom(typeof(Effect_AttachAttack)))
            {
                effects.Add((Effect_AttachAttack)effect);
            }
        }
        return effects;
    }

    internal bool WasDamaged()
    {
        int la = LastHitActorNr;
        int actor = GetComponent<PhotonView>().ControllerActorNr;
        return LastHitActorNr != -1 && LastHitActorNr != GetComponent<PhotonView>().ControllerActorNr;
    }


    internal void ApplyEffect(Effect effect, GameObject instigator)
    {
        effect.SetContext(instigator, gameObject);
        if (typeof(Effect_Instant).IsAssignableFrom(effect.GetType()))
        {
            effect.Apply();
        }
        else if (typeof(Effect_Duration).IsAssignableFrom(effect.GetType())){
            AddEffect(effect);
        }
    }

    internal void DealDamage(GameObject instigator, float damage)
    {
        AttributeSet attributeSet = GetComponent<AttributeSet>();
        if (!isActiveAndEnabled) return;
        float prediectedCurrentHealth = attributeSet.GetCurrentValue(AttributeType.Health) - damage;
        if (TryGetComponent<BattleSystem>(out BattleSystem battleSystem) && instigator.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            battleSystem.LastHitActorNr = photonView.ControllerActorNr;
        }
        attributeSet.SetAttribute(AttributeType.Health, prediectedCurrentHealth);

    }

    private void Die()
    {
        enabled = false;
        GetComponentInChildren<Animator>().Play("Death");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponentInChildren<Animator>().applyRootMotion = true;
        if (TryGetComponent<PhotonView>(out PhotonView photonviewSelf))
        {
            if (WasDamaged())
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameRoom.gameRoom.AddScore(LastHitActorNr, photonviewSelf.ControllerActorNr);
                }
            }
        }


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
}
