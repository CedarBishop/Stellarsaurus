using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public event System.Action OnHit;

    public RuntimeAnimatorController patrolController;
    public RuntimeAnimatorController guardController;
    public RuntimeAnimatorController flyerController;
    public RuntimeAnimatorController carrierController;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;

    public ParticleSystem bloodParticle;

    [HideInInspector] public AIType aiType;
    [HideInInspector] public int health;

    private Animator animator;
    private Perception perception;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D collider;

    private AIBehaviour behaviour;

    private bool isBurning;
    [HideInInspector] public Vector2 startingPosition;


    public virtual void Initialise (AIType aIType)
    {
        animator = GetComponent<Animator>();
        perception = GetComponent<Perception>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();


        aiType = aIType;
        spriteRenderer.sprite = aiType.aiSprite;
        health = aiType.health;

        perception.viewingDistance = aiType.viewingDistance;
        perception.fieldOfView = aiType.fieldOfView;
        perception.hearingRadius = aiType.hearingRadius;

        collider.offset = aiType.colliderOffset;
        collider.size = aiType.colliderSize;

        startingPosition = transform.position;
        behaviour = aiType.aiBehaviour;
        switch (behaviour)
        {
            case AIBehaviour.Patrol:
                animator.runtimeAnimatorController = patrolController;
                animator.SetBool("CanAttack", true);
                break;
            case AIBehaviour.Guard:
                animator.runtimeAnimatorController = guardController;
                break;
            case AIBehaviour.Fly:
                animator.runtimeAnimatorController = flyerController;
                break;
            case AIBehaviour.Carrier:
                animator.runtimeAnimatorController = carrierController;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        DirectionChecker();
    }

    void DirectionChecker ()
    {
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

    public virtual void TakeDamage (int playerNumber,int damage)
    {        
        health -= damage;
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
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    IEnumerator Burning(int projectilePlayerNumber)
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(1.0f);
            health--;
            if (health <= 0)
            {
                Death(projectilePlayerNumber);
            }
        }
    }

    public virtual void Death (int playerNumber)
    {
        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AwardAiKill(playerNumber);
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
}

public enum AIBehaviour { Patrol, Guard, Fly, Carrier }

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

    public Vector2 colliderSize;
    public Vector2 colliderOffset;

    public float viewingDistance;
    public float fieldOfView;
    public float hearingRadius;

    public float chanceOfDroppingWeapon;

    public AIBehaviour aiBehaviour;

    public float smallJumpHeight;
    public float largeJumpHeight;
    public float jumpDetectionDistance;

    public Vector2 FiringPoint;
    public AIProjectile projectile;
    public float projectileForce;
    public float bulletDeviation;

    public float swoopSpeed;

}
