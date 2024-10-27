using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    // Start is called before the first frame update
    public string characterName;
    public Sprite avator;
    public GameObject modelPrefab;


    public float maxHealth;
    public float attack;
    public float defense;
    public List<string> abilities;
    
}
