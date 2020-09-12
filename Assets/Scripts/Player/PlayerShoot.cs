using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform gunOriginTransform;
    public SpriteRenderer gunSprite;

    [HideInInspector] public bool isGamepad;
    [HideInInspector] public int playerNumber;
    public Weapon currentWeapon;
    [HideInInspector] public Player player;

    private Camera mainCamera;
    private CameraShake cameraShake;
    private PlayerMovement playerMovement;
    private Gamepad gamepad;
    private bool isAimingRightstick;


    private Weapon triggeredWeapon;
    private bool isTriggeringWeapon;
    private bool isTriggeringExtractionObjective;

    private ExtractionObjective triggeredExtractionObjective;
    private ExtractionObjective extractionObjective;

    private bool isHoldingFireButton;
    private bool semiLimiter;

    private float cookTime;
    private bool shootOnRelease;

    // Firing Type Variables
    private bool isFiring;
    private float chargeUpTimer;
    private AudioSourceController chargeUpWeaponAudioController;
    private AudioSourceController chargeDownWeaponAudioController;
    private string chargeUpSound;
    private string chargeDownSound;
    private float chargeUpTime;
    private float explosionTime;

    private float cameraShakeDuration;
    private float cameraShakeMagnitude;


    void Start()
    {
        mainCamera = Camera.main;
        cameraShake = mainCamera.GetComponent<CameraShake>();
        currentWeapon = null;
        playerMovement = GetComponent<PlayerMovement>();
        gamepad = Gamepad.current;
    }

    private void OnDestroy()
    {
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
    }

    void Update()
    {
        if (isHoldingFireButton)
        {
            if (currentWeapon != null)
            {
                switch (currentWeapon.fireType)
                {
                    case FireType.SemiAutomatic:

                        if (semiLimiter)
                        {
                            semiLimiter = false;
                            Shoot();
                        }

                        break;
                    case FireType.Automatic:
                        Shoot();
                        break;
                    case FireType.ChargeUp:
                        if (semiLimiter)
                        {
                            if (chargeUpTimer >= chargeUpTime)
                            {
                                semiLimiter = false;
                                Shoot();
                            }
                            else
                            {
                                if (chargeUpWeaponAudioController == null)
                                {
                                    StartChargeUpSound();
                                }
                                chargeUpTimer += Time.deltaTime;
                            }
                        }
                        break;
                    case FireType.WindUp:

                        if (chargeUpTimer >= chargeUpTime)
                        {
                            Shoot();
                            if (isFiring == false)
                            {
                                isFiring = true;
                            }
                        }
                        else
                        {
                            if (chargeUpWeaponAudioController == null)
                            {
                                StartChargeUpSound();
                            }
                            chargeUpTimer += Time.deltaTime;
                        }
                        break;
                    case FireType.Cook:

                        if (semiLimiter)
                        {
                            if (cookTime >= explosionTime)
                            {
                                semiLimiter = false;
                                Shoot();
                            }
                            else
                            {
                                // Play Charge Animation here

                                cookTime += Time.deltaTime;
                                shootOnRelease = true;
                            }

                        }
                        break;
                    default:
                        break;


                }

                StopChargeDownSound();
            }

        }
        else
        {
            semiLimiter = true;
            chargeUpTimer = 0;
            StopChargeUpSound();
            isFiring = false;

            if (shootOnRelease)
            {
                Shoot();
                shootOnRelease = false;
            }

            cookTime = 0;
        }

    }

    private void LateUpdate()
    {
        isAimingRightstick = false;
    }

    public void Aim(Vector2 v, bool isRightstick = false)
    {
        if (isAimingRightstick && isRightstick == false)
        {
            return;
        }
        isAimingRightstick = isRightstick;

        if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
        {
            gunOriginTransform.right = TranslateToEightDirection(v);
        }
    }

    Vector2 TranslateToEightDirection(Vector2 v)
    {
        Vector2 result = v;

        if (Mathf.Abs(v.x) < 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Up & Down
            if (v.y > 0)
            {
                result = Vector2.up;
                gunSprite.flipY = false;
            }
            else
            {
                result = Vector2.down;
                gunSprite.flipY = true;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) < 0.25f)
        {
            // Left & Right
            if (v.x > 0)
            {
                result = Vector2.right;
                gunSprite.flipY = false;
            }
            else
            {
                result = new Vector2(-1, 0.01f);
                gunSprite.flipY = true;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Diagonals
            if (v.x < -0.25f && v.y < -0.25f)
            {
                // down left
                result = new Vector2(-1, -1);
                gunSprite.flipY = true;
            }
            else if (v.x > 0.25f && v.y < -0.25f)
            {
                // down right
                result = new Vector2(1, -1);
                gunSprite.flipY = false;
            }
            else if (v.x > 0.25f && v.y > 0.25f)
            {
                // up right
                result = new Vector2(1, 1);
                gunSprite.flipY = false;
            }
            else if (v.x < -0.25f && v.y > 0.25f)
            {
                // up left
                result = new Vector2(-1, 1);
                gunSprite.flipY = true;
            }

        }
        else
        {
            result = transform.right;
        }


        return result;
    }


    public void StartFire()
    {
        isHoldingFireButton = true;
    }

    public void EndFire()
    {
        isHoldingFireButton = false;
    }

    private void Shoot()
    {
        if (currentWeapon == null)
        {
            return;
        }

        if (currentWeapon.Shoot(playerNumber))
        {
            StartCoroutine("Haptic");
            StartCoroutine("WeaponJitter");
            CameraShake();
        }
    }

    IEnumerator Haptic()
    {
        gamepad.SetMotorSpeeds(0.5f, 1.0f);
        gamepad.ResumeHaptics();
        yield return new WaitForSeconds(0.1f);
        gamepad.PauseHaptics();
        gamepad.ResetHaptics();
    }

    IEnumerator WeaponJitter()
    {
        Vector3 originalPosition = gunSprite.transform.localPosition;
        gunSprite.transform.localPosition += (gunSprite.transform.right * -1) * currentWeapon.jitter;
        yield return new WaitForSeconds(0.03f);
        gunSprite.transform.localPosition = originalPosition;
    }

    void CameraShake()
    {
        if (cameraShake != null)
            cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
    }

    public void Grab()
    {
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
        else if (currentWeapon != null)
        {
            DropWeapon();
        }
        else if (isTriggeringExtractionObjective)
        {
            PickupExtractionObject();
        }
        else if (isTriggeringWeapon)
        {
            if (triggeredWeapon != null)
            {
                currentWeapon = triggeredWeapon;
                OnWeaponPickup();
                

                isTriggeringWeapon = false;
                triggeredWeapon = null;
            }
        }
    }

    public void Grab(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
        currentWeapon = weapon;
        OnWeaponPickup();


    }


    void OnWeaponPickup()
    {
        if (currentWeapon == null)
        {
            return;
        }
        currentWeapon.Pickup(gunOriginTransform);

        cameraShakeDuration = currentWeapon.cameraShakeDuration;
        cameraShakeMagnitude = currentWeapon.cameraShakeMagnitude;

        switch (currentWeapon.fireType)
        {
            case FireType.SemiAutomatic:
                break;
            case FireType.Automatic:
                break;
            case FireType.ChargeUp:
                chargeUpTime = currentWeapon.chargeUpTime;
                chargeUpSound = currentWeapon.chargeUpSound;
                chargeDownSound = currentWeapon.chargeDownSound;
                break;
            case FireType.WindUp:
                chargeUpTime = currentWeapon.chargeUpTime;
                chargeUpSound = currentWeapon.chargeUpSound;
                chargeDownSound = currentWeapon.chargeDownSound;
                break;
            case FireType.Cook:
                explosionTime = currentWeapon.explosionTime;
                break;
            default:
                break;
        }




        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_WeaponPickup");
        }
    }

    public void DropWeapon()
    {
        if (currentWeapon.AimCheck())
        {
            return;
        }

        currentWeapon.Drop();
        currentWeapon = null;
    }

    public void DropExtractionObject()
    {
        extractionObjective.OnDrop(gunSprite.transform.position);
        playerMovement.SetIsHoldingExtractionObject(false);
        extractionObjective = null;
    }

    void PickupExtractionObject()
    {
        playerMovement.SetIsHoldingExtractionObject(true);
        extractionObjective = triggeredExtractionObjective;
        extractionObjective.OnPickup(playerNumber, this);

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_WeaponPickup");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            triggeredWeapon = other.GetComponentInParent<Weapon>();
            isTriggeringWeapon = true;
        }
        else
        {
            isTriggeringWeapon = false;
            triggeredWeapon = null;
        }
        if (other.CompareTag("Extraction"))
        {
            triggeredExtractionObjective = other.GetComponentInParent<ExtractionObjective>();
            isTriggeringExtractionObjective = true;
        }
        else
        {
            isTriggeringExtractionObjective = false;
            triggeredExtractionObjective = null;
        }
    }

    public void Disarm()
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }
    }

    void StartChargeUpSound()
    {
        if (SoundManager.instance != null)
        {
            chargeUpWeaponAudioController = SoundManager.instance.PlaySFX(chargeUpSound);
        }
    }

    void StopChargeUpSound()
    {
        if (chargeUpWeaponAudioController != null)
        {
            chargeUpWeaponAudioController.StopPlaying();
            chargeUpWeaponAudioController = null;
            StartChargeDownSound();
        }
    }

    void StartChargeDownSound()
    {
        if (SoundManager.instance != null)
        {
            chargeDownWeaponAudioController = SoundManager.instance.PlaySFX(chargeDownSound);
        }
    }

    void StopChargeDownSound()
    {
        if (chargeDownWeaponAudioController != null)
        {
            chargeDownWeaponAudioController.StopPlaying();
            chargeDownWeaponAudioController = null;
        }
    }
}
