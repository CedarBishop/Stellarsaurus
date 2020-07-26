using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Achievements : ScriptableObject
{
    public string achievementName;
    public Sprite sprite;
    public StatTypes stat;
    public int amountToGainAchievement;
}
