using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public Image panelBackgroundImage;
    public Text playerNumberText;
    public Text playerPointsText;
    public Text playerKillsText;
    public Text aiKillsText;
    public Text roundWinsText;
    public GameObject[] achievementParents;
    public Text[] achievementTexts;
    public Image[] achievementSprites;

    public void Initialise (PlayerMatchStats playerMatchStats)
    {
        panelBackgroundImage.color = GameManager.instance.playerColours[playerMatchStats.playerNumber - 1];

        playerNumberText.text = "Player " + playerMatchStats.playerNumber.ToString();
        playerPointsText.text = "Total Points: " + playerMatchStats.points;
        playerKillsText.text = "Total Player Kills: " + playerMatchStats.playerKills;
        aiKillsText.text = "Total AI Kills: " + playerMatchStats.playerKills;
        roundWinsText.text = "Total Round Wins: " + playerMatchStats.roundWins + playerMatchStats.extractions;

        List <Achievements> achievements = GameManager.instance.achievementChecker.GetAchievements(playerMatchStats.playerNumber);

        foreach (var item in achievementParents)
        {
            item.SetActive(false);
        }        

        if (achievements == null)
        {
            return;
        }

        if (achievements.Count >= achievementParents.Length)
        {
            for (int i = 0; i < achievementParents.Length; i++)
            {
                int randNum = Random.Range(0, achievements.Count);
                achievementParents[i].SetActive(true);
                achievementTexts[i].text = achievements[randNum].achievementName;
                achievementSprites[i].sprite = achievements[randNum].sprite;
                achievements.RemoveAt(randNum);
            }
        }
        else
        {
            for (int i = 0; i < achievements.Count; i++)
            {
                int randNum = Random.Range(0, achievements.Count);
                achievementParents[i].SetActive(true);
                achievementTexts[i].text = achievements[randNum].achievementName;
                achievementSprites[i].sprite = achievements[randNum].sprite;
                achievements.RemoveAt(randNum);
            }
        }
    }
}
