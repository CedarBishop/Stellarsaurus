using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;

public class PterodactylSwoop : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Animator _Animator;
    Transform transform;
    Rigidbody2D rigidbody;
    private float movementSpeed;
    private float swoopSpeed;
    private float pathFindingSwoopSpeed;

    private Transform startGround;
    private Transform endGround;
    private bool hasTwoTargets;
    private bool reachedFirstTarget;

    private List<Node> path;
    private int pathIndex;
    private AStar aStar;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        transform = animator.transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        aStar = ai.aStar;
        ai.OnHit += Retreat;
        swoopSpeed = ai.aiType.swoopSpeed;
        pathFindingSwoopSpeed = ai.aiType.pathFindingSwoopSpeed;

        hasTwoTargets = SetStartAndEnd();
        reachedFirstTarget = false;
        if (hasTwoTargets)
        {
            FindNewPath();
        }
        ai.RefreshCollider(true);
    }

    private void OnDestroy()
    {
        ai.OnHit -= Retreat;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasTwoTargets)
        {
            Move();
            CheckIfReachedIndex();
        }
        else
        {
            Swoop();
            WallCheck();
        }
        Damage();
    }

    void Retreat ()
    {
        ai.OnHit -= Retreat;
        _Animator.SetTrigger("Retreat");
    }
    
    bool SetStartAndEnd ()
    {
        if (ai.pteroGroundTargets != null)
        {
            if (ai.pteroGroundTargets.Length >= 1)
            {
                PairTargets targets = ai.pteroGroundTargets[Random.Range(0, ai.pteroGroundTargets.Length)];
                if (Vector2.Distance(transform.position, targets.targetOne.position) <= Vector2.Distance(transform.position, targets.targetTwo.position))
                {
                    startGround = targets.targetOne;
                    endGround = targets.targetTwo;
                }
                else
                {
                    startGround = targets.targetTwo;
                    endGround = targets.targetOne;
                }
                
                if (startGround != null && endGround != null)
                {
                    return true;
                }
            }
        }

        return false;
    }


    void Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = (path[pathIndex].worldPosition - currentPos).normalized;
        rigidbody.velocity = direction * pathFindingSwoopSpeed * Time.fixedDeltaTime;
    }

    void CheckIfReachedIndex()
    {
        if (Vector2.Distance(path[pathIndex].worldPosition, transform.position) < 0.1f)
        {
            if (pathIndex < path.Count - 1)
            {
                pathIndex++;
            }
            else
            {
                ReachedTarget();
            }
        }
    }

    void FindNewPath()
    {
        if (reachedFirstTarget)
        {
            pathIndex = 0;
            path = aStar.FindPath(transform.position, endGround.position);
        }
        else
        {
            pathIndex = 0;
            path = aStar.FindPath(transform.position, startGround.position);
        }
    }

    void ReachedTarget ()
    {
        if (reachedFirstTarget)
        {
            Retreat();
        }
        else
        {
            reachedFirstTarget = true;
            FindNewPath();
        }
    }


    void Swoop ()
    {
        rigidbody.velocity = new Vector2((perception.isFacingRight)?movementSpeed: -movementSpeed, -swoopSpeed) * Time.fixedDeltaTime;        
    }

    void Damage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ai.aiType.attackSize);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<PlayerHealth>())
                {
                    collider.GetComponent<PlayerHealth>().HitByAI(ai.aiType.attackDamage);
                }
            }
        }
    }

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            Retreat();
        }
    }
}
