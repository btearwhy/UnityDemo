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


    [field: NonSerialized]
    private Ability_Absorb_Data ability_data;

    [field: NonSerialized]
    private Image slotImage;
    [field: NonSerialized]
    public float percentage;
    [field: NonSerialized]
    public Element? element;
    [field: NonSerialized]
    public int curBuffNum;

    public Ability_Absorb() { }

    public Ability_Absorb(string data) : base(data) { }


    public override void Initialize()
    {
        base.Initialize();
        Ability_Absorb_Data ability_absorb_data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
        this.detectRadius = ability_absorb_data.detectRadius;
        this.detectRange = ability_absorb_data.detectRange;
        this.chargeSpeed = ability_absorb_data.chargeSpeed;
        this.slotsNr = ability_absorb_data.slotsNr;
    }

    public override void Init(GameObject go)
    {
        base.Init(go);


        ability_data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
        
        progressPresent = GameObject.Instantiate(ability_data.slotBarCanvas);
        slotBackgroundImage = progressPresent.transform.GetChild(0).gameObject.GetComponent<Image>();
        //slotBackgroundImage = GameObject.Instantiate(ability_data.slotBackgroundImage, progressPresent.transform).GetComponent<Image>();

        progressPresent.SetActive(true);
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
                        var ele = AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("elements", worldProperty.element.ToString());
                        Buff_Data buff_data = ((Element_Data)ele).buff;
                        Buff buff = buff_data.CreateInstance();
                        Effect effect_Buff = new Effect(buff);
                        Buff_Instant buff_Instant = new Buff_Instant(1, effect_Buff, true);
                        buff.Init(character, character);
                        buff.OnRemoved += () => { curBuffNum--; };
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
                    element = worldProperty.element;
                    percentage = amount;
                    slotImage = GameObject.Instantiate(ability_data.dictSlotImages[element.Value], slotBackgroundImage.transform).GetComponent<Image>();
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
