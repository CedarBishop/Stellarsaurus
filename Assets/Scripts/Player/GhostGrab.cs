using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGrab : MonoBehaviour
{
    [HideInInspector] public int playerNumber;

    public Transform objectOriginTransform;
    public SpriteRenderer objectSprite;

    [HideInInspector] public Player player;
    [HideInInspector] public AimType aimType;
    [HideInInspector] public bool isGamepad;

    private GameObject heldObject;
    private Camera mainCamera;
    private GhostMovement ghostMovement;
    private CameraShake cameraShake;

    bool isAimingRightstick;



    void Start()
    {
        mainCamera = Camera.main;
        ghostMovement = GetComponent<GhostMovement>();
        cameraShake = mainCamera.GetComponent<CameraShake>();
    }

    private void Update()
    {
        switch (aimType)
        {
            case AimType.FreeAim:
                if (isGamepad == false)
                {
                    Vector2 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    objectOriginTransform.right = direction;
                }
                break;
            case AimType.EightDirection:
                if (isGamepad == false)
                {
                    Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
                    objectOriginTransform.right = TranslateToEightDirection(directionToTarget.normalized);
                }
                break;
            case AimType.FourDirection:

                //switch (playerMovement.GetDirection())
                //{
                //    case Orthogonal.Up:
                //        gunOriginTransform.right = Vector2.up;
                //        gunSprite.flipY = false;
                //        break;
                //    case Orthogonal.Right:
                //        gunOriginTransform.right = Vector2.right;
                //        gunSprite.flipY = false;
                //        break;
                //    case Orthogonal.Down:
                //        gunOriginTransform.right = Vector2.down;
                //        gunSprite.flipY = true;
                //        break;
                //    case Orthogonal.Left:
                //        gunOriginTransform.right = new Vector2(-1, 0.01f);
                //        gunSprite.flipY = true;
                //        break;
                //    default:
                //        break;
                //}
                break;
            case AimType.HybridEightDirection:
                //if (isGamepad == false)
                //{
                //    Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //    Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
                //    gunOriginTransform.right = TranslateToEightDirection(directionToTarget.normalized);
                //}
                break;

            default:
                break;
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
        switch (aimType)
        {
            case AimType.FreeAim:
                if (isGamepad)
                {
                    if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                    {
                        objectOriginTransform.right = v;
                    }
                }
                break;
            case AimType.EightDirection:
                if (isGamepad)
                {
                    if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                    {
                        objectOriginTransform.right = TranslateToEightDirection(v);
                    }
                }
                break;
            case AimType.FourDirection:
                break;

            case AimType.HybridEightDirection:

                if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                {
                    objectOriginTransform.right = TranslateToEightDirection(v);
                }

                break;
            default:
                break;
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
                objectSprite.flipY = false;
            }
            else
            {
                result = Vector2.down;
                objectSprite.flipY = true;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) < 0.25f)
        {
            // Left & Right
            if (v.x > 0)
            {
                result = Vector2.right;
                objectSprite.flipY = false;
            }
            else
            {
                result = new Vector2(-1, 0.01f);
                objectSprite.flipY = true;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Diagonals
            if (v.x < -0.25f && v.y < -0.25f)
            {
                // down left
                result = new Vector2(-1, -1);
                objectSprite.flipY = true;
            }
            else if (v.x > 0.25f && v.y < -0.25f)
            {
                // down right
                result = new Vector2(1, -1);
                objectSprite.flipY = false;
            }
            else if (v.x > 0.25f && v.y > 0.25f)
            {
                // up right
                result = new Vector2(1, 1);
                objectSprite.flipY = false;
            }
            else if (v.x < -0.25f && v.y > 0.25f)
            {
                // up left
                result = new Vector2(-1, 1);
                objectSprite.flipY = true;
            }

        }
        else
        {
            result = transform.right;
        }


        return result;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

    }

    public void SetAimType(AimType _AimType)
    {
        aimType = _AimType;
    }

    public void Grab ()
    {
        if (heldObject == null)
        {
            Drop();
            return;
        }
    }

    public void Drop ()
    {


        heldObject = null;
    }
}
