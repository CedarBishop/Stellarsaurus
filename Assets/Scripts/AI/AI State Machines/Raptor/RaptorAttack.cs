using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAttack : StateMachineBehaviour
{
    private Raptor ai;
    private Perception perception;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<Raptor>();
        perception = animator.GetComponent<Perception>();
        Attack(animator);
    }

    void Attack (Animator animator)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_RaptorBite");
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(animator.transform.position + (((perception.isFacingRight)? (Vector3.right): Vector3.left) * ai.attackRange), ai.attackSize);

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<PlayerHealth>())
                {
                    collider.GetComponent<PlayerHealth>().HitByAI(ai.attackDamage);
                }
            }
        }              
        
        animator.SetBool("CanAttack",false);
        ai.StartAttackCooldown();
    }
}
