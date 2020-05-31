using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpritePrefab : MonoBehaviour
{
    public Sprite weaponSprite;
    public BoxCollider2D collider;
    public Transform firingPoint;


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
