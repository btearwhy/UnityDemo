using Photon.Pun;
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


    /*    [field: NonSerialized]
        private Element?[] elements;*/

    /*    [field: NonSerialized]
        private int curPos;*/


    [field: NonSerialized]
    private GameObject progressPresent;

    [field: NonSerialized]
    private List<Image> slotBackgroundImages;


    [field: NonSerialized]
    private Ability_Absorb_Data ability_data;

    [field: NonSerialized]
    private Image slotImage;
    [field: NonSerialized]
    public float percentage;
    [field: NonSerialized]
    public List<Element?> elements;
    [field: NonSerialized]
    public int curBuffNum;
    [field: NonSerialized]
    public int curPos;

    public Ability_Absorb() { }

    public Ability_Absorb(string data) : base(data) { }


    public override void Initialize()
    {
        Ability_Absorb_Data ability_absorb_data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
        this.animStartStateName = ability_absorb_data.animStartStateName;
        this.animHeldStateName = ability_absorb_data.animHeldStateName;
        this.animReleaseStateName = ability_absorb_data.animReleaseStateName;
        this.detectRadius = ability_absorb_data.detectRadius;
        this.detectRange = ability_absorb_data.detectRange;
        this.chargeSpeed = ability_absorb_data.chargeSpeed;
        this.slotsNr = ability_absorb_data.slotsNr;
    }

    public override void Init(GameObject go)
    {
        base.Init(go);


        ability_data = (Ability_Absorb_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);


        progressPresent = GameObject.Instantiate(ability_data.slotBarCanvas, PlayerState.GetInstance().HUD.SkillUIPlaceHolder.transform);
        if (!character.GetComponent<PhotonView>().IsMine)
        {
            progressPresent.SetActive(false);
        }
        else progressPresent.SetActive(true);
        slotBackgroundImages = new List<Image>();
        elements = new List<Element?>();
        for (int i = 0; i < slotsNr; i++)
        {
            slotBackgroundImages.Add(GameObject.Instantiate(ability_data.slotBackgroundImage, progressPresent.transform).GetComponent<Image>());
            elements.Add(null);
        }
        //slotBackgroundImage = GameObject.Instantiate(ability_data.slotBackgroundImage, progressPresent.transform).GetComponent<Image>();


        percentage = 0.0f;

        slotImage = null;
        curBuffNum = 0;
        curPos = 0;
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
                if(elements[curPos] == worldProperty.element)
                {
                    percentage += amount;
                    if(percentage >= 1.0f)
                    {
                        var ele = AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("elements", worldProperty.element.ToString());
                        Effect effect = ((Element_Data)ele).effect.CreateInstance();


                        Image curImage = slotImage;
                        int tmp = curPos;
                        effect.OnRemoved += () => {
                            if(curBuffNum == slotsNr)
                            {
                                curPos = tmp;
                            }
                            GameObject.Destroy(curImage);
                            elements[tmp] = null;
                            curBuffNum--;
                        };

                        BattleSystem battle = character.GetComponent<BattleSystem>();
                        battle.ApplyEffect(effect, character);

                        slotImage.fillAmount = 1.0f;
                        percentage -= 1.0f;
                        
                        curBuffNum++;
                        slotImage = null;
                        if(curBuffNum != slotsNr)
                        {
                            while (elements[curPos] != null)
                            {
                                curPos = (curPos + 1) % slotsNr;
                            }
                        }
                    }
                    else
                    {
                        slotImage.fillAmount = percentage;
                    }
                }
                else
                {
                    if(slotImage != null)
                    {
                        GameObject.Destroy(slotImage);
                    }
                    elements[curPos] = worldProperty.element;
                    percentage = amount;
                    slotImage = GameObject.Instantiate(ability_data.dictSlotImages[worldProperty.element], slotBackgroundImages[curPos].transform).GetComponent<Image>();
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
