using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSegment : MonoBehaviour
{
    private Window window;

    private void Awake()
    {
        window = transform.GetComponentInParent<Window>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            window.health -= collision.damage;
        }
    }
}
