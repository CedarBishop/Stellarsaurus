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
    public float timeBeforeMovingToNextSlide;


    private List<Achievements> achievements;
    private int[] indexes;

    public void Initialise (PlayerMatchStats playerMatchStats)
    {
        panelBackgroundImage.color = GameManager.instance.playerColours[playerMatchStats.playerNumber - 1];

        playerNumberText.text = "Player " + playerMatchStats.playerNumber.ToString();
        playerPointsText.text = "Total Points: " + playerMatchStats.points;
        playerKillsText.text = "Total Player Kills: " + playerMatchStats.playerKills;
        aiKillsText.text = "Total AI Kills: " + playerMatchStats.playerKills;
        roundWinsText.text = "Total Round Wins: " + playerMatchStats.roundWins + playerMatchStats.extractions;

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
            }
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
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
