using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType { DoubleJump, Healing, Shield, SuperShield, SpeedBoost, InfiniteAmmo, SlowdownTime}

public class Consumable : MonoBehaviour
{
    private Player player;

    private ConsumableType consumableType;

    private float timer;
    private bool isActive;

    public virtual void Use(Player _Player, ConsumableType type, float duration, float amount, Color color)
    {
        isActive = true;
        timer = duration;
        player = _Player;
        consumableType = type;

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
                JuiceManager.TimeSleep(duration, (amount > 0 )? 1/amount: 1.0f);
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
        timer -= Time.fixedDeltaTime;
        if (isActive)
        {
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
