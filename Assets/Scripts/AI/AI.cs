using PlatformerPathFinding;
using PlatformerPathFinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public event System.Action OnHit;

    public RuntimeAnimatorController patrolController;
    public RuntimeAnimatorController jumpPatrolController;
    public RuntimeAnimatorController guardController;
    public RuntimeAnimatorController flyerController;
    public RuntimeAnimatorController carrierController;

    public Material patrolMaterial;
    public Material jumpPatrolMaterial;
    public Material guardMaterial;
    public Material flyerMaterial;
    public Material carrierMaterial;

    public PathFindingAgent agent;
    public AiController controller;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public ParticleSystem bloodParticle;
    public Egg eggPrefab;

    public Transform aimOrigin;
    public SpriteRenderer aimSpriteRenderer;

    public Sprite trexArmSprite;

    [HideInInspector] public AIType aiType;
    [HideInInspector] public PathFindingGrid pathFindingGrid;
    [HideInInspector] public AStar aStar;
    [HideInInspector] public int health;
    [HideInInspector] public Vector2 startingPosition;

    private Animator animator;
    private Perception perception;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D collider;

    private AIBehaviour behaviour;

    private bool isBurning;

    private Material material;
    private Material aimSpriteMaterial;

    private Transform[] targetsInMap;
    private Transform[] pteroAirTargets;
    [HideInInspector] public PairTargets[] pteroGroundTargets;


    public virtual void Initialise (AIType aIType, Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null)
    {
        animator = GetComponent<Animator>();
        perception = GetComponent<Perception>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        material = spriteRenderer.material;
        aStar = FindObjectOfType<AStar>();
        pathFindingGrid = aStar.GetComponent<PathFindingGrid>();

        aiType = aIType;
        spriteRenderer.sprite = aiType.aiSprite;
        health = aiType.health;

        perception.viewingDistance = aiType.viewingDistance;
        perception.fieldOfView = aiType.fieldOfView;
        perception.hearingRadius = aiType.hearingRadius;
        perception.targetMemoryTime = aiType.targetMemoryTime;        

        startingPosition = transform.position;
        behaviour = aiType.aiBehaviour;

        targetsInMap = transforms;
        pteroAirTargets = pteroAir;
        pteroGroundTargets = pteroGround;

        aimSpriteRenderer.enabled = false;

        switch (behaviour)
        {
            case AIBehaviour.Patrol:
                RefreshCollider(false);
                controller.enabled = false;
                agent.enabled = false;
                animator.runtimeAnimatorController = patrolController;
                spriteRenderer.material = patrolMaterial;
                animator.SetBool("CanAttack", true);

                break;
            case AIBehaviour.Guard:
                RefreshCollider(false);
                controller.enabled = false;
                agent.enabled = false;
                animator.runtimeAnimatorController = guardController;
                aimSpriteRenderer.enabled = true;
                aimSpriteRenderer.sprite = trexArmSprite;
                aimSpriteMaterial = aimSpriteRenderer.material;
                spriteRenderer.material = guardMaterial;
                break;
            case AIBehaviour.Fly:
                RefreshCollider(true);
                controller.enabled = false;
                agent.enabled = false;
                animator.runtimeAnimatorController = flyerController;
                collider.isTrigger = true;
                spriteRenderer.material = flyerMaterial;
                break;
            case AIBehaviour.Carrier:
                RefreshCollider(true);
                controller.enabled = false;
                agent.enabled = false;
                animator.runtimeAnimatorController = carrierController;
                spriteRenderer.material = carrierMaterial;
                break;
            case AIBehaviour.JumpPatrol:
                RefreshCollider(true);
                controller.enabled = true;
                agent.enabled = true;
                agent.Init(FindObjectOfType<PathFindingGrid>());
                animator.runtimeAnimatorController = jumpPatrolController;
                perception.isUsingAIController = true;
                animator.SetBool("CanAttack", true);
                Destroy(GetComponent<Rigidbody2D>());
                spriteRenderer.material = jumpPatrolMaterial;

                agent._fallLimit = aiType.fallLimit;
                agent._jumpStrength = (int)aiType.jumpStrength;
                controller._walkSpeed = aiType.movementSpeed;
                controller._jumpSpeed = aiType.jumpSpeed;
                controller._fallSpeed = aiType.fallSpeed;
                break;
            default:
                break;
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_DinasaurSpawn");
        }
    }

    private void FixedUpdate()
    {
        DirectionChecker();
    }

    void DirectionChecker ()
    {
        if (rigidbody == null)
        {
            return;
        }

        if (rigidbody.velocity.x > 0)
        {
            perception.isFacingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (rigidbody.velocity.x < 0)
        {
            perception.isFacingRight = false;
            spriteRenderer.flipX = true;
        }
    }

    public void RefreshCollider (bool isTrigger)
    {
        Destroy(collider);
        collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = isTrigger;
    }

    public virtual void TakeDamage (int playerNumber,int damage)
    {        
        health -= damage;
        StartCoroutine("FlashHurt");
        if (health <= 0)
        {
            Death(playerNumber);
        }
        else
        {
            if (OnHit != null)
            {
                OnHit();
            }
        }
        ParticleSystem p = Instantiate(bloodParticle, transform.position, Quaternion.identity);
        p.Play();
        Destroy(p, 1);
    }


    public void HitByFlame(int projectilePlayerNumber)
    {
        if (isBurning)
        {
            return;
        }

        isBurning = true;
        StartCoroutine(Burning(projectilePlayerNumber));
        spriteRenderer.color = Color.red;
    }

    public void StopBurning ()
    {
        if (isBurning)
        {
            isBurning = false;
            spriteRenderer.color = Color.white;
        }
    }

    IEnumerator Burning(int projectilePlayerNumber)
    {
        yield return new WaitForSeconds(1.0f);
        while (health > 0)
        {
            health--;
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFX("SFX_DinoBurn");
            }
            if (health <= 0)
            {
                Death(projectilePlayerNumber);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public virtual void Death (int playerNumber)
    {
        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AwardAiKill(playerNumber);
            ScorePopup scorePopup = Instantiate(LevelManager.instance.scorePopupPrefab, transform.position, Quaternion.identity);
            scorePopup.Init(GameManager.instance.SelectedGamemode.aiKillsPointReward);
        }

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.weaponTypes.Count > 0)
            {
                float rand = Random.Range(0.0f,1.0f);
                if (rand < aiType.chanceOfDroppingWeapon)
                {
                    Weapon weapon = Instantiate(LevelManager.instance.weaponPrefab,
                    transform.position,
                    Quaternion.identity);
                    weapon.Init(LevelManager.instance.weaponTypes, WeaponSpawnType.FallFromSky);
                }                
            }
        }

        Destroy(gameObject);
    }

    public void StartAttackCooldown()
    {
        StartCoroutine("CoAttackCooldown");
    }

    IEnumerator CoAttackCooldown ()
    {
        yield return new WaitForSeconds(aiType.attackCooldown);
        animator.SetBool("CanAttack", true);
    }

    public Transform SetRandomGoal()
    {
        switch (behaviour)
        {
            case AIBehaviour.Patrol:
                break;
            case AIBehaviour.Guard:
                break;
            case AIBehaviour.Fly:
                if (pteroAirTargets != null)
                {
                    return pteroAirTargets[Random.Range(0, pteroAirTargets.Length)];
                }
                break;
            case AIBehaviour.Carrier:
                if (pteroAirTargets != null)
                {
                    return pteroAirTargets[Random.Range(0, pteroAirTargets.Length)];
                }
                break;
            case AIBehaviour.JumpPatrol:
                if (targetsInMap != null)
                {
                    controller._goal = targetsInMap[Random.Range(0, targetsInMap.Length)];
                }
                break;
            default:
                break;
        }

        return null;        
    }

    IEnumerator FlashHurt()
    {
        if (aimSpriteMaterial != null)
            aimSpriteMaterial.SetFloat("_IsHurt", 1.0f);
        material.SetFloat("_IsHurt", 1.0f);
        yield return new WaitForSeconds(0.2f);
        material.SetFloat("_IsHurt", 0.0f);
        if (aimSpriteMaterial != null)
            aimSpriteMaterial.SetFloat("_IsHurt", 0.0f);
    }
}

public enum AIBehaviour { Patrol, Guard, Fly, Carrier, JumpPatrol }

[System.Serializable]
public class AIType
{
    public string AIName;
    public Sprite aiSprite;
    public string spriteName;
    public int health;
    public float movementSpeed;
    public int attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float attackSize;

    public float viewingDistance;
    public float fieldOfView;
    public float hearingRadius;
    public float targetMemoryTime;

    public float chanceOfDroppingWeapon;

    public AIBehaviour aiBehaviour;

    public float smallJumpHeight;
    public float largeJumpHeight;

    public float jumpSpeed;
    public int jumpStrength;
    public float fallSpeed;
    public int fallLimit;
    public float targetResetTime;

    public float wallDetectionDistance;
    public Vector2 FiringPoint;
    public string projectileName;
    public float projectileForce;
    public float bulletDeviation;

    public float swoopSpeed;
    public float pathFindingSwoopSpeed;
    public float retreatSpeed;
    public float minTimeBetweenSwoop;
    public float maxTimeBetweenSwoop;

    public Vector2 eggOffset;

}
