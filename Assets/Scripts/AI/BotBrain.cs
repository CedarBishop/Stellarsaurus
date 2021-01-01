using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
    public RuntimeAnimatorController aiController;

    public GameObject target;

    private Animator animator;
  

    void Start()
    {
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = aiController;
    }
}
