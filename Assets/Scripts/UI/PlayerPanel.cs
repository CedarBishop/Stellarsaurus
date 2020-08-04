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
    public Text accuracyText;
    public Text roundWinsText;
    public Sprite bronzeBorder;
    public Sprite silverSprite;
    public Sprite goldSprite;
    public Color[] achievementLevelColours;
    public GameObject[] achievementParents;
    public Text[] achievementTexts;
    public Image[] achievementBorders;
    public Image[] achievementSprites;
    public float timeBeforeMovingToNextSlide;


    private List<Achievements> achievements;
    private int[] indexes;

    public void Initialise (GameMode gamemode,PlayerMatchStats playerMatchStats)
    {
        panelBackgroundImage.color = GameManager.instance.playerColours[playerMatchStats.playerNumber - 1];

        playerNumberText.text = "Player " + playerMatchStats.playerNumber.ToString();

        switch (gamemode)
        {
            case GameMode.FreeForAll:
                playerPointsText.text = "Total Round Wins: " + playerMatchStats.roundWins;
                roundWinsText.text = "";
                break;
            case GameMode.Elimination:
                break;
            case GameMode.Extraction:
                playerPointsText.text = "Total Points: " + playerMatchStats.points;
                roundWinsText.text = "Total Round Wins: " + playerMatchStats.roundWins + playerMatchStats.extractions;
                break;
            case GameMode.Climb:
                break;
            default:
                break;
        }

        playerKillsText.text = "Total Player Kills: " + playerMatchStats.playerKills;
        aiKillsText.text = "Total AI Kills: " + playerMatchStats.aiKills;

        if (playerMatchStats.bulletsFired == 0)
        {
            accuracyText.text = "Accuracy: N/A";
        }
        else
        {
            accuracyText.text = "Accuracy: " + ((playerMatchStats.bulletsHit / playerMatchStats.bulletsFired) * 100).ToString() + "%";
        }
        

        achievements = GameManager.instance.achievementChecker.GetAchievements(playerMatchStats.playerNumber);

        IListExtensions.Shuffle<Achievements>(achievements);

        indexes = new int[achievementParents.Length];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = i;
        }

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
                achievementParents[i].SetActive(true);
                achievementTexts[i].text = achievements[i].achievementName;
                achievementSprites[i].sprite = achievements[i].sprite;

                SetBorder(i, i);
            }
            StartCoroutine("NextSlide");
        }
        else
        {
            for (int i = 0; i < achievements.Count; i++)
            {
                achievementParents[i].SetActive(true);
                achievementTexts[i].text = achievements[i].achievementName;
                achievementSprites[i].sprite = achievements[i].sprite;

                SetBorder(i, i);
            }
        }
    }

    void SetBorder(int borderIndex, int achievementIndex)
    {
        switch (achievements[achievementIndex].achievementLevel)
        {
            case AchievementLevel.None:
                achievementBorders[borderIndex].sprite = null;
                break;
            case AchievementLevel.Bronze:
                achievementBorders[borderIndex].color = achievementLevelColours[0];
                break;
            case AchievementLevel.Silver:
                achievementBorders[borderIndex].color = achievementLevelColours[1];
                break;
            case AchievementLevel.Gold:
                achievementBorders[borderIndex].color = achievementLevelColours[2];
                break;
            default:
                break;
        }
    }

    IEnumerator NextSlide ()
    {
        while (UIManager.instance.CurrentUIState == UIState.MatchEnd)
        {
            yield return new WaitForSeconds(timeBeforeMovingToNextSlide);
            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i]++;
                indexes[i] %= achievements.Count;
                achievementTexts[i].text = achievements[indexes[i]].achievementName;
                achievementSprites[i].sprite = achievements[indexes[i]].sprite;
                SetBorder(i, indexes[i]);
            }
        }
    }
}


public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
