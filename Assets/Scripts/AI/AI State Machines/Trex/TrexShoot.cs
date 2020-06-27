using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TrexShoot : StateMachineBehaviour
{
    private AI ai;
    private Perception perception;
    private Animator _Animator;
    private float movementSpeed;

    private float shootTimer;
    private AIProjectile aiProjectile;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        shootTimer = ai.aiType.attackCooldown;
        aiProjectile = Resources.Load<AIProjectile>("AIProjectiles/" + ai.aiType.projectileName);
        rigidbody = ai.GetComponent<Rigidbody2D>();
        spriteRenderer = ai.GetComponent<SpriteRenderer>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget == false)
        {
            animator.SetBool("TargetDetected", false);
        }
        if (shootTimer <= 0)
        {
            Shoot();
            Debug.Log("Shoot");
            shootTimer = ai.aiType.attackCooldown;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
         Debug.Log(shootTimer);
        rigidbody.velocity = Vector2.zero;

        FaceTarget();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void FaceTarget ()
    {
        if (perception.targetTransform == null)
        {
            return;
        }

        if (perception.targetTransform.position.x < ai.transform.position.x)
        {
            perception.isFacingRight = false;
            spriteRenderer.flipX = true;
        }
        else
        {
            perception.isFacingRight = true;
            spriteRenderer.flipX = false;
        }

    }

    void Shoot ()
    {
        if (perception.targetTransform == null)
        {
            return;
        }

        Vector2 firingPos = new Vector2(((perception.isFacingRight) ? ai.aiType.FiringPoint.x: -ai.aiType.FiringPoint.x) + ai.transform.position.x, ai.aiType.FiringPoint.y + ai.transform.position.y);
        float deviation = ai.aiType.bulletDeviation;
        Vector2 directionToTarget =  new Vector2(perception.targetTransform.position.x + Random.Range(-deviation,deviation), perception.targetTransform.position.y + Random.Range(-deviation, deviation)) -firingPos;
        AIProjectile projectile =  Instantiate(aiProjectile, firingPos, Quaternion.identity);
        projectile.InitialiseProjectile(ai.aiType.attackDamage,directionToTarget,ai.aiType.projectileForce,ai.aiType.attackRange);
    }
}
