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

    public void PlayAnimation (RuntimeAnimatorController controller, string clipName)
    {
        if (controller == null || string.IsNullOrEmpty(clipName))
        {
            return;
        }

        animator.runtimeAnimatorController = controller;
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
