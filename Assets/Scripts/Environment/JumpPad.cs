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
            //rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            rb.velocity = Quaternion.Euler(0, 0, transform.rotation.z) * new Vector2(rb.velocity.x, jumpVelocity) * Vector2.up;
            if (collision.GetComponent<PlayerMovement>())
            {
                collision.GetComponent<PlayerMovement>().OnJumpPadBoost();
            }
        }
    }
}
