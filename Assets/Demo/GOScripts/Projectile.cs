using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour, IPunInstantiateMagicCallback
{
    public GameObject FlyingVisual;
    public GameObject ImpactVisual;
    public AudioSource flyingAudio;
    public AudioSource impactAudio;

    public GameObject instigator;
    public float effectRange;
    public float initSpeed;
    public float impactForce;
    public float explosionDelay;
    public List<Effect> effects;

    private bool collided;
    private void Start()
    {
        collided = false;
        if(FlyingVisual != null)
        {
            Instantiate(FlyingVisual, transform);
        }
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
        
        if (collision.collider.gameObject != instigator && !collided)
        {
            collided = true;
            Invoke("Explode", explosionDelay);
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
            foreach (Effect effect in effects)
            {
                effect.Apply(instigator, hitCollider.gameObject);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            IEnumerator DestroyDelay(GameObject gameObject)
            {
                yield return new WaitForSeconds(1.0f);
                PhotonNetwork.Destroy(gameObject);
            }
            StartCoroutine(DestroyDelay(gameObject));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] parameters = info.photonView.InstantiationData;
        int photonViewID = (int)parameters[0];
        instigator = PhotonView.Find(photonViewID).gameObject;
        GetComponent<Rigidbody>().velocity = (Vector3)parameters[1];
        GetComponent<Rigidbody>().useGravity = (bool)parameters[2];
        for(int i = 3; i < parameters.Length; i++)
        {
            effects.Add((Effect)parameters[i]);
        }
    }
}
