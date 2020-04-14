using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem bloodSplatterParticle;

    [HideInInspector]public int playerNumber;
    
    Player playerParent;
    PlayerParams playerParams;
    int health;
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
    }

    private void Update()
    {
        if (transform.position.y < -13)
        {
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
}
