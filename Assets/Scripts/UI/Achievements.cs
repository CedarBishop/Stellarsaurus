using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementLevel { None, Bronze, Silver, Gold}

[CreateAssetMenu]
public class Achievements : ScriptableObject
{
    public string achievementName;
    public Sprite sprite;
    public StatTypes stat;
    public AchievementLevel achievementLevel;
    public int bronzeAmountToGainAchievement;
    public int silverAmountToGainAchievement;
    public int goldAmountToGainAchievement;
}
