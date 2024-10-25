using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public VisualEffectAsset visualEffectAsset;
    public VisualEffectAsset impactVisualEffectAsset;
    public AudioSource flyingAudio;
    public AudioSource impactAudio;

    public GameObject Instigator;

    public float initSpeed;
    Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = initSpeed * transform.forward;
        //flyingAudio.Play();
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //impactAudio.Play();
        if(collision.collider.gameObject != Instigator)
            Debug.Log("Collide with " + collision.collider.name);
    }
}
