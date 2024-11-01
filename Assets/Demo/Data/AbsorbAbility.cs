using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbAbility : Ability
{
    public float detectRange;
    public float detectRadius;
    public float chargeSpeed;
    public int slotsNr;
    List<Element> elements;
    public static Dictionary<Element, string> element2BuffName;
    int curPos;
    float percentage;

    public override void Init(GameObject go)
    {
        base.Init(go);

        elements = new List<Element>(slotsNr);
        curPos = 0;
        percentage = 0.0f;
    }

    internal void Pressed(Animator animator, Movement movement)
    {

    }

    internal void Held(Animator animator, Movement movement)
    {
        if(Physics.SphereCast(character.transform.position, detectRadius, character.transform.forward, out RaycastHit rayCastHit, detectRange, ~(1 << character.layer))){
            GameObject hitObject = rayCastHit.collider.gameObject;
            if (curPos == slotsNr) return;
            if (hitObject.TryGetComponent<WorldProperty>(out WorldProperty worldProperty))
            {
                float chargeAmount = chargeSpeed * Time.deltaTime;
                if(worldProperty.element == elements[curPos])
                {
                    percentage += chargeAmount;
                    if(percentage >= 1.0f)
                    {
                        percentage -= 1.0f;
                        curPos++;
                        if(curPos == slotsNr)
                        {
                            for(int i = 0; i < slotsNr; i++)
                            {
                                character.GetComponent<BattleSystem>().AddBuff(Instantiate(AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("buffs", element2BuffName[elements[i]])) as Buff);
                            }

                            percentage = 0.0f;
                        }
                    }
                }
                else
                {
                    percentage = chargeAmount;
                }
            }
        }
        
        
    }

    internal void Released(Animator animator, Movement movement)
    {

    }   
}
