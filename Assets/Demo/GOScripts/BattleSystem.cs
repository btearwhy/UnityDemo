using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    List<Buff> carriedBuffs;

    private void Awake()
    {
        carriedBuffs = new List<Buff>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuff(Buff buff)
    {
        carriedBuffs.Add(buff);
        buff.Init(gameObject);
        buff.OnAdded();
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
        StartCoroutine(HitFlashCoroutine(gameObject, color, interval));
    } 

    IEnumerator HitFlashCoroutine(GameObject target, Color color, float interval)
    {
        SkinnedMeshRenderer[] meshRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Color> colors = new List<Color>();
        foreach (SkinnedMeshRenderer meshRender in meshRenderers)
        {
            foreach (Material material in meshRender.materials)
            {
                material.EnableKeyword("_EmissionColor");
                colors.Add(material.GetColor("_EmissionColor"));
                material.SetColor("_EmissionColor", color);
            }
        }

        yield return new WaitForSeconds(interval);

        int index = 0;
        foreach (SkinnedMeshRenderer meshRender in meshRenderers)
        {
            foreach (Material material in meshRender.materials)
            {
                material.SetColor("_EmissionColor", colors[index]);
                index++;
            }
        }
    }

    internal Buff GetOneAttackAttachedBuff()
    {
        foreach(Buff buff in carriedBuffs)
        {
            if(buff is Buff_Instant)
            {
                return buff;
            }
        }
        return null;
    }
}
