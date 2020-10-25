using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType { DoubleJump, Healing, Shield, SuperShield, SpeedBoost, InfiniteAmmo, SlowdownTime}

public class Consumable : MonoBehaviour
{
    public event Action OnPickUp;

    private Controller player;

    public ConsumableType consumableType;
    public Color color;
    public float duration;
    public float amount;

    private float timer;
    private bool isActive;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        timer = duration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            return;
        }
        if (collision.GetComponent<PlayerShoot>())
        {
            player = collision.GetComponent<PlayerShoot>().player;
            Destroy(collider);
            Destroy(rigidbody);
            spriteRenderer.enabled = false;
            Use();
        }
    }

    public virtual void Use()
    {
        if (OnPickUp != null)
        {
            OnPickUp();
        }
        isActive = true;
        switch (consumableType)
        {
            case ConsumableType.DoubleJump:
                player.playerMovement.CanDoubleJump(true, this);
                break;
            case ConsumableType.Healing:
                player.playerHealth.Heal((int)amount);
                break;
            case ConsumableType.Shield:
                player.playerHealth.GainShield((int)amount, this);
                break;
            case ConsumableType.SuperShield:
                break;
            case ConsumableType.SpeedBoost:
                player.playerMovement.IsSpeedBoosted(true, amount, this);
                break;
            case ConsumableType.InfiniteAmmo:
                break;
            case ConsumableType.SlowdownTime:
                player.playerMovement.IsSpeedBoosted(true, amount, this);
                JuiceManager.TimeSleep(duration, (amount > 0 )? 1/amount: 1.0f, true);
                break;

            default:
                break;
        }

        foreach (var renderer in player.playerHealth.spriteRenderers)
        {
            if (color != Color.white)
            {
                renderer.color = color;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0.0f)
            {
                Deactivate();
            }
        }
    }

    private void Deactivate ()
    {
        isActive = false;

        switch (consumableType)
        {
            case ConsumableType.DoubleJump:
                player.playerMovement.CanDoubleJump(false, null);
                break;
            case ConsumableType.Healing:
                break;
            case ConsumableType.Shield:
                player.playerHealth.EndShield();
                break;
            case ConsumableType.SuperShield:
                break;
            case ConsumableType.SpeedBoost:
                player.playerMovement.IsSpeedBoosted(false,1.0f, null);
                break;
            case ConsumableType.InfiniteAmmo:
                break;
            case ConsumableType.SlowdownTime:
                player.playerMovement.IsSpeedBoosted(false, 1.0f, null);
                break;
            default:
                break;
        }
        foreach (var renderer in player.playerHealth.spriteRenderers)
        {
            renderer.color = Color.white;
        }
        Destroy(gameObject);
    }
}
