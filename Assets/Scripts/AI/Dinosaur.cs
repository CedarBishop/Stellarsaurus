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


    public virtual void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null)
    {
        animator = GetComponent<Animator>();
        perception = GetComponent<Perception>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        pathFindingGrid = FindObjectOfType<PathFindingGrid>();

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

        //if (LevelManager.instance != null)
        //{
        //    if (LevelManager.instance.weaponTypes.Count > 0)
        //    {
        //        float rand = Random.Range(0.0f, 1.0f);
        //        if (rand < chanceOfDroppingWeapon)
        //        {
        //            OldWeapon weapon = Instantiate(LevelManager.instance.weaponPrefab,
        //            transform.position,
        //            Quaternion.identity);
        //            weapon.Init(LevelManager.instance.weaponTypes, WeaponSpawnType.FallFromSky);
        //        }
        //    }
        //}

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
}
