using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AI : MonoBehaviour
{
    public AnimatorController patrolController;
    public AnimatorController guardController;
    public AnimatorController flyerController;
    public AnimatorController carrierController;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;

    public ParticleSystem bloodParticle;

    [HideInInspector] public AIType aiType;
    [HideInInspector] public int health;
    [HideInInspector] public float moveMentSpeed;
    [HideInInspector] public int attackDamage;
    [HideInInspector] public float attackCoolDown;

    private Animator animator;
    private Perception perception;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    private AIBehaviour behaviour;  



    public virtual void Initialise (AIType aIType)
    {
        animator = GetComponent<Animator>();
        perception = GetComponent<Perception>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        aiType = aIType;
        spriteRenderer.sprite = aiType.aiSprite;
        health = aiType.health;
        moveMentSpeed = aiType.moveMentSpeed;
        attackDamage = aiType.attackDamage;
        attackCoolDown = aiType.attackCoolDown;


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
        ParticleSystem p = Instantiate(bloodParticle, transform.position, Quaternion.identity);
        p.Play();
        Destroy(p, 1);
    }

    public virtual void Death (int playerNumber)
    {
        if (UIManager.instance != null)
        {

        }
        

        Destroy(gameObject);
    }

    public void StartAttackCooldown()
    {
        StartCoroutine("CoAttackCooldown");
    }

    IEnumerator CoAttackCooldown ()
    {
        yield return new WaitForSeconds(attackCoolDown);
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
    public float moveMentSpeed;
    public int attackDamage;
    public float attackCoolDown;

    public AIBehaviour aiBehaviour;

    public float smallJumpHeight;
    public float largeJumpHeight;

}
