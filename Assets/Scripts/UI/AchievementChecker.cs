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

                    foreach (var achievement in achievementObjects)
                    {
                        switch (achievement.stat)
                        {
                            case StatTypes.Jumps:
                                if (thisPlayerStats.jumps >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.WeaponsPickedUp:
                                if (thisPlayerStats.weaponsPickedUp >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.Deaths:
                                if (thisPlayerStats.deaths >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.ObstaclesHit:
                                if (thisPlayerStats.obstaclesHit >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.DamageDealt:
                                if (thisPlayerStats.damageDealt >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.BulletsFired:
                                if (thisPlayerStats.bulletsFired >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.HealthRegained:
                                if (thisPlayerStats.totalHealthRegained >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.Suicides:
                                if (thisPlayerStats.suicides >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.ExplosivesUsed:
                                if (thisPlayerStats.explosivesUsed >= achievement.amountToGainAchievement)
                                {
                                    achievements.Add(achievement);
                                }
                                break;
                            case StatTypes.FlamesShot:
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

}
