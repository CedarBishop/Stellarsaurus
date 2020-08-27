using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementChecker : MonoBehaviour
{
    public Achievements[] achievementObjects;
    public List<Achievements> GetAchievements (int playerNumber)
    {
        List<Achievements> achievements = new List<Achievements>();

        if (GameManager.instance.SelectedGamemode != null)
        {
            PlayerMatchStats thisPlayerStats;
            foreach (var stat in GameManager.instance.SelectedGamemode.playerMatchStats)
            {
                if (stat.playerNumber == playerNumber)
                {
                    thisPlayerStats = stat;

                    foreach (var item in achievementObjects)
                    {
                        Achievements achievement = item;
                        switch (achievement.stat)
                        {
                            case StatTypes.Jumps:

                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.jumps))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.WeaponsPickedUp:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.weaponsPickedUp))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.Deaths:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.deaths))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.ObstaclesHit:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.obstaclesHit))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.DamageDealt:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.damageDealt))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.BulletsFired:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.bulletsFired))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.HealthRegained:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.totalHealthRegained))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.Suicides:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.suicides))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.ExplosivesUsed:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.explosivesUsed))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.FlamesShot:
                                if (CheckAchievementLevel(ref achievement, thisPlayerStats.flamesShot))
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }          
            
        }
        return achievements;
    }


    bool CheckAchievementLevel (ref Achievements achievement, int amount)
    {
        if (amount >= achievement.bronzeAmountToGainAchievement)
        {
            achievement.achievementLevel = AchievementLevel.Bronze;
            if (amount >= achievement.silverAmountToGainAchievement)
            {
                achievement.achievementLevel = AchievementLevel.Silver;
                if (amount >= achievement.goldAmountToGainAchievement)
                {
                    achievement.achievementLevel = AchievementLevel.Gold;
                }
            }
            return true;
        }

        return false;
    }
}
