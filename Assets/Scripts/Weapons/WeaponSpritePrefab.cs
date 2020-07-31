using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponSpritePrefab : MonoBehaviour
{
    public Sprite weaponSprite;
    public BoxCollider2D collider;
    public Transform firingPoint;
    public Material weaponSpriteMaterial;
    public Light2D light;
    public Transform lightTransform;
    public GameObject particle;
    public Transform particleTransform;
    public RuntimeAnimatorController animatorController;
    public string idleAnimName;
    public string chargeAnimName;
    public string attackAnimName;


    private void OnValidate()
    {
        weaponSprite = GetComponent<SpriteRenderer>().sprite;
        collider = GetComponent<BoxCollider2D>();

        if (transform.childCount == 0)
        {
            firingPoint = transform;
        }
        else
        {
            firingPoint = transform.GetChild(0);
        }
    }

}
