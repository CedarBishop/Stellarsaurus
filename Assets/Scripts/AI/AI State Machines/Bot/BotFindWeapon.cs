using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFindWeapon : StateMachineBehaviour
{
    BotBrain botBrain;
    BotController controller;
    private PlayerShoot playerShoot;
    private PlayerMovement playerMovement;
    private Transform transform;

    private Weapon weaponToMoveTo;

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
        if (playerShoot.currentWeapon != null)
        {
            animator.SetBool("HasWeapon", true);
        }

        if (weaponToMoveTo == null)
        {
            controller.OnEndJump();
            FindClosestWeapon();
        }
        else
        {
            MoveToClosestWeapon();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    void FindClosestWeapon()
    {
        Weapon[] weapons = FindObjectsOfType<Weapon>();
        Weapon closestWeapon = null;
        foreach (var item in weapons)
        {
            if (closestWeapon == null)
            {
                if (item.isHeld)
                {
                    continue;
                }
                else
                {
                    closestWeapon = item;
                }

            }
            else
            {
                if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closestWeapon.transform.position))
                {
                    if (item.isHeld == false)
                    {
                        closestWeapon = item;
                    }
                }
            }
        }

        weaponToMoveTo = closestWeapon;
    }

    void MoveToClosestWeapon()
    {
        if (Vector3.Distance(transform.position, weaponToMoveTo.transform.position) < 0.5f)
        {
            controller.OnGrab();
        }
        else
        {
            controller.OnMove(((weaponToMoveTo.transform.position.x - transform.position.x) > 0) ? 1 : -1);
            if (weaponToMoveTo.transform.position.y - transform.position.y > 0)
            {
                if (playerMovement.isGrounded)
                {
                    controller.OnEndJump();
                    controller.OnStartJump();
                }
            }
            
        }
    }
}
