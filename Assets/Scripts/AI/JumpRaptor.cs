﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;
using PlatformerPathFinding.Examples;

public class JumpRaptor : Dinosaur
{
    public float targetResetTime;
    protected PathFindingAgent agent;
    [HideInInspector] public AiController controller;
    protected Transform[] targets;

    public override void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null)
    {
        base.Initialise(transforms, pteroGround, pteroAir);

        agent = GetComponent<PathFindingAgent>();
        controller = GetComponent<AiController>();
        agent.Init(FindObjectOfType<PathFindingGrid>());
        animator.SetBool("CanAttack", true);
        Destroy(rigidbody);
    }

    public Transform SetRandomGoal()
    {
        if (targets != null)
        {
            controller._goal = targets[Random.Range(0, targets.Length)];
        }

        return null;
    }
}
