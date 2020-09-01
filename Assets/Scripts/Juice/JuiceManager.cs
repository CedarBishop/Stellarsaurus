using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JuiceManager : MonoBehaviour
{
    static bool timeIsSlowed;
    static float timer;
    static float time;
    static Volume volume;
    static float volumeValue;

    void Start()
    {
        timeIsSlowed = false;
    }


    void Update()
    {
        if (timeIsSlowed)
        {
            if (timer <= 0)
            {
                StopSleep();
            }
            else
            {
                timer -= Time.unscaledDeltaTime;
            }
        }
    }


    public static void TimeSleep (float realTimeDuration, float timescale)
    {
        timeIsSlowed = true;
        Time.timeScale = timescale;
        timer = realTimeDuration;
        time = realTimeDuration * timescale;
        volume = FindObjectOfType<Volume>();
    }

    static void StopSleep ()
    {
        timeIsSlowed = false;
        Time.timeScale = 1.0f;
    }

}


[System.Serializable]
public class JuiceParams
{
    
}

