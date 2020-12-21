using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
    private BotController botController;
    private PlayerShoot playerShoot;
    private PlayerMovement playerMovement;

    private Weapon weaponToMoveTo;
    private bool isHoldingJumpKey;
    private PlayerMovement playerToMoveTo;

    void Start()
    {
        botController = GetComponentInParent<BotController>();
        playerShoot = GetComponent<PlayerShoot>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerShoot.currentWeapon != null)
        {
            if (playerToMoveTo != null)
            {
                if (Vector3.Distance(transform.position, playerToMoveTo.transform.position) < playerShoot.currentWeapon.range)
                {
                    AttackAimAtPlayer();
                }
                else
                {
                    MoveToPlayer();
                }
            }
            else
            {
                FindClosestPlayer();
            }
            
        }
        else
        {
            if (weaponToMoveTo == null)
            {
                FindClosestWeapon();
            }
            else
            {
                MoveToClosestWeapon();
            }
        }
    }

    void FindClosestWeapon ()
    {
        Weapon[] weapons = FindObjectsOfType<Weapon>();
        Weapon closestWeapon = null;
        foreach (var item in weapons)
        {
            if (closestWeapon == null)
            {
                closestWeapon = item;
            }
            else
            {
                if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closestWeapon.transform.position))
                {
                    closestWeapon = item;
                }
            }
        }

        weaponToMoveTo = closestWeapon;
    }

    void FindClosestPlayer ()
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

    void MoveToClosestWeapon ()
    {
        if (Vector3.Distance(transform.position, weaponToMoveTo.transform.position) < 1)
        {
            botController.OnGrab();
        }
        else
        {
            botController.OnMove(((weaponToMoveTo.transform.position.x - transform.position.x) > 0)?1:-1);
            if (isHoldingJumpKey)
            {

            }
            else
            {
            }
        }
    }

    void MoveToPlayer ()
    {

    }

    void AttackAimAtPlayer ()
    {

    }
}
