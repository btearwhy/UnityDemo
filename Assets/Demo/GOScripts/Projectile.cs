using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour, IPunInstantiateMagicCallback
{
    public VisualEffectAsset visualEffectAsset;
    public VisualEffectAsset impactVisualEffectAsset;
    public AudioSource flyingAudio;
    public AudioSource impactAudio;

    public GameObject instigator;
    public float effectRange;
    public float initSpeed;
    public float impactForce;

    public Effect effect;
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == gameObject) continue;
                if (hitCollider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    Vector3 force = hitCollider.transform.position - transform.position;
                    force /= force.magnitude * force.magnitude * force.magnitude;
                    force *= impactForce;
                    rigidbody.AddForce(force, ForceMode.Impulse);
                }
                effect.Apply(instigator, hitCollider.gameObject);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int photonViewID = (int)info.photonView.InstantiationData[0];
        instigator = PhotonView.Find(photonViewID).gameObject;
        GetComponent<Rigidbody>().velocity = (Vector3)info.photonView.InstantiationData[1];
    }
}
