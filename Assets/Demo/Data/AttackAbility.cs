using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackAbility : Ability
{
    public string animStartStateName;
    public string animHeldStateName;
    public string animReleaseStateName;

    public string fireSocket;
    private Transform fireSocketTransform;

    public AudioSource fireAudio;

    public GameObject projectilePrefab;

    public float maxAttackRange;
    public float initialAngle;
    public float speed;

    public int numberOfPoints;
    private float attackRange;

    LineRenderer lineRenderer;

    public override void Fire(Transform transform, Transform trans_projectileSpawnSocket)
    {
        base.Fire(transform, trans_projectileSpawnSocket);
        GameObject projectileObject = Instantiate(projectilePrefab, trans_projectileSpawnSocket.position, transform.rotation);
        projectileObject.GetComponent<Projectile>().Instigator = character;
    }

    internal override void Pressed()
    {
        movement.StopTranslation();
        animator.Play(animStartStateName);
    }

    internal override void Held()
    {
        movement.StopTranslation();
        animator.Play(animHeldStateName);
    }

    internal override void Released()
    {
        animator.Play(animReleaseStateName);
    }

    internal override void HandleAnimationEvent(string dispatch)
    {
        if ("Fire".Equals(dispatch))
        {
            Fire(character.transform, fireSocketTransform);
        }
        else if ("End".Equals(dispatch))
        {
            character.GetComponent<Movement>().ResetStatus();
        }
        
    }

    public override void Init(GameObject go)
    {
        base.Init(go);
        
        foreach(Transform ts in go.GetComponentsInChildren<Transform>())
        {
            if (fireSocket.Equals(ts.name))
            {
                fireSocketTransform = ts;
                break;
            }
        }

        lineRenderer = new GameObject().AddComponent<LineRenderer>();
    }

    public void DrawLine(Transform startTransform)
    {
        Rigidbody rb;
        Vector3 startPosition;
        Vector3 startVelocity;
        float initialForce = 15;
        Quaternion rot;
        int i = 0;
        float timer = 0.1f;
        i = 0;
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.enabled = true;
        startPosition = startTransform.position;
        startVelocity = rot * (InitialForce * startTransform.forward) / rb.mass;
        lr.SetPosition(i, startPosition);
        for (float j = 0; i < lr.positionCount - 1; j += timer)
        {
            i++;
            Vector3 linePosition = startPosition + j * startVelocity;
            linePosition.y = startPosition.y + startVelocity.y * j + 0.5f * Physics.gravity.y * j * j;
            lr.SetPosition(i, linePosition);
        }
    }
}
