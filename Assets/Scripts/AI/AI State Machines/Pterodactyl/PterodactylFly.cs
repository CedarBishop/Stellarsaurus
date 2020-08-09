using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;

public class PterodactylFly : StateMachineBehaviour
{
    private AI ai;
    private Rigidbody2D rigidbody;
    private Transform transform;
    private Animator _Animator;
    private AStar aStar;

    private float movementSpeed;
    private float swoopTimer;
    private List<Node> path;
    private int pathIndex;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        swoopTimer = Random.Range(10, 20);
        ai = animator.GetComponent<AI>();
        rigidbody = ai.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        movementSpeed = ai.aiType.movementSpeed;
        transform = ai.transform;
        _Animator = animator;
        aStar = ai.aStar;

        FindNewPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        CheckIfReachedTarget();

        SwoopCountdown();
    }

    void Move ()
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = (path[pathIndex].worldPosition - currentPos).normalized;
        rigidbody.velocity = direction * movementSpeed * Time.fixedDeltaTime;
    }

    void CheckIfReachedTarget ()
    {
        if (Vector2.Distance(path[pathIndex].worldPosition, transform.position) < 0.1f)
        {
            if (pathIndex < path.Count - 1)
            {
                pathIndex++;
            }
            else
            {
                FindNewPath();
            }
        }
    }

    void FindNewPath()
    {
        Transform target = ai.SetRandomGoal();
        if (target == null)
        {
            Debug.Log("Ptero new target is null");
            return;
        }

        path = aStar.FindPath(transform.position, target.position);
        pathIndex = 0;
    }

    void SwoopCountdown ()
    {
        if (swoopTimer <= 0)
        {
            _Animator.SetTrigger("Swoop");
        }
        else
        {
            swoopTimer -= Time.fixedDeltaTime;
        }       
    }
}
