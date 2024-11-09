using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    List<Buff> carriedBuffs;
    List<Color> _emisionColors = new List<Color>();
    SkinnedMeshRenderer[] meshRenderers;

    public int LastHitActorNr { get; internal set; }

    
    private void Awake()
    {
        LastHitActorNr = -1;
        carriedBuffs = new List<Buff>();
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
    }

    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<AttributeSet>(out AttributeSet attributeSet))
        {
            attributeSet.OnDied += () =>
            {
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
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuff(Buff buff)
    {
        Debug.Log(gameObject + " acqurie buff " + buff);
        carriedBuffs.Add(buff);
        buff.Added();
    }

    public void RemoveBuff(Buff buff)
    {
        Debug.Log(gameObject + " remove buff " + buff);
        carriedBuffs.Remove(buff);
        buff.Removed();
    }

    public List<Buff_Instant> GetAttackAttachedBuff()
    {
        List<Buff_Instant> instantBuffs = new List<Buff_Instant>();
        foreach(Buff buff in carriedBuffs)
        {
            if(buff is Buff_Instant)
            {
                instantBuffs.Add(buff as Buff_Instant);
            }
        }
        return instantBuffs;
    }


    public void HitFlash(Color color, float interval = 0.2f)
    {
        StartCoroutine(HitFlashCoroutine(color, interval));
    } 

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

    internal bool WasDamaged()
    {
        int la = LastHitActorNr;
        int actor = GetComponent<PhotonView>().ControllerActorNr;
        return LastHitActorNr != -1 && LastHitActorNr != GetComponent<PhotonView>().ControllerActorNr;
    }

    public static void AttachBuffToEffect(ref Effect effect, Buff_Instant buff)
    {
        effect.buffs.Add(buff);
        buff.OnRemoved.Invoke();
    }

    internal Buff_Instant GetOneAttackAttachedBuff()
    {
        foreach(Buff buff in carriedBuffs)
        {
            if(buff is Buff_Instant)
            {
                return buff as Buff_Instant;
            }
        }
        return null;
    }
}
