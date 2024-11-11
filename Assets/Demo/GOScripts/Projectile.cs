using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour, IPunInstantiateMagicCallback
{
    public GameObject FlyingVisual;
    public GameObject ImpactVisual;
    public AudioClip FlyingAudio;
    public AudioClip ImpactAudio;

    public GameObject instigator;
    public float effectRange;
    public float initSpeed;
    public float impactForce;
    public float explosionDelay;

    private AudioSource audioSource;
    public EffectContainer EffectContainer { get; set; }

    private bool collided;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = FlyingAudio;
        audioSource.Play();
        collided = false;
        if(FlyingVisual != null)
        {
            Instantiate(FlyingVisual, transform);
        }
        
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //impactAudio.Play();
        
        if (collision.collider.gameObject != instigator && !collided)
        {
            collided = true;
            Invoke("Explode", explosionDelay);
        }
    }

    public void Explode()
    {
        audioSource.PlayOneShot(ImpactAudio, AudioManager.soundRatio);
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
            if(hitCollider.gameObject.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
            {
                foreach (Effect effect in EffectContainer)
                {
                    battleSystem.ApplyEffect(effect, instigator);
                }
            }

        }
        gameObject.transform.localScale = Vector3.zero;
        if (PhotonNetwork.IsMasterClient)
        {
            IEnumerator DestroyDelay(GameObject gameObject)
            {
                yield return new WaitForSeconds(1.0f);

                PhotonNetwork.Destroy(gameObject);
            }
            StartCoroutine(DestroyDelay(gameObject));
        }
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] parameters = info.photonView.InstantiationData;
        int photonViewID = (int)parameters[0];
        instigator = PhotonView.Find(photonViewID).gameObject;
        GetComponent<Rigidbody>().velocity = (Vector3)parameters[1];
        GetComponent<Rigidbody>().useGravity = (bool)parameters[2];
        EffectContainer = (EffectContainer)parameters[3];
    }
}
