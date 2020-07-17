using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool isWeapon;
    private Weapon weapon;
    private WeaponType weaponType;
    private bool hadRigidbody;
    private float mass;

    public void Grab(Transform holderTransform)
    {
        transform.parent = holderTransform;

        if (TryGetComponent<Weapon>(out Weapon _Weapon))
        {
            weapon = _Weapon;
            weaponType = weapon.weaponType;
        }
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            hadRigidbody = true;
            mass = rigidbody.mass;
            Destroy(rigidbody);
        }
    }

    public void Drop()
    {
        transform.parent = null;
        if (isWeapon)
        {
            weapon.OnDrop(weaponType,weaponType.ammoCount);
        }
        if (hadRigidbody)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.mass = mass;
        }

    }
}
