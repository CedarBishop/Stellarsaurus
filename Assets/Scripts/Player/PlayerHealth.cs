using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem bloodSplatterParticle;
    public ParticleSystem shieldParticleSystem;
    public ParticleSystem healParticle;
    public SpriteRenderer[] spriteRenderers;
    public PlayerBattery playerBattery;

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
    private Consumable currentShieldConsumable;

    private Gamepad gamepad;
    private bool isGamepad;
    private bool hapticIsActive;
    
    void Start()
    {
        playerParent = GetComponentInParent<Player>();
        isAlive = true;

        playerParams = GameManager.instance.loader.saveObject.playerParams;
        health = playerParams.startingHealth;
        maxHealth = health;
        gamepad = Gamepad.current;
        isGamepad = playerParent.isGamepad;
        if (playerBattery != null)
        {
            playerBattery.gameObject.SetActive(true);
            playerBattery.Initialise(playerNumber);
        } 
    }

    private void Update()
    {
        if (transform.position.y < -50)
        {
            Debug.Log("Player fell off the map");
            Death();
        }
    }

    public void HitByAI(int damage)
    {
        if (isAlive == false)  // Already dead so cant be killed again
        {
            return;
        }
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
        ParticleSystem p = Instantiate(bloodSplatterParticle, transform.position, Quaternion.identity);
        p.Play();
        if (playerBattery != null)
        {
            playerBattery.gameObject.SetActive(true);
            playerBattery.UpdateHealth(health);
        }
        Destroy(p.gameObject,3);
        if (health <= 0)
        {
            isAlive = false;
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

        if (projectilePlayerNumber == playerNumber)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.Suicides, 1);
            }
        }

        if (isGamepad)
        {
            StartCoroutine("Haptic");
        }

        StartCoroutine("FlashHurt");
        JuiceManager.TimeSleep(playerParams.playerHitSlowMoDuration,playerParams.playerHitTimeScale, false);

        ParticleSystem p = Instantiate(bloodSplatterParticle, transform.position, Quaternion.identity);
        p.Play();
        Destroy(p.gameObject, 3);

        if (projectilePlayerNumber != playerNumber)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                GameManager.instance.AwardKill(projectilePlayerNumber);
                ScorePopup scorePopup = Instantiate(LevelManager.instance.scorePopupPrefab, transform.position, Quaternion.identity);
                scorePopup.Init(GameManager.instance.SelectedGamemode.playerKillsPointReward);
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
        foreach (var item in spriteRenderers)
        {
            item.color = Color.red;
        }
    }

    public void StopBurning()
    {
        if (isBurning)
        {
            isBurning = false;
            foreach (var item in spriteRenderers)
            {
                item.color = Color.white;
            }
        }
    }

    IEnumerator Burning(int projectilePlayerNumber)
    {
        yield return new WaitForSeconds(1.0f);
        while (health > 0 && isBurning)
        {
            health--;
            if (playerBattery != null)
            {
                playerBattery.gameObject.SetActive(true);
                playerBattery.UpdateHealth(health);
            }
            if (isGamepad)
            {
                StartCoroutine("Haptic");
            }
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
            yield return new WaitForSeconds(1.0f);
        }
    }

    void Death ()
    {
        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber,StatTypes.Deaths, 1);
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material.SetFloat("_IsGlitching", 1.0f);
        }

        playerParent.CharacterDied(true);
    }

    public void Heal(int healingAmount)
    {
        health += healingAmount;
        if (playerBattery != null)
        {
            playerBattery.gameObject.SetActive(true);
            playerBattery.UpdateHealth(health);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (healParticle != null)
        {
            ParticleSystem particle = Instantiate(healParticle, transform);
            Destroy(particle.gameObject, 3);
        }

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.HealthRegained, 1);
        }
    }


    public void GainShield(int amount, Consumable consumable)
    {
        if (currentShieldConsumable != null)
        {
            Destroy(currentShieldConsumable.gameObject);
        }
        currentShieldConsumable = consumable;

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
        currentShieldConsumable = null;
        hasShield = false;
        Destroy(shieldParticle);
    }

    IEnumerator FlashHurt ()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material.SetFloat("_IsHurt", 1.0f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderers[i].material.SetFloat("_IsHurt", 0.0f);
        }
    }

    IEnumerator Haptic ()
    {
        gamepad.SetMotorSpeeds(0.5f, 1.0f);
        gamepad.ResumeHaptics();
        hapticIsActive = true;
        yield return new WaitForSeconds(0.1f);
        gamepad.PauseHaptics();
        gamepad.ResetHaptics();
        hapticIsActive = false;
    }

    private void OnDestroy()
    {
        if (hapticIsActive)
        {
            gamepad.ResetHaptics();
        }
    }
}
