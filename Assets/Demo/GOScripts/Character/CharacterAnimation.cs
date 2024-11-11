using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private AbilitySystem abilitySystem;
    private AudioSource audioSource;

    public AudioClip footstep;

    // Start is called before the first frame update
    void Start()
    {
        abilitySystem = GetComponentInParent<AbilitySystem>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HandleAnimation(AnimationEvent animationEvent)
    {
        abilitySystem.abilities[animationEvent.intParameter].HandleAnimationEvent(animationEvent.stringParameter);
    }

    public void FootStepSound()
    {
        audioSource.PlayOneShot(footstep, AudioManager.soundRatio);
    }

    public void RunStepSound()
    {
        audioSource.PlayOneShot(footstep, AudioManager.soundRatio);
    }
}
