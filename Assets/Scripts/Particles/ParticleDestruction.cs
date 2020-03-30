using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestruction : MonoBehaviour
{
    public float lifetime;

    public void DestroyParticle()
    {
        StartCoroutine(DestructionSequence());
    }

    // Call this to destroy the particle upon starting to destroy it when it finishes.
    IEnumerator DestructionSequence()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
