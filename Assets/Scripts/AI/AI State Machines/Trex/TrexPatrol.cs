using UnityEngine;

public class TrexPatrol : StateMachineBehaviour
{
    private AI ai;
    private Perception perception;
    private Transform transform;
    private Rigidbody2D rigidbody;

    LayerMask groundLayer;
    LayerMask wallLayer;
    LayerMask platformLayer;

    private float movementSpeed;
    private float wallDetectionDistance;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        movementSpeed = ai.aiType.movementSpeed;
        transform = animator.transform;

        groundLayer = ai.groundLayer;
        wallLayer = ai.wallLayer;
        platformLayer = ai.platformLayer;

        wallDetectionDistance = ai.aiType.wallDetectionDistance;

        rigidbody.mass = 1000;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", true);
        }
        Move();
        CalculateWallAndLedge();
    }

    void CalculateWallAndLedge()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, wallDetectionDistance, groundLayer) ||
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, wallDetectionDistance, wallLayer))   // Check if there is a wall in front of the ai
        {           
              perception.isFacingRight = !perception.isFacingRight;        
        }
        if (!Physics2D.Raycast(transform.position, (perception.isFacingRight) ? new Vector2(1, -1) : new Vector2(-1, -1), 4, groundLayer) &&
            !Physics2D.Raycast(transform.position, (perception.isFacingRight) ? new Vector2(1, -1) : new Vector2(-1, -1), 4, platformLayer))
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
    }

    void Move()
    {
        if (perception.isFacingRight)
        {
            rigidbody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
    }
}
