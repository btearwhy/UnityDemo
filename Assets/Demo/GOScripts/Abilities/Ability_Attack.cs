using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability_Attack : Ability
{

    public string fireSocket;

    [field:NonSerialized]
    private Transform fireSocketTransform;

    [field:NonSerialized]
    private AudioSource fireAudio;

    public string projectileName;

    
    public float initialAngle;
    public float minAttackRange;
    public float maxAttackRange;
    public float attackRangeChargeSpeed;
    public List<Effect> effects;



    private float attackRange;

    public string lineName;
    [field: NonSerialized]
    private LineRenderer lineRenderer;
    private int numberOfPoints;

    public Ability_Attack() { }
    public Ability_Attack(string animStartStateName, string animHeldStateName, string animReleaseStateName, string fireSocket, string projectileName, float initialAngle, float minAttackRange, float maxAttackRange, float attackRangeChargeSpeed, List<Effect> effects, GameObject linePrefab):base(animStartStateName, animHeldStateName, animReleaseStateName)
    {
        this.fireSocket = fireSocket;
        this.projectileName = projectileName;
        this.initialAngle =  initialAngle;
        this.minAttackRange = minAttackRange;
        this.maxAttackRange = maxAttackRange;
        this.attackRangeChargeSpeed = attackRangeChargeSpeed;
        this.effects = effects;
        this.lineRenderer = GameObject.Instantiate(linePrefab).GetComponent<LineRenderer>();
    }

    public override void Fire(Transform transform, Transform trans_projectileSpawnSocket)
    {
        base.Fire(transform, trans_projectileSpawnSocket);
        if (PhotonNetwork.IsMasterClient)
        {
            //int parameterCount = effectsToCarry.Count + 2 + effects.Count;
            int parameterCount = 2 + effects.Count;
            object[] parameters = new object[parameterCount];
            parameters[0] = character.GetPhotonView().ViewID;
            parameters[1] = getProjectileVelocity(character.transform.forward, attackRange, initialAngle);
            int i = 2;
            foreach (Effect effect in effects)
            {
                parameters[i] = effect;
                i++;
            }
            PhotonNetwork.Instantiate(projectileName, trans_projectileSpawnSocket.position, Quaternion.AngleAxis(initialAngle, Vector3.Cross(character.transform.forward, character.transform.up)) * character.transform.rotation, 0, parameters);
        }
    }

    internal override void Pressed()
    {
        base.Held();
        movement.StopTranslation();
    }


    internal override void Held()
    {
        base.Held();
        movement.StopTranslation();

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
        base.Held();
        lineRenderer.enabled = false;
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

        foreach (Transform ts in go.GetComponentsInChildren<Transform>())
        {
            if (fireSocket.Equals(ts.name))
            {
                fireSocketTransform = ts;
                break;
            }
        }
        attackRange = minAttackRange;

        lineRenderer = GameObject.Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("lines", "LineStrip")).GetComponent<LineRenderer>();
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        numberOfPoints = 30;
    }

    public void DrawLine(Vector3 startPosition, Vector3 direction, float castRange, float pitchAngle)
    {
        lineRenderer.positionCount = numberOfPoints;
        int i = 0;

        lineRenderer.SetPosition(i, startPosition);
        Vector3 velocity = getProjectileVelocity(direction, castRange, pitchAngle);
        float timer = 0.2f;
        Vector3 lastPosition = lineRenderer.GetPosition(i);
        i++;
        for (float j = 0; i < lineRenderer.positionCount; j += timer)
        {
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
            i++;
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
