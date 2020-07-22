using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    public Text text;
    public float movementSpeed;
    public float timeToLive;
    public float scaleSpeed;

    private float timer;
    private bool initialised;

    public void Init (int score)
    {
        text.text = "+" + score.ToString();
        timer = timeToLive;
        initialised = true;
        if (score <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (initialised)
        {
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                timer -= Time.fixedDeltaTime;
                transform.position += Vector3.up * movementSpeed * Time.fixedDeltaTime;
                transform.localScale += (new Vector3(scaleSpeed,scaleSpeed,scaleSpeed) * Time.fixedDeltaTime);
            }
        }
    }
}
