using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablePotion : Weapon
{
    ConsumableType consumableType;
    public float duration;
    public float amount;
    public Color effectColor;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        // Spawn a Empty gameobject then add a consumable component to it,
        GameObject go = Instantiate(new GameObject(), playerShoot.transform);
        Consumable consumable = go.AddComponent<Consumable>();

        // Activate Consumable effect with parameters of current weapon consumable
        consumable.Use(playerShoot.player, consumableType, duration, amount, effectColor);
    }
}
