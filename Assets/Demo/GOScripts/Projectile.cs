using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour, IPunInstantiateMagicCallback
{
    public VisualEffectAsset visualEffectAsset;
    public VisualEffectAsset impactVisualEffectAsset;
    public GameObject ImpactVisual;
    public AudioSource flyingAudio;
    public AudioSource impactAudio;

    public GameObject instigator;
    public float effectRange;
    public float initSpeed;
    public float impactForce;
    
    public List<Effect> effects;
    private void Start()
    {
        if(flyingAudio != null)
        {
            flyingAudio.Play();
        }
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //impactAudio.Play();
        if (impactAudio != null)
        {
            impactAudio.Play();
        }
        
        if (collision.collider.gameObject != instigator)
        {
            Invoke("Explode", 2.0f);
        }
    }

    public void Explode()
    {
        Instantiate(ImpactVisual, transform.position, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject) continue;
            if (hitCollider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                Vector3 force = hitCollider.transform.position - transform.position;
                force /= force.magnitude * (1 + force.magnitude) * (1 + force.magnitude);
                force *= impactForce;
                rigidbody.AddForce(force, ForceMode.Impulse);
            }
            foreach(Effect effect in effects){
                effect.Apply(instigator, hitCollider.gameObject);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] parameters = info.photonView.InstantiationData;
        int photonViewID = (int)parameters[0];
        instigator = PhotonView.Find(photonViewID).gameObject;
        GetComponent<Rigidbody>().velocity = (Vector3)parameters[1];
        for(int i = 2; i < parameters.Length; i++)
        {
            effects.Add((Effect)parameters[i]);
        }
    }
}
