using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionObjective : MonoBehaviour
{
    [Range(1.0f,60.0f)] public float timeRequiredToCharge;
    [Range(0.0f,1.0f)] public float chargeDownScaler;

    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;

    private float timer;
    private bool chargeCompleted;
    private bool isHeld;
    private int playerNumber;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
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

    public Sprite OnPickup (int num)
    {
        isHeld = true;
        Sprite sprite = spriteRenderer.sprite;
        playerNumber = num;

        spriteRenderer.enabled = false;
        collider.enabled = false;

        transform.position = new Vector3(1000,1000,0);

        return sprite;
    }

    public void OnDrop (Vector3 newPos)
    {
        isHeld = false;
        transform.position = newPos;
        spriteRenderer.enabled = true;
        collider.enabled = true;
    }



    void OnChargeComplete ()
    {
        GameManager.instance.SelectedGamemode.PlayerWonRound(playerNumber);
    }
}
