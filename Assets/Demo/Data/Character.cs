using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttributeData
{
    public AttributeType type;
    public float value;
}
public class Character : ScriptableObject
{
    // Start is called before the first frame update
    public string characterName;
    public Sprite avator;
    public GameObject modelPrefab;

    public List<AttributeData> initialAttributes;
    public List<Ability_Data> abilities;
    
}
