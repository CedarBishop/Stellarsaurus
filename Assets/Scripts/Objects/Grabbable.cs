using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
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
