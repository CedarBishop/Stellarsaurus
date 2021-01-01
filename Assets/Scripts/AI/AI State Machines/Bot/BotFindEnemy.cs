using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFindEnemy : StateMachineBehaviour
{
    BotBrain botBrain;
    BotController controller;
    private PlayerShoot playerShoot;
    private PlayerMovement playerMovement;
    private Transform transform;

    private PlayerMovement playerToMoveTo;

    private float timer;
    private float timeTillChecksForPlayerAgain = 1f;

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
        if (playerToMoveTo != null)
        {
            if (Vector3.Distance(transform.position, playerToMoveTo.transform.position) < playerShoot.currentWeapon.range)
            {
                animator.SetBool("IsAttacking", true);
            }
            else
            {
                MoveToPlayer();
            }
        }
        else
        {
            controller.OnMove(0);
            if (timer <= 0)
            {
                FindClosestPlayer();
                timer = timeTillChecksForPlayerAgain;
            }
            else
            {
                timer -= Time.fixedDeltaTime;
            }
        }

        if (playerShoot.currentWeapon == null)
        {
            animator.SetBool("HasWeapon", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        botBrain.target = playerToMoveTo.gameObject;
    }

    void FindClosestPlayer()
    {
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        PlayerMovement closestPlayer = null;
        foreach (var item in players)
        {
            if (item == playerMovement)
            {
                continue;
            }
            if (closestPlayer == null)
            {
                closestPlayer = item;
            }
            else
            {
                if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closestPlayer.transform.position))
                {
                    closestPlayer = item;
                }
            }
        }
        playerToMoveTo = closestPlayer;
    }

    void MoveToPlayer()
    {
        controller.OnMove(((playerToMoveTo.transform.position.x - transform.position.x) > 0) ? 1 : -1);
        if (playerToMoveTo.transform.position.y - transform.position.y > 0)
        {
            if (playerMovement.isGrounded)
            {
                controller.OnEndJump();
                controller.OnStartJump();
            }
        }
    }
}
