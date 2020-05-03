using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestruction : MonoBehaviour
{
    public float lifetime;

    public void DestroyParticle()
    {
        Destroy(gameObject, lifetime);
    }
}
