using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Transform trans_projectileSpawnSocket;
    public AbilitySystem abilitySystem;
    // Start is called before the first frame update
    void Start()
    {
        abilitySystem = GetComponentInParent<AbilitySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackEvent(int abilityNo)
    {
        abilitySystem.actions[abilityNo].Fire(transform, trans_projectileSpawnSocket);
    }
}
