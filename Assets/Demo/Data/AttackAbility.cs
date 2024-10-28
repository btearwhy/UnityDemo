using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using VolumetricLines;

public class AttackAbility : Ability
{
    public string animStartStateName;
    public string animHeldStateName;
    public string animReleaseStateName;

    public string fireSocket;
    private Transform fireSocketTransform;

    public AudioSource fireAudio;

    public GameObject projectilePrefab;

    public float initialAngle;
    public float minAttackRange;
    public float maxAttackRange;
    public float attackRangeChargeSpeed;



    private float attackRange;

    LineRenderer lineRenderer;
    public int numberOfPoints;
    public Material lineMaterial;
    public float lineWidth;
    public override void Fire(Transform transform, Transform trans_projectileSpawnSocket)
    {
        base.Fire(transform, trans_projectileSpawnSocket);
        if (PhotonNetwork.IsMasterClient)
        {
            object[] parameters = new object[2];
            parameters[0] = character.GetPhotonView().ViewID;
            parameters[1] = getProjectileVelocity(character.transform.forward, attackRange, initialAngle);
            PhotonNetwork.Instantiate(projectilePrefab.name, trans_projectileSpawnSocket.position, Quaternion.AngleAxis(initialAngle, Vector3.Cross(character.transform.forward, character.transform.up)) * character.transform.rotation, 0, parameters);
        }
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
        if (PlayerState.IsUnderControll(character))
        {
            lineRenderer.enabled = true;
            attackRange += attackRangeChargeSpeed * Time.deltaTime;
            attackRange = Mathf.Clamp(attackRange, minAttackRange, maxAttackRange);
            DrawLine(fireSocketTransform.position, character.transform.forward, attackRange, initialAngle);
        }

    }

    internal override void Released()
    {
        lineRenderer.enabled = false;
        animator.Play(animReleaseStateName);
    }

    internal override void End()
    {
        base.End();
        attackRange = minAttackRange;
        character.GetComponent<Movement>().ResetStatus();
    }

    internal override void HandleAnimationEvent(string dispatch)
    {
        if ("Fire".Equals(dispatch))
        {
            Fire(character.transform, fireSocketTransform);
        }
        else if ("End".Equals(dispatch))
        {
            End();
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
        attackRange = minAttackRange;

        lineRenderer = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("lines", "LineStrip")).GetComponent<LineRenderer>();

        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
/*        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.black;
        lineRenderer.material.SetColor("lineColor", new Color(1, 1, 1, 0.5f));*/
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void DrawLine(Vector3 startPosition, Vector3 direction, float castRange, float pitchAngle)
    {

        int i = 0;
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.SetPosition(i, startPosition);
        Vector3 velocity = getProjectileVelocity(direction, castRange, pitchAngle);
        float timer = 0.2f;
        Vector3 lastPosition = lineRenderer.GetPosition(i);
        for (float j = 0; i < lineRenderer.positionCount - 1; j += timer)
        {
            i++;
            Vector3 curPosition = startPosition + j * velocity + 0.5f * j * j * Physics.gravity;


            Vector3 dir = curPosition - lastPosition;
            if (Physics.Raycast(lastPosition, dir, out RaycastHit hit, dir.magnitude))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
            }
            else
            {
                lineRenderer.SetPosition(i, curPosition);
            }
        }

    }



    private static Vector3 getProjectileVelocity(Vector3 direction, float castRange, float pitchAngle)
    {
        float pitchAngleRadians = pitchAngle * Mathf.PI / 180.0f;
        float velocityMagnitude = Mathf.Sqrt(-Physics.gravity.y * castRange / Mathf.Sin(2 * pitchAngleRadians));
        float velocityVertical = velocityMagnitude * Mathf.Sin(pitchAngleRadians);
        float velocityHorizontal = velocityMagnitude * Mathf.Cos(pitchAngleRadians);
        Vector3 velocity = new Vector3(direction.x * velocityHorizontal, velocityVertical, direction.z * velocityHorizontal);
        return velocity;
    }
}
