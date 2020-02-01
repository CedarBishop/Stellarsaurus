using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public LayoutGroup layoutGroup;
    List<PlayerStats> playerStats = new List<PlayerStats>();
    public PlayerStats playerStatsPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }


    public void CreateNewPlayerStats(int playerNumber)
    {
        PlayerStats p = Instantiate(playerStatsPrefab, layoutGroup.transform);
        p.playerNumber = playerNumber;
        p.playerNumberText.text = "P" + playerNumber.ToString();
        p.playerNumberText.color = GameManager.instance.playerColours[playerNumber - 1];
        playerStats.Add(p);
    }

    public void RemovePlayerStats(int playerNumber)
    {
        foreach (PlayerStats player in playerStats)
        {
            if (player.playerNumber == playerNumber)
            {
                GameObject g = player.gameObject;
                playerStats.Remove(player);
                Destroy(g);
            }
        }
    }

    public void UpdateHealth(int playerNumber, int newHealth)
    {
        foreach (PlayerStats player in playerStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.playerHealthText.text = "Health = " + newHealth.ToString();
            }
        }
    }

    public void AwardKill(int playerNumber)
    {
        foreach (PlayerStats player in playerStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.playerKills++;
                player.playerWinsText.text = "Kills = " + player.playerKills.ToString();

                if (player.playerKills >= LevelManager.instance.requiredKillsToWin)
                {
                    // display win screen of player number parameter
                }
            }
        }
    }

    public void UpdateWeaponType(int playerNumber, string weaponName, int ammoCount)
    {
        foreach (PlayerStats player in playerStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.ammoCountText.text = ammoCount.ToString();
                player.currentWeaponText.text = weaponName;
            }
        }
    }
}
