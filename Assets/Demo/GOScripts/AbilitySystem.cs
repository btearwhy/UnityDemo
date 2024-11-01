using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AbilitySystem : MonoBehaviourPunCallbacks
{
    public List<Ability> actions;
    private Animator animator;
    private Movement movement;
    List<int> abilitiesOnHeld;

    AttributeSet attributeSet;

    // Start is called before the first frame update

    private void Awake()
    {
        actions = new List<Ability>();
        abilitiesOnHeld = new List<int>();
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
        abilitiesOnHeld.ForEach(v => { actions[v].Held(); });
    }

    internal void ActionEnded(int abilityNo)
    {
        
    }


    public void GrantAbility(string abilityName)
    {
        photonView.RPC("GrantAbility_RPC", RpcTarget.All, abilityName);
    }

    [PunRPC]
    public void GrantAbility_RPC(string abilityName)
    {
        Ability abi = Instantiate((Ability)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", abilityName));
        abi.Init(gameObject);
        actions.Add(abi);
    }



    public void ActionPressed(int actionNo)
    {
        photonView.RPC("ActionPressed_RPC", RpcTarget.All, actionNo);
    }

    [PunRPC]
    public void ActionPressed_RPC(int actionNo)
    {
        actions[actionNo].Pressed();
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
        actions[actionNo].Released();
    }

}
