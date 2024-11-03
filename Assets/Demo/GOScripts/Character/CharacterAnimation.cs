using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
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
    
    public void HandleAnimation(AnimationEvent animationEvent)
    {
        abilitySystem.abilities[animationEvent.intParameter].HandleAnimationEvent(animationEvent.stringParameter);
    }
}
