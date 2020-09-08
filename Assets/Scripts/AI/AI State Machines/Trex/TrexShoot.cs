using UnityEngine;

public class TrexShoot : StateMachineBehaviour
{
    private Trex ai;
    private Perception perception;
    private AIProjectile aiProjectile;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private Transform aimOrigin;

    private Vector2 directionToTarget;
    private Vector2 firingPos;
    private float shootTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<Trex>();
        perception = animator.GetComponent<Perception>();
        shootTimer = ai.attackCooldown;
        aiProjectile = ai.projectile;
        rigidbody = ai.GetComponent<Rigidbody2D>();
        spriteRenderer = ai.GetComponent<SpriteRenderer>();
        aimOrigin = ai.aimOrigin;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget == false)
        {
            animator.SetBool("TargetDetected", false);
        }
        if (shootTimer <= 0)
        {
            Shoot();
            Debug.Log("Shoot");
            shootTimer = ai.attackCooldown;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
        rigidbody.velocity = Vector2.zero;

        FaceTarget();
        AimAtTarget();
    }

    void FaceTarget ()
    {
        if (perception.targetTransform == null)
        {
            return;
        }

        if (perception.targetTransform.position.x < ai.transform.position.x)
        {
            perception.isFacingRight = false;
            spriteRenderer.flipX = true;
            ai.aimSpriteRenderer.flipX = true;
        }
        else
        {
            perception.isFacingRight = true;
            spriteRenderer.flipX = false;
            ai.aimSpriteRenderer.flipX = false;
        }
    }

    void AimAtTarget ()
    {
        if (perception.targetTransform == null)
        {
            return;
        }

        firingPos = new Vector2(((perception.isFacingRight) ? ai.projectileFiringPoint.position.x : -ai.projectileFiringPoint.position.x) + ai.transform.position.x, ai.projectileFiringPoint.position.y + ai.transform.position.y);
        float deviation = ai.projectileDeviation;
        directionToTarget = new Vector2(perception.targetTransform.position.x + Random.Range(-deviation, deviation), perception.targetTransform.position.y + Random.Range(-deviation, deviation)) - firingPos;
        aimOrigin.transform.right = Vector3.Slerp(aimOrigin.transform.right, ((perception.isFacingRight) ? 1: -1) * new Vector3(directionToTarget.x, directionToTarget.y, 0), 3 * Time.fixedDeltaTime);
    }

    void Shoot ()
    {
        if (perception.targetTransform == null)
        {
            return;
        }

        AIProjectile projectile =  Instantiate(aiProjectile, firingPos, aimOrigin.rotation);
        projectile.InitialiseProjectile(ai.attackDamage,directionToTarget,ai.projectileForce,ai.attackRange);

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_AILaser");
        }
    }
}
