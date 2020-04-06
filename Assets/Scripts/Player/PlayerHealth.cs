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
    
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(playerNumber, health);
        }
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        health = playerParams.startingHealth;
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


    public void HitByPlayer (int projectilePlayerNumber)
    {
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
        playerParent.CharacterDied();

        Destroy(gameObject);
    }
}
