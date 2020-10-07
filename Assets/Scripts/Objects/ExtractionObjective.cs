using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionObjective : MonoBehaviour
{
    [Range(1.0f,60.0f)] public float timeRequiredToCharge;
    [Range(0.0f,1.0f)] public float chargeDownScaler;

    public GameObject chargedParticle;

    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;
    private Animator animator;
    private PlayerShoot playerShoot;

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
                animator.SetFloat("AnimSpeed", timer * 0.1f);
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

        if (transform.position.y < -30)
        {
            transform.position = LevelManager.instance.extractionObjectSpawnPosition.position;
        }
         
    }

    public void OnPickup (int num, PlayerShoot player)
    {
        isHeld = true;
        playerNumber = num;

        playerShoot = player;

        collider.enabled = false;

        if (rigidbody != null)
        {
            Destroy(rigidbody);
        }
        transform.position = player.gunParentTransform.position;
        transform.parent = player.gunParentTransform;

    }

    public void OnDrop ()
    {
        isHeld = false;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        playerShoot = null;
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
        rigidbody.velocity = Vector2.zero;
        transform.parent = null;
    }

    void OnChargeComplete ()
    {
        if (chargedParticle != null)
        {
            GameObject go = Instantiate(chargedParticle,transform.position, Quaternion.identity);
            ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.startColor = GameManager.instance.playerColours[playerNumber - 1];
                particle.Play();
            }
        }
        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AwardExtraction(playerNumber);
            ScorePopup scorePopup = Instantiate(LevelManager.instance.scorePopupPrefab, playerShoot.gunOriginTransform.transform.position, Quaternion.identity);
            scorePopup.Init(GameManager.instance.SelectedGamemode.extractionPointReward);
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_Extraction");
        }
    }

    public void Grab(Transform holderTransform)
    {
        transform.parent = holderTransform;
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.gravityScale = 0;
        }
    }

    public void Drop()
    {
        transform.parent = null;
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.gravityScale = 0;
        }
    }
}