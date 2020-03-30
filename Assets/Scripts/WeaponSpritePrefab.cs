using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpritePrefab : MonoBehaviour
{
    public Sprite weaponSprite;
    public Transform firingPointTransform;
    public Vector3 firingPoint;

    private void OnValidate()
    {
        weaponSprite = GetComponent<SpriteRenderer>().sprite;
        firingPoint = firingPointTransform.position;
    }

}
