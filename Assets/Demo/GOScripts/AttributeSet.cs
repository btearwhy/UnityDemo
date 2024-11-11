using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum AttributeType
{
    Health,
    MaxHealth,
    Attack,
    Defense,
    MaxSpeed
}

[Serializable]
public class Attribute
{
    public AttributeType Type { get; set; }
    public float BaseValue { get; set; }
    public float CurrentValue { get; set; }

    public Attribute(AttributeType type, float value)
    {
        Type = type;
        BaseValue = value;
        CurrentValue = value;
    }

    public Attribute(AttributeType type)
    {
        Type = type;
    }

    internal void Init(float value)
    {
        BaseValue = value;
        CurrentValue = value;
    }
}

public class AttributeSet : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public delegate void FloatAttributeHandler(float value);

    public Dictionary<AttributeType, Attribute> Attributes = new Dictionary<AttributeType, Attribute>();

    public EventHandlerList<AttributeType> events = new EventHandlerList<AttributeType>();

    public Attribute Health;
    public event FloatAttributeHandler OnHealthChanged { add { events.AddHandler(AttributeType.Health, value); } remove { events.RemoveHandler(AttributeType.Health, value); } }

    public Attribute MaxHealth;
    public event FloatAttributeHandler OnMaxHealthChanged { add { events.AddHandler(AttributeType.MaxHealth, value); } remove { events.RemoveHandler(AttributeType.MaxHealth, value); } }

    public Attribute Attack;
    public event FloatAttributeHandler OnAttackChanged { add { events.AddHandler(AttributeType.Attack, value); } remove { events.RemoveHandler(AttributeType.Attack, value); } }

    public Attribute Defense;
    public event FloatAttributeHandler OnDefenseChanged { add { events.AddHandler(AttributeType.Defense, value); } remove { events.RemoveHandler(AttributeType.Defense, value); } }

    public Attribute MaxSpeed;
    public event FloatAttributeHandler OnMaxSpeedChanged { add { events.AddHandler(AttributeType.MaxSpeed, value); } remove { events.RemoveHandler(AttributeType.MaxSpeed, value); } }


    public delegate void LeathalHandler(GameObject instigator);
    public event LeathalHandler OnLeathal;






    private void Awake()
    {

        Health = new Attribute(AttributeType.Health);
        Attributes.Add(AttributeType.Health, Health);

        MaxHealth = new Attribute(AttributeType.MaxHealth);
        Attributes.Add(AttributeType.MaxHealth, MaxHealth);

        Attack = new Attribute(AttributeType.Attack);
        Attributes.Add(AttributeType.Attack, Attack);

        Defense = new Attribute(AttributeType.Defense);
        Attributes.Add(AttributeType.Defense, Defense);

        MaxSpeed = new Attribute(AttributeType.MaxSpeed);
        Attributes.Add(AttributeType.MaxSpeed, MaxSpeed);
    }

    private void Start()
    {
       
    }



    
    public FloatAttributeHandler GetAttributeEvent(AttributeType attributeType)
    {
        return (FloatAttributeHandler)events[attributeType];
    }

    public void SetAttribute(AttributeType attributeType, float value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetAttribute_RPC", RpcTarget.All, attributeType, value);
        }
        
    }

    [PunRPC]
    private void SetAttribute_RPC(AttributeType attributeType, float value)
    {
        Attributes[attributeType].CurrentValue = value;
        FloatAttributeHandler deleg = (FloatAttributeHandler)events[attributeType];
        deleg?.Invoke(value);
    }
    

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string characterName = (string)info.photonView.InstantiationData[0];
        Character character = (Character)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("characters", characterName);
        foreach(AttributeData attributeData in character.initialAttributes)
        {
            Attributes[attributeData.type].Init(attributeData.value);
        }
    }

    internal float GetCurrentValue(AttributeType attributeType)
    {
        return Attributes[attributeType].CurrentValue;
    }
}
