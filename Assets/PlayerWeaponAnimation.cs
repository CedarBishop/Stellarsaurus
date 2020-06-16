using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimation : MonoBehaviour
{
    private Animator animator;

    public RuntimeAnimatorController[] animatorControllers;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void PlayAnimation (string weaponName)
    {
        foreach (var item in animatorControllers)
        {
            if (item.name == weaponName)
            {
                animator.runtimeAnimatorController = item;
                animator.Play(weaponName);
            }
            
        }
    }

    public void StopAnimation ()
    {
        animator.runtimeAnimatorController = null;
    }
}
