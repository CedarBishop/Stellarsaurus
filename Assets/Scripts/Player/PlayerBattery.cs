using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattery : MonoBehaviour
{
    private int playerNumber;
    private int health;
    private Image batteryImage;
    private Material batteryMaterial;

    public Sprite[] healthSprites;

    public void Initialise (int playerNum)
    {
        playerNumber = playerNum;
        health = 3;
        batteryImage.GetComponent<Image>();
        batteryMaterial = batteryImage.material;
        batteryMaterial.SetColor("PlayerColor", GameManager.instance.playerColours[playerNumber - 1]);
    }

    public void UpdateHealth (int value)
    {
        health = value;
        batteryImage.sprite = healthSprites[health - 1];
        batteryMaterial.SetFloat("HealthValue", health);
    }
}
