﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public ParticleSystem destructionParticles;
    public GameObject muzzleFlash;
    public TrailRenderer trailRenderer;
    public Rigidbody2D bulletShell;
    protected Rigidbody2D rigidbody;

    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>())
        {
            if (collision.GetComponent<Projectile>().playerNumber == playerNumber)
            {
                return;
            }
        }
        else if (collision.GetComponent<Teleport>())
        {
            print("Going through teleporter");
            if (trailRenderer != null)
            {
                isActive = !isActive;
                trailRenderer.gameObject.SetActive(isActive);
            }
        }
        else if (collision.GetComponent<AutomaticDoor>())
        {
            if (collision.GetComponent<AutomaticDoor>().isOpen)
            {
                return;
            }
        }

        if (damagesOnHit)
        {
            // Player Damage
            if (collision.GetComponent<PlayerHealth>())
            {
                HitPlayer(collision.GetComponent<PlayerHealth>());
            }
            // AI Damage
            else if (collision.GetComponent<Dinosaur>())
            {
                HitAI(collision.GetComponent<Dinosaur>());
            }
            // Environmental Object Damage
            else if (collision.GetComponent<EnvironmentalObjectHealth>())
            {
                collision.GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, playerNumber);
                Debug.Log("Bullet found Enviro Obj");
            }

            // All these checks are to ensure that hittiing a decorative environmental object doesn't add any stats to the player's achievements
            if (collision.GetComponent<EnvironmentalObjectHealth>() == null)
            {
                OnHit();
            }
        }
        if (destroysOnHit)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                if (GameManager.instance.SelectedGamemode.noFriendlyFire)
                {
                    return;
                }
            }
            if (collision.GetComponent<Teleport>())
            {
                return;
            }
            // Check to see if it hit a decorative environmental object, and if so, not delete the bullet.
            if (collision.GetComponent<EnvironmentalObjectHealth>())
            {
                if (collision.GetComponent<EnvironmentalObjectHealth>().isDecorative)
                {
                    return;
                }
            }
            if (destructionParticles != null)
            {
                ParticleSystem p = Instantiate(destructionParticles, transform.position,Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 1.0f);
            }
            Destroy(gameObject);
        }
    }

    public virtual void InitialiseProjectile (float Range, int _Damage, int _PlayerNumber, float force, float Spread, bool spawnBulletShell, float upForce = 0)
    {
        startingPosition = new Vector2(transform.position.x, transform.position.y);
        range = Range;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        initialForce = force;
        damagesOnHit = true;
        destroysOnHit = true;
        spreadRange = Spread;

        float zRotation = transform.rotation.eulerAngles.z + Random.Range(-spreadRange, spreadRange);
        transform.rotation = Quaternion.Euler(0, 0, zRotation);


        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
        rigidbody.AddForce(transform.up * upForce);
        StartCoroutine("DestroySelf");
        StartCoroutine("DistanceTracker");

        if (muzzleFlash != null)
        {
            GameObject g = Instantiate(muzzleFlash, transform.position, Quaternion.identity);
            Destroy(g,0.05f);
        }

        if (spawnBulletShell)
        {
            if (bulletShell != null)
            {
                Vector3 position = transform.position + (0.5f * (transform.right * -1));
                Rigidbody2D shell = Instantiate(bulletShell, position, transform.rotation);

                shell.AddForce((Vector3.up + (transform.right * -1)) * Random.Range(80.0f, 200.0f));
                shell.AddTorque(Random.Range(0.0f, 0.5f), ForceMode2D.Impulse);
            }
        }

        OnSpawn();
    }

    IEnumerator DistanceTracker()
    {
        while (Vector2.Distance(startingPosition, new Vector2(transform.position.x, transform.position.y)) < range)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StopCoroutine("DestroySelf");
        Destroy(gameObject);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    protected virtual void HitPlayer(PlayerHealth playerHealth)
    {
        playerHealth.HitByPlayer(playerNumber);

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.DamageDealt, damage);
        }
    }

    protected virtual void HitAI (Dinosaur ai)
    {
        ai.TakeDamage(playerNumber,damage);

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.DamageDealt, damage);
        }
    }
}
