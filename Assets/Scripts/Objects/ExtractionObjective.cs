﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionObjective : MonoBehaviour
{
    [Range(1.0f,60.0f)] public float timeRequiredToCharge;
    [Range(0.0f,1.0f)] public float chargeDownScaler;

    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;
    private Animator animator;
    private Animator weaponSpriteAnimator;

    private float timer;
    private bool chargeCompleted;
    private bool isHeld;
    private int playerNumber;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("AnimSpeed",0.1f);
    }

    void Update()
    {
        if (chargeCompleted == false)
        {
            if (isHeld)
            {
                timer += Time.deltaTime;
                weaponSpriteAnimator.SetFloat("AnimSpeed", timer * 0.1f);
            }
            else
            {
                if (timer <= 0)
                {
                    timer = 0;
                }
                timer -= (Time.deltaTime * chargeDownScaler);
                animator.SetFloat("AnimSpeed", timer * 0.1f);
            }

            if (timer >= timeRequiredToCharge)
            {
                chargeCompleted = true;
                OnChargeComplete();
            }
        }

        if (transform.position.y < -20)
        {
            transform.position = LevelManager.instance.extractionObjectSpawnPosition.position;
        }
         
    }

    public void OnPickup (int num, Animator playerWeaponAnimator)
    {
        weaponSpriteAnimator = playerWeaponAnimator;
        isHeld = true;
        playerNumber = num;

        spriteRenderer.enabled = false;
        collider.enabled = false;

        transform.position = new Vector3(1000,1000,0);

    }

    public void OnDrop (Vector3 newPos)
    {
        rigidbody.velocity = Vector2.zero;
        weaponSpriteAnimator = null;
        isHeld = false;
        transform.position = newPos;
        spriteRenderer.enabled = true;
        collider.enabled = true;
    }



    void OnChargeComplete ()
    {
        GameManager.instance.SelectedGamemode.AwardExtraction(playerNumber);
    }
}
