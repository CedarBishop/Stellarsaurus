using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexShoot : StateMachineBehaviour
{
    private AI ai;
    private Perception perception;
    private Animator _Animator;
    private float movementSpeed;

    private float shootTimer;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        shootTimer = ai.aiType.attackCooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
        }
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = ai.aiType.attackCooldown;
        }
        else
        {
            shootTimer -= Time.fixedDeltaTime;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void Shoot ()
    {
        float deviation = ai.aiType.bulletDeviation;
        Vector2 directionToTarget =  new Vector3(perception.targetTransform.position.x + Random.Range(-deviation,deviation), perception.targetTransform.position.y + Random.Range(-deviation, deviation), 0) - ai.transform.position;
        AIProjectile projectile =  Instantiate(ai.aiType.projectile, ai.aiType.FiringPoint, Quaternion.identity);
        projectile.InitialiseProjectile(ai.aiType.attackDamage,directionToTarget,ai.aiType.projectileForce,ai.aiType.attackRange);
    }
}
