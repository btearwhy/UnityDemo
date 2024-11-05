using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AbilitySystem : MonoBehaviourPunCallbacks
{
    public List<Ability> abilities;
    private Animator animator;
    private Movement movement;

    private List<int> abilitiesOnHeld;

    public AttributeSet attributeSet;

    // Start is called before the first frame update

    private void Awake()
    {
        abilities = new List<Ability>();
        abilitiesOnHeld = new List<int>();
    }

    internal void SetAttackAbility(Ability ability)
    {
        abilities[0] = ability;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        attributeSet = GetComponent<AttributeSet>();


    }
    
    // Update is called once per frame
    void Update()
    {
        abilitiesOnHeld.ForEach(v => {
            abilities[v].Held();
        });
    }
    
    public Ability_Attack GetAttackAbility()
    {
        return abilities[0] as Ability_Attack;
    }

    internal void ActionEnded(int abilityNo)
    {
        
    }


    public void GrantAbility(Ability ability)
    {
        photonView.RPC("GrantAbility_RPC", RpcTarget.All, ability);
    }

    [PunRPC]
    public void GrantAbility_RPC(Ability ability)
    {
        ability.Init(gameObject);
        abilities.Add(ability);
    }



    public void ActionPressed(int actionNo)
    {
        photonView.RPC("ActionPressed_RPC", RpcTarget.All, actionNo);
    }

    [PunRPC]
    public void ActionPressed_RPC(int actionNo)
    {
        abilities[actionNo].Pressed();
    }

    public void ActionHeld(int actionNo)
    {
        photonView.RPC("ActionHeld_RPC", RpcTarget.All, actionNo);
    }

    [PunRPC]
    public void ActionHeld_RPC(int actionNo)
    {
        abilitiesOnHeld.Add(actionNo);
    }

    public void ActionReleased(int actionNo)
    {
        photonView.RPC("ActionReleased_RPC", RpcTarget.All, actionNo);
    }

    [PunRPC]
    public void ActionReleased_RPC(int actionNo)
    {
        abilitiesOnHeld.Remove(actionNo);
        abilities[actionNo].Released();
    }

    public static T Instantiate<T>(string abilityDataName) where T:Ability
    {
        return (T)((Ability_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", abilityDataName)).CreateInstance();
    }
}
