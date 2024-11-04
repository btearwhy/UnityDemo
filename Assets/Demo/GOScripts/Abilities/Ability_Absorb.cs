using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ability_Absorb : Ability
{
    public float detectRange;
    public float detectRadius;
    public float chargeSpeed;
    public int slotsNr;

    [field: NonSerialized]
    private Dictionary<Element, Buff> element2BuffName;


    /*    [field: NonSerialized]
        private Element?[] elements;*/

    /*    [field: NonSerialized]
        private int curPos;*/


    [field: NonSerialized]
    private GameObject progressPresent;

    [field: NonSerialized]
    private Image slotBackgroundImage;


    [field:NonSerialized]
    private Ability_Absorb_Data data;

    [field: NonSerialized]
    private Image slotImage;
    [field: NonSerialized]
    public float percentage;
    [field: NonSerialized]
    public Element? element;
    [field: NonSerialized]
    public int curBuffNum;

    public Ability_Absorb() { }
    public Ability_Absorb(string animStartStateName, string animHeldStateName, string animReleaseStateName, float detectRange, float detectRadius, float chargeSpeed, int slotsNr) : base(animStartStateName, animHeldStateName, animReleaseStateName)
    {
        this.detectRadius = detectRadius;
        this.detectRange = detectRange;
        this.chargeSpeed = chargeSpeed;
        this.slotsNr = slotsNr;


    }
    public override void Init(GameObject go)
    {
        base.Init(go);


        data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", "Absorb_Ability_Data");
        
        progressPresent = GameObject.Instantiate(data.slotBarCanvas);

        slotBackgroundImage = GameObject.Instantiate(data.slotBackgroundImage, progressPresent.transform).GetComponent<Image>();

        percentage = 0.0f;
        element = null;
        slotImage = null;
        curBuffNum = 0;
    }

    internal override void Pressed()
    {
        //base.Pressed();
    }

    internal override void Held()
    {
        //base.Held();
        
        if (Physics.SphereCast(character.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, detectRadius, character.transform.forward, out RaycastHit rayCastHit, detectRange/*, ~(1 << character.layer)*/))
        {
            GameObject hitObject = rayCastHit.collider.gameObject;

            

            if (curBuffNum == slotsNr)
            {
                return;
            }

            if (hitObject.TryGetComponent<WorldProperty>(out WorldProperty worldProperty))
            {
                float amount = Time.deltaTime * chargeSpeed;
                if(element == worldProperty.element)
                {
                    percentage += amount;
                    if(percentage >= 1.0f)
                    {
                        Buff buff = ((Element_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("elements", worldProperty.element.ToString())).buff.CreateInstance();
                        Effect_Buff effect_Buff = new Effect_Buff(buff);
                        Buff_Instant buff_Instant = new Buff_Instant(1, effect_Buff, true);
                        buff.Instigator = character;
                        character.GetComponent<BattleSystem>().AddBuff(buff);
                        
                        percentage -= 1.0f;
                        curBuffNum++;
                    }
                    slotImage.fillAmount = percentage;
                }
                else
                {
                    if(slotImage != null)
                    {
                        GameObject.Destroy(slotImage);
                    }

                    percentage = amount;
                    slotImage = GameObject.Instantiate(data.dictSlotImages[element.Value], slotBackgroundImage.transform).GetComponent<Image>();
                    slotImage.fillAmount = percentage;
                }
            }


            
        }


    }

    internal override void Released()
    {
        //base.Released();
    }
}
