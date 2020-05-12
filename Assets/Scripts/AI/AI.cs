using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int health;
    public float moveMentSpeed;
    public int attackDamage;
    public float attackCoolDown;

    public void Initialise (int Health, float MovementSpeed, int AttackDamage, float AttackCooldown)
    {
        health = Health;
        moveMentSpeed = MovementSpeed;
        attackDamage = AttackDamage;
        attackCoolDown = AttackCooldown;
    }

    public void TakeDamage (int playerNumber,int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death(playerNumber);
        }
    }

    public void Death (int playerNumber)
    {
        if (UIManager.instance != null)
        {

        }
    }
}

public enum AIBehaviour { Patrol, Guard, Fly }

[System.Serializable]
public class AIType
{
    public string AIName;
    public string spritePrefabName;
    public int health;
    public float moveMentSpeed;
    public int attackDamage;
    public float attackCoolDown;

    public AIBehaviour aiBehaviour;

}
