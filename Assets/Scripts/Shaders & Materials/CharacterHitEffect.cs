using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitEffect : MonoBehaviour
{
    private float hitOpacity;
    public SpriteRenderer spriteRenderer;

    public float fade = 1;
    public float gain = 1;

    public enum HitEffect
    {
        NoBlend,
        Smooth,
        SmoothOut,
    }

    HitEffect effect;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.material = GetComponent<Material>();
    }

    private void Update()
    {
        switch (effect)
        {
            case HitEffect.NoBlend:

                setOpacity(hitOpacity);
                break;
            case HitEffect.Smooth:
                setOpacity(hitOpacity);
                break;
            case HitEffect.SmoothOut:
                setOpacity(hitOpacity);
                break;
            default:
                setOpacity(1);
                break;
        }
    }

    private void setOpacity(float f)
    {
        spriteRenderer.material.SetFloat("_Opacity", f);
    }
}
