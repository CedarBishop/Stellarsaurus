using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpritePrefab : MonoBehaviour
{
    public Sprite weaponSprite;
    public Transform firingPoint;


    private void OnValidate()
    {
        weaponSprite = GetComponent<SpriteRenderer>().sprite;
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
