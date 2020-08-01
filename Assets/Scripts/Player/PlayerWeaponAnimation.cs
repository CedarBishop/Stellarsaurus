using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimation : MonoBehaviour
{
    public RuntimeAnimatorController extractionAnimator;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Init (RuntimeAnimatorController controller)
    {
        if (controller == null )
        {
            return;
        }

        animator.runtimeAnimatorController = controller;
    }

    public void PlayAnimation (string clipName)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            return;
        }
        if (animator.runtimeAnimatorController == null)
        {
            return;
        }

        animator.Play(clipName);
    }

    public void PlayExtractionAnimation ()
    {
        if (extractionAnimator == null)
        {
            return;
        }

        animator.runtimeAnimatorController = extractionAnimator;
        animator.Play("Extraction");
    }

    public void StopAnimation ()
    {
        animator.runtimeAnimatorController = null;
    }
}
