using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionObjective : MonoBehaviour
{
    [Range(1.0f,60.0f)] public float timeRequiredToCharge;
    [Range(0.0f,1.0f)] public float chargeDownScaler;

    private SpriteRenderer spriteRenderer;


    private float timer;
    private bool chargeCompleted;
    private bool isHeld;
    private int playerNumber;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (chargeCompleted == false)
        {
            if (isHeld)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer -= (Time.deltaTime * chargeDownScaler);
            }

            if (timer >= timeRequiredToCharge)
            {
                chargeCompleted = true;
                OnChargeComplete();
            }
        }
    }

    public void OnPickup (int num)
    {
        isHeld = true;
        spriteRenderer.enabled = false;
        playerNumber = num;
    }

    public void OnDrop ()
    {
        isHeld = false;
        spriteRenderer.enabled = true;
    }



    void OnChargeComplete ()
    {
        GameManager.instance.SelectedGamemode.PlayerWonRound(playerNumber);
    }
}
