using PlatformerPathFinding;
using PlatformerPathFinding.Examples;
using System.Collections;
using UnityEngine;

public class Dinosaur : MonoBehaviour
{
    public event System.Action OnHit;

    public int health;
    public float movementSpeed;
    public int attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float attackSize;
    public float wallDetectionDistance;
    [Range(0.0f,1.0f)]public float chanceOfDroppingWeapon;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public ParticleSystem bloodParticle;
    [HideInInspector] public PathFindingGrid pathFindingGrid;

    protected Material material;
    protected Animator animator;
    protected Perception perception;
    protected Rigidbody2D rigidbody;
    protected SpriteRenderer spriteRenderer;
    protected bool isBurning;
    protected Material aimSpriteMaterial;

    protected Weapon[] weaponsToSwawnOnDeath;


    public virtual void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null, Weapon[] weapons = null)
    {
        animator = GetComponent<Animator>();
        perception = GetComponent<Perception>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        pathFindingGrid = FindObjectOfType<PathFindingGrid>();
        weaponsToSwawnOnDeath = weapons;

        animator.SetBool("CanAttack", true);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_DinasaurSpawn");
        }
    }

    private void FixedUpdate()
    {
        DirectionChecker();
    }

    void DirectionChecker()
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

    public virtual void TakeDamage(int playerNumber, int damage)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_DinoHurt");
        }
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
        Destroy(p, 2);
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

    public void StopBurning()
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

    public virtual void Death(int playerNumber)
    {
        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AwardAiKill(playerNumber);
            ScorePopup scorePopup = Instantiate(LevelManager.instance.scorePopupPrefab, transform.position, Quaternion.identity);
            scorePopup.Init(GameManager.instance.SelectedGamemode.aiKillsPointReward);
        }

        SpawnWeapon();       

        Destroy(gameObject);
    }

    public void StartAttackCooldown()
    {
        StartCoroutine("CoAttackCooldown");
    }

    IEnumerator CoAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("CanAttack", true);
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

    public virtual Transform SetRandomGoal()
    {
        return null;
    }

    protected void SpawnWeapon ()
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand < chanceOfDroppingWeapon)
        {
            print("Should spawn weapon");
            Weapon weaponPrefab;
            if (weaponsToSwawnOnDeath.Length == 0)
            {
                return;
            }
            else if (weaponsToSwawnOnDeath.Length == 1)
            {
                weaponPrefab = weaponsToSwawnOnDeath[0];
            }
            else
            {
                weaponPrefab = weaponsToSwawnOnDeath[Random.Range(0, weaponsToSwawnOnDeath.Length)];
            }
            
            Weapon weapon = Instantiate(weaponPrefab,
            transform.position,
            Quaternion.identity);
            print(weapon.name);
        }
    }
}
