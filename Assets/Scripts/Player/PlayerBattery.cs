using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattery : MonoBehaviour
{
    private int playerNumber;
    private int health;
    private SpriteRenderer spriteRenderer;
    private Material batteryMaterial;

    public Sprite[] healthSprites;
    public float timeBeforeDeactivates;

    private float timer;

    public void Initialise (int playerNum)
    {
        playerNumber = playerNum;
        health = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        batteryMaterial = spriteRenderer.material;
        print("Battery Initialise: " + playerNumber);
        batteryMaterial.SetColor("PlayerColor", GameManager.instance.playerColours[playerNumber - 1]);
        batteryMaterial.SetFloat("HealthValue", health);
        timer = timeBeforeDeactivates;
    }

    public void UpdateHealth (int value)
    {
        if (value < 0 && value >= healthSprites.Length)
        {
            return;
        }
        health = value;
        spriteRenderer.sprite = healthSprites[playerNumber - 1];
        batteryMaterial.SetFloat("HealthValue", health);
        timer = timeBeforeDeactivates;
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer -= Time.fixedDeltaTime;
        }
    }
}
