using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public LayoutGroup layoutGroup;
    [HideInInspector] public List<PlayerStats> playerStats = new List<PlayerStats>();
    public PlayerStats playerStatsPrefab;
    public Text roundText;

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
            }
        }
    }

    public void AwardRoundWin (int playerNumber)
    {
        foreach (PlayerStats player in playerStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.roundWins++;
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

    public void StartNewRound(int roundNumber)
    {
        roundText.text = "Round " + roundNumber.ToString();
    }

    public void EndRound(int winningPlayerNumber, int roundNumber)
    {
        if (winningPlayerNumber == 0)
        {
            roundText.text = "Nobody won round " + roundNumber.ToString();
        }
        else
        {
            AwardRoundWin(winningPlayerNumber);
            roundText.text = "Player " + winningPlayerNumber.ToString() + " won round " + roundNumber.ToString();
        }
        
    }

    public void EndMatch(List<int> winningPlayerNumbers)
    {
        string str = "";
        if (winningPlayerNumbers.Count == 1)
        {
            str = "Player " + winningPlayerNumbers[0].ToString() + " won the match";
        }
        else if (winningPlayerNumbers.Count > 1)
        {            
            for (int i = 0; i < winningPlayerNumbers.Count - 1; i++) 
            {
                str += "Player " + winningPlayerNumbers[i].ToString() + " and ";
            }
            str += "Player " + winningPlayerNumbers[winningPlayerNumbers.Count - 1].ToString() + " won the match";

        }

        roundText.text = str;
    }
}
