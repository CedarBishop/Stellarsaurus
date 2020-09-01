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
                StopSleep();
            }
            else
            {
                timer -= Time.unscaledDeltaTime;
            }
        }
    }


    public static void TimeSleep (float realTimeDuration, float timescale, bool activateSlowMoTint)
    {
        timeIsSlowed = true;
        Time.timeScale = timescale;
        timer = realTimeDuration;
        time = realTimeDuration * timescale;
        if (activateSlowMoTint)
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.slowMoTint.gameObject.SetActive(true);
            }
        }
    }

    static void StopSleep ()
    {
        timeIsSlowed = false;
        Time.timeScale = 1.0f;
        if (UIManager.instance != null)
        {
            UIManager.instance.slowMoTint.gameObject.SetActive(false);
        }
    }

}


[System.Serializable]
public class JuiceParams
{
    
}

