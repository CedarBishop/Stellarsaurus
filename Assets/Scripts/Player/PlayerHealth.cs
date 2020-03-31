using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startinghealth = 3;
    public ParticleSystem bloodSplatterParticle;

    Player playerParent;
    public int playerNumber;
    int health;
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        health = startinghealth;
        UIManager.instance.UpdateHealth(playerNumber, health);
    }

    public void TakeDamage(int damage, int projectilePlayerNumber)
    {
        health -= damage;
        UIManager.instance.UpdateHealth(playerNumber,health);
        ParticleSystem p = Instantiate(bloodSplatterParticle,transform.position,Quaternion.identity);
        p.Play();
        Destroy(p.gameObject,3);
        if (health <= 0)
        {
            if (projectilePlayerNumber != playerNumber)
            {
                UIManager.instance.AwardKill(projectilePlayerNumber);
            }
            Death();
        }
    }

    void Death ()
    {
        playerParent.CharacterDied();

        Destroy(gameObject);
    }
}
