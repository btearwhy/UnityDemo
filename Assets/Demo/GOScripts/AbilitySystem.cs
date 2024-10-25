using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AbilitySystem : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    
    public List<Ability> actions;
    private Animator animator;
    private Movement movement;
    List<int> abilitiesOnHeld;
    // Start is called before the first frame update
    public AbilitySystem()
    {
        actions = new List<Ability>();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        abilitiesOnHeld = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        abilitiesOnHeld.ForEach(v => { actions[v].OnHeld(); });
    }

    internal void ActionEnded(int abilityNo)
    {
        
    }

    [PunRPC]
    public void StartActionRPC(int actionNo)
    {
/*        if (actionNo < actions.Count)
        {
            
            actions[actionNo].StartAction(animator);
        }*/
    }

    public void GrantAbility(Ability ability)
    {
        ability.Init(gameObject);
        actions.Add(ability);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("info" + info);
        foreach (string abilityName in info.photonView.InstantiationData)
        {
            Ability ability = (Ability)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", abilityName);
            GrantAbility(ability);
        }
        // ...
    }

    public void ActionPressed(int actionNo)
    {
        actions[actionNo].Pressed();
    }

    public void ActionHeld(int actionNo)
    {
        abilitiesOnHeld.Add(actionNo);
        actions[actionNo].Held();
    }

    public void ActionReleased(int actionNo)
    {
        abilitiesOnHeld.Remove(actionNo);
        actions[actionNo].Released();
    }

}
