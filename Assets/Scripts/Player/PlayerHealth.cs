﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem bloodSplatterParticle;

    [HideInInspector]public int playerNumber;
    
    Player playerParent;
    PlayerParams playerParams;
    int health;
    int maxHealth;
    bool isAlive;
    
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        isAlive = true;
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(playerNumber, health);
        }
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        health = playerParams.startingHealth;
        maxHealth = health;
    }

    private void Update()
    {
        if (transform.position.y < -13)
        {
            Debug.Log("Player fell off the map");
            Death();
        }
    }

    public void HitByAI(int damage)
    {
        health -= damage;
        UIManager.instance.UpdateHealth(playerNumber,health);
        ParticleSystem p = Instantiate(bloodSplatterParticle,transform.position,Quaternion.identity);
        p.Play();
        Destroy(p.gameObject,3);
        if (health <= 0)
        {
            Death();
        }
    }


    public void HitByPlayer (int projectilePlayerNumber, bool canHurtSelf = false)
    {
        if (canHurtSelf == false)
        {
            if (projectilePlayerNumber == playerNumber)
            {
                return;
            }
        }
        if (isAlive == false)  // Already dead so cant be killed again
        {
            return;
        }
        isAlive = false;
        health = 0;

        UIManager.instance.UpdateHealth(playerNumber, health);
        ParticleSystem p = Instantiate(bloodSplatterParticle, transform.position, Quaternion.identity);
        p.Play();
        Destroy(p.gameObject, 3);

        if (projectilePlayerNumber != playerNumber)
        {
            UIManager.instance.AwardKill(projectilePlayerNumber);
        }
        Death();

    }


    void Death ()
    {
        playerParent.CharacterDied(true);

        Destroy(gameObject);
    }

    public void Heal(int healingAmount)
    {
        health += healingAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UIManager.instance.UpdateHealth(playerNumber, health);
    }
}
