﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem bloodSplatterParticle;
    public ParticleSystem shieldParticleSystem;
    public SpriteRenderer[] material;

    [HideInInspector]public int playerNumber;
    
    private Player playerParent;
    private PlayerParams playerParams;
    
    private int health;
    private int maxHealth;
    private bool isAlive;
    private bool isBurning;

    private bool hasShield;
    private int shieldBlocksRemaining;

    private GameObject shieldParticle;

    private Gamepad gamepad;
    private bool isGamepad;
    
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        isAlive = true;

        playerParams = GameManager.instance.loader.saveObject.playerParams;
        health = playerParams.startingHealth;
        maxHealth = health;
        gamepad = Gamepad.current;
        isGamepad = playerParent.isGamepad;
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
        if (hasShield)
        {
            UseShield();
            return;
        }
        health -= damage;
        if (isGamepad)
        {
            StartCoroutine("Haptic");
        }
        StartCoroutine("FlashHurt");
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
        if (hasShield)
        {
            UseShield();
            return;
        }

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

        if (isGamepad)
        {
            StartCoroutine("Haptic");
        }

        JuiceManager.TimeSleep(0.5f,0.1f);

        ParticleSystem p = Instantiate(bloodSplatterParticle, transform.position, Quaternion.identity);
        p.Play();
        Destroy(p.gameObject, 3);

        if (projectilePlayerNumber != playerNumber)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                GameManager.instance.AwardKill(projectilePlayerNumber);
            }            
        }
        Death();

    }

    public void HitByFlame(int projectilePlayerNumber, bool canHurtSelf = false)
    {
        if (canHurtSelf == false)
        {
            if (projectilePlayerNumber == playerNumber)
            {
                return;
            }
        }
        if (isBurning)
        {
            return;
        }
        if (isAlive == false)  // Already dead so cant be killed again
        {
            return;
        }

        isBurning = true;
        StartCoroutine(Burning(projectilePlayerNumber));
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    IEnumerator Burning(int projectilePlayerNumber)
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(1.0f);
            health--;
            if (isGamepad)
            {
                StartCoroutine("Haptic");
            }
            StartCoroutine("FlashHurt");
            if (health <= 0)
            {
                if (projectilePlayerNumber != playerNumber)
                {
                    if (GameManager.instance.SelectedGamemode != null)
                    {
                        GameManager.instance.AwardKill(projectilePlayerNumber);
                    }
                }
                Death();
            }
        }
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
    }

    public void GainShield(int amount)
    {
        hasShield = true;
        shieldBlocksRemaining = amount;
        shieldParticle = Instantiate(shieldParticleSystem, transform.position, Quaternion.identity).gameObject;
        shieldParticle.transform.parent = transform;
    }

    public void UseShield()
    {
        shieldBlocksRemaining--;
        if (shieldBlocksRemaining <= 0)
        {
            EndShield();
        }
    }

    public void EndShield()
    {
        hasShield = false;
        Destroy(shieldParticle);
    }

    IEnumerator FlashHurt ()
    {
        for (int i = 0; i < material.Length; i++)
        {
            material[i].material.SetFloat("_IsHurt", 1.0f);
            yield return new WaitForSeconds(0.2f);
            material[i].material.SetFloat("_IsHurt", 0.0f);
        }
    }

    IEnumerator Haptic ()
    {
        gamepad.SetMotorSpeeds(0.5f,1.0f);
        gamepad.ResumeHaptics();
        yield return new WaitForSeconds(0.1f);
        gamepad.PauseHaptics();
        gamepad.ResetHaptics();
    }
}
