﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool hadRigidbody;
    private float mass;
    private float gravityScale;
    private Transform holderTransform;
    private bool isHeld;
    public void Grab(Transform _HolderTransform)
    {
        transform.parent = _HolderTransform;
        isHeld = true;
        holderTransform = _HolderTransform;
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            hadRigidbody = true;
            mass = rigidbody.mass;
            gravityScale = rigidbody.gravityScale;
            Destroy(rigidbody);
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            transform.position = holderTransform.position;
        }        
    }

    public void Drop()
    {
        transform.parent = null;
        isHeld = false;
        
        if (hadRigidbody)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.mass = mass;
            rigidbody.gravityScale = gravityScale;
        }
    }
}
