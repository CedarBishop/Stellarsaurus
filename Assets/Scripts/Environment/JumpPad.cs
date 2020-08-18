using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpHeight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<GhostMovement>())
        {
            return;
        }
        if (collision.GetComponent<Rigidbody2D>())
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = jumpVelocity * transform.up;
            if (collision.GetComponent<PlayerMovement>())
            {
                collision.GetComponent<PlayerMovement>().OnJumpPadBoost();
            }
        }
    }
}
