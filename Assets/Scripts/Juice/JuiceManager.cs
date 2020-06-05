using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    static bool timeIsSlowed;
    static float timer;
    static float time;

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
                timeIsSlowed = false;
                Time.timeScale = 1.0f;
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
        
    }

  

}
