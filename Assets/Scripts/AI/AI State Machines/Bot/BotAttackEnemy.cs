using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttackEnemy : StateMachineBehaviour
{
    BotBrain botBrain;
    BotController controller;
    private PlayerShoot playerShoot;
    private PlayerMovement playerMovement;
    private Transform transform;

    private Weapon weaponToMoveTo;
    private PlayerMovement playerToMoveTo;

    private bool isHoldingJumpKey;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        botBrain = animator.GetComponent<BotBrain>();
        controller = botBrain.GetComponentInParent<BotController>();
        playerShoot = botBrain.GetComponent<PlayerShoot>();
        playerMovement = botBrain.GetComponent<PlayerMovement>();
        transform = botBrain.transform;

        controller.OnMove(0);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerShoot == null)
        {
            return;
        }
        if (botBrain.target == null)
        {
            animator.SetBool("IsAttacking", false);
        }

        if (playerShoot.currentWeapon == null)
        {
            animator.SetBool("HasWeapon", false);
        }

        if (botBrain.target != null)
        {
            if (botBrain.target.transform != null)
            {
                Vector2 directionToTarget = (botBrain.target.transform.position - transform.position).normalized;
                controller.OnAim(directionToTarget);

                if (playerShoot != null)
                {
                    if (playerShoot.currentWeapon.canShoot)
                    {
                        controller.OnStartFire();
                    }
                    else
                    {
                        controller.OnEndFire();
                    }
                }
                
            }
        }       

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    Vector2 TranslateToEightDirection(Vector2 v)
    {
        Vector2 result = v;

        if (Mathf.Abs(v.x) < 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Up & Down
            if (v.y > 0)
            {
                result = Vector2.up;
            }
            else
            {
                result = Vector2.down;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) < 0.25f)
        {
            // Left & Right
            if (v.x > 0)
            {
                result = Vector2.right;
            }
            else
            {
                result = new Vector2(-1, 0.01f);
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Diagonals
            if (v.x < -0.25f && v.y < -0.25f)
            {
                // down left
                result = new Vector2(-1, -1);
            }
            else if (v.x > 0.25f && v.y < -0.25f)
            {
                // down right
                result = new Vector2(1, -1);
            }
            else if (v.x > 0.25f && v.y > 0.25f)
            {
                // up right
                result = new Vector2(1, 1);
            }
            else if (v.x < -0.25f && v.y > 0.25f)
            {
                // up left
                result = new Vector2(-1, 1);
            }

        }
        else
        {
            result = transform.right;
        }

        return result;
    }
}
