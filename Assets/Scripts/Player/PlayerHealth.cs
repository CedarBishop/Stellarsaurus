using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem bloodSplatterParticle;

    [HideInInspector]public int playerNumber;
    
    private Player playerParent;
    private PlayerParams playerParams;
    
    private int health;
    private int maxHealth;
    private bool isAlive;
    private bool isBurning;

    private bool hasShield;
    private int shieldBlocksRemaining;
    
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        isAlive = true;

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
        if (hasShield)
        {
            UseShield();
            return;
        }
        health -= damage;
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

    public void GainShield(bool value, int amount)
    {
        hasShield = value;
        shieldBlocksRemaining = amount;
         
    }

    public void UseShield()
    {
        shieldBlocksRemaining--;
        if (shieldBlocksRemaining <= 0)
        {
            hasShield = false;
        }
    }
}
