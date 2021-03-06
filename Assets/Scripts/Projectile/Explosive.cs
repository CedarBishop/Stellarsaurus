﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Explosive : Projectile
{
    public bool damagesTileMap;
    protected float timeTillExplode = 3;
    protected float explosionSize = 2;
    public ParticleSystem explodeParticle;
    protected CameraShake cameraShake;
    protected float duration;
    protected float magnitude;
    protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected string sfxName;

    IEnumerator CoExplode ()
    {
        yield return new WaitForSeconds(timeTillExplode);
        Explode();
    }

    protected void Explode ()
    {
        cameraShake.StartShake(duration, magnitude);
        if (explodeParticle != null)
        {
            GameObject go = Instantiate(explodeParticle, transform.position, Quaternion.identity).gameObject;
            Destroy(go, 2);
        }
            
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionSize);
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<PlayerHealth>())
                {
                    colliders[i].GetComponent<PlayerHealth>().HitByPlayer(playerNumber, true);

                    if (GameManager.instance.SelectedGamemode != null)
                    {
                        GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.DamageDealt, damage);
                    }

                    OnHit();
                }
                else if (colliders[i].GetComponent<Dinosaur>())
                {
                    colliders[i].GetComponent<Dinosaur>().TakeDamage(playerNumber, damage);

                    if (GameManager.instance.SelectedGamemode != null)
                    {
                        GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.DamageDealt, damage);
                    }
                    OnHit();
                }
                else if (colliders[i].GetComponent<EnvironmentalObjectHealth>())
                {
                    colliders[i].GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, playerNumber);
                }
                else if (colliders[i].GetComponent<Window>())
                {
                    colliders[i].GetComponent<Window>().UpdateHealth(damage);
                }
                else if (colliders[i].GetComponent<TilemapCollider2D>())
                {
                    if (damagesTileMap)
                    {
                        Tilemap tilemap = colliders[i].GetComponent<Tilemap>();
                        TileBase tile = tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z)));
                        Vector3Int upPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1), Mathf.FloorToInt(transform.position.z));
                        Vector3Int downPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y - 1), Mathf.FloorToInt(transform.position.z));
                        Vector3Int leftPos = new Vector3Int(Mathf.FloorToInt(transform.position.x - 1), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
                        Vector3Int rightPos = new Vector3Int(Mathf.FloorToInt(transform.position.x + 1), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
                        tilemap.SetTile(upPos, null);
                        tilemap.SetTile(downPos, null);
                        tilemap.SetTile(leftPos, null);
                        tilemap.SetTile(rightPos, null);
                    }

                }
            }
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX(sfxName);
        }

        Destroy(gameObject);
    }


    public virtual void InitExplosive (float explodeTime, float explodeSize, int _Damage, int _PlayerNumber, float force, string explosionSFXName,float cameraShakeDuration, float cameraShakeMagnitude, float cookTime = 0)
    {
        animator = GetComponent<Animator>();

        if (animator)
        {
            if (cookTime >= explodeTime)
            {
                animator.Play("Grenade", 0, 0.99f);
            }
            else
            {
                float cookPercent = cookTime / explodeTime;
                print(cookPercent);
                animator.Play("Grenade", 0, cookPercent);
            }
        }
        
        
        timeTillExplode = (cookTime >= explodeTime) ? 0.01f : explodeTime - cookTime;
        print(timeTillExplode);
        explosionSize = explodeSize;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        sfxName = explosionSFXName;
        initialForce = force;
        duration = cameraShakeDuration;
        magnitude = cameraShakeMagnitude;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);

        OnSpawn();

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.ExplosivesUsed, 1);
        }

        if (damagesOnHit == false )
        {
            StartCoroutine("CoExplode");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerHealth>().playerNumber == playerNumber)
            {
                return;
            }
        }

        if (damagesOnHit)
        {
            print("It damages on hit");
            Explode();
        }
    }
}


public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}