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
    private List<Image> slotBackgroundImages;

    [field: NonSerialized]
    private GameObject backgroundImage;


    [field:NonSerialized]
    private Ability_Absorb_Data data;

    private struct Slot{
        public Image slotImage;
        public bool charged;
        public Element element;

        public Slot(Element element, Image slotImage, bool charged)
        {
            this.element = element;
            this.slotImage = slotImage;
            this.charged = charged;
        }
    }

    [field:NonSerialized]
    private Stack<Slot> stSlot;

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
        slotBackgroundImages = new List<Image>();
        stSlot = new Stack<Slot>();

        data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", "Absorb_Ability_Data");
        
        progressPresent = GameObject.Instantiate(data.slotBarCanvas);
        backgroundImage = data.slotBackgroundImage;
        for (int i = 0; i < slotsNr; i++)
        {
            slotBackgroundImages.Add(GameObject.Instantiate(backgroundImage, progressPresent.transform).GetComponent<Image>());
        }
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




            if (stSlot.Count == slotsNr && stSlot.Peek().charged) return;

            if (hitObject.TryGetComponent<WorldProperty>(out WorldProperty worldProperty))
            {
                float amount = Time.deltaTime * chargeSpeed;
                if (stSlot.TryPop(out Slot current))
                {
                    if(current.element != worldProperty.element && !current.charged)
                    {
                        GameObject.Destroy(current.slotImage);
                        current.slotImage = GameObject.Instantiate(data.dictSlotImages[worldProperty.element], slotBackgroundImages[stSlot.Count].transform).GetComponent<Image>();
                        current.slotImage.fillAmount = amount;
                    }
                    else
                    {
                        if(current.slotImage.fillAmount + amount >= 1.0f)
                        {
                            current.slotImage.fillAmount = 1.0f;
                            current.charged = true;

                            Buff buff = ((Element_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("elements", worldProperty.element.ToString())).buff.CreateInstance();
                            Effect_Buff effect_Buff = new Effect_Buff(buff);
                            Buff_Instant buff_Instant = new Buff_Instant(1, effect_Buff, true);
                            buff.Instigator = character;
                            character.GetComponent<BattleSystem>().AddBuff(buff);
                        }
                        else
                        {
                            current.slotImage.fillAmount += amount;
                        }
                    }
                    current.element = worldProperty.element;
                }
                else
                {
                    stSlot.Push(new Slot(amount, worldProperty.element, GameObject.Instantiate(data.dictSlotImages[worldProperty.element], slotBackgroundImages[stSlot.Count].transform).GetComponent<Image>())));
                    stSlot.Peek().slotImage.fillAmount = stSlot.Peek().percentage;
                }
                
            }


            
        }


    }
+

    internal override void Released()
    {
        //base.Released();
    }

    public void Consume()
    {
        if(stSlot.TryPop(out Slot peek))
        {
            if(peek.percentage)
        }

    }
}
