using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Absorb_Data : Ability_Data
{
    public float detectRange;
    public float detectRadius;
    public float chargeSpeed;
    public int slotsNr;

    public GameObject slotBackgroundImage;
    public GameObject slotBarCanvas;

    public List<Element> elements;
    /*public List<Buff>*/
    public List<GameObject> slotImages;
    public Dictionary<Element, GameObject> dictSlotImages;

    private void OnEnable()
    {
        dictSlotImages = new Dictionary<Element, GameObject>();
        for(int i = 0; i < elements.Count; i++)
        {
            dictSlotImages.Add(elements[i], slotImages[i]);
        }    
    }

    public override Ability CreateInstance()
    {
        return new Ability_Absorb(name);   
    }
}
