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
    public bool projectileGravity;
    public float projectileSpeed;

    public float initialAngle;
    public float minAttackRange;
    public float maxAttackRange;
    public float attackRangeChargeSpeed;
    public Effect effect;



    private float attackRange;

    public string lineName;
    [field: NonSerialized]
    private LineRenderer lineRenderer;
    private int numberOfPoints;

    public Ability_Attack() { }

    public Ability_Attack(string data):base(data)
    {
        
    }

    public override void Initialize()
    {
        Ability_Attack_Data ability_attack_data = (Ability_Attack_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", data);
        this.animStartStateName = ability_attack_data.animStartStateName;
        this.animHeldStateName = ability_attack_data.animHeldStateName;
        this.animReleaseStateName = ability_attack_data.animReleaseStateName;
        this.fireSocket = ability_attack_data.fireSocket;
        this.projectileName = ability_attack_data.projectilePrefab.name ;
        this.projectileGravity = ability_attack_data.projectileGravity;
        this.projectileSpeed = ability_attack_data.projectileSpeed; 
        this.initialAngle = ability_attack_data.initialAngle;
        this.minAttackRange = ability_attack_data.minAttackRange;
        this.maxAttackRange = ability_attack_data.maxAttackRange;
        this.attackRangeChargeSpeed = ability_attack_data.attackRangeChargeSpeed;
        this.effect = ability_attack_data.effect_Data.CreateInstance();
        this.lineRenderer = GameObject.Instantiate(ability_attack_data.lineRenderer).GetComponent<LineRenderer>();
    }

    public override void Fire(Transform transform, Transform trans_projectileSpawnSocket)
    {
        base.Fire(transform, trans_projectileSpawnSocket);
        
        if (PhotonNetwork.IsMasterClient)
        {
            List<Effect> effects = new List<Effect>();
            if(character.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
            {
                Buff_Instant buff = battleSystem.GetOneAttackAttachedBuff();
                if(buff != null)
                {
                    effects.AddRange(buff.EffectsOnSelf);
                    effects.AddRange(buff.EffectsOnEnemy);
                    battleSystem.RemoveBuff(buff);
                }
            }
            object[] parameters = new object[4 + effects.Count];
            parameters[0] = character.GetPhotonView().ViewID;
            parameters[1] = GetProjectileVelocity(character.transform.forward, attackRange, initialAngle, projectileGravity?Physics.gravity:Vector3.zero, projectileSpeed);
            parameters[2] = projectileGravity;
            int i = 3;
            for(; i < effects.Count + 3; i++)
            {
                parameters[i] = effects[i - 3];
            }
            parameters[i] = effect;
            PhotonNetwork.Instantiate(projectileName, trans_projectileSpawnSocket.position, Quaternion.AngleAxis(initialAngle, Vector3.Cross(character.transform.forward, character.transform.up)) * character.transform.rotation, 0, parameters);
        }
        OnFired?.Invoke();
    }

    internal override void Pressed()
    {
        base.Pressed();
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
            DrawLine(fireSocketTransform.position, character.transform.forward, attackRange, initialAngle, projectileGravity?Physics.gravity:Vector3.zero);
        }

    }

    internal override void Released()
    {
        base.Released();
        lineRenderer.enabled = false;
        
    }

    internal override void End()
    {
        base.End();
        attackRange = minAttackRange;
        character.GetComponent<Movement>().ResetStatus();
        OnEnded?.Invoke();
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

    public void DrawLine(Vector3 startPosition, Vector3 direction, float castRange, float pitchAngle, Vector3 gravity)
    {
        lineRenderer.positionCount = numberOfPoints;
        int i = 0;

        lineRenderer.SetPosition(i, startPosition);
        Vector3 velocity = GetProjectileVelocity(direction, castRange, pitchAngle, gravity, projectileSpeed);
        float timer = 0.2f;
        Vector3 lastPosition = lineRenderer.GetPosition(i);
        i++;
        for (float j = 0; i < lineRenderer.positionCount; j += timer)
        {
            Vector3 curPosition = startPosition + j * velocity + 0.5f * j * j * gravity;


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



    private static Vector3 GetProjectileVelocity(Vector3 direction, float castRange, float pitchAngle, Vector3 gravity, float projectileSpeed)
    {
        if (gravity == Vector3.zero) return direction * projectileSpeed; 
        float pitchAngleRadians = pitchAngle * Mathf.PI / 180.0f;
        float velocityMagnitude = Mathf.Sqrt(-gravity.y * castRange / Mathf.Sin(2 * pitchAngleRadians));
        float velocityVertical = velocityMagnitude * Mathf.Sin(pitchAngleRadians);
        float velocityHorizontal = velocityMagnitude * Mathf.Cos(pitchAngleRadians);
        Vector3 velocity = new Vector3(direction.x * velocityHorizontal, velocityVertical, direction.z * velocityHorizontal);
        return velocity;
    }
}
