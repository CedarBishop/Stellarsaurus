using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    public float dragScaler;
    public float gravityScaler;
    public float massScaler;
    public float velocityScalerOnEntry;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.mass *= massScaler;
            rigidbody.gravityScale *= gravityScaler;
            rigidbody.drag *= dragScaler;
            rigidbody.angularDrag *= dragScaler;
            rigidbody.velocity *= velocityScalerOnEntry;
        }

        if (collision.GetComponent<PlayerHealth>())
        {
            collision.GetComponent<PlayerHealth>().StopBurning();
        }
        if (collision.GetComponent<AI>())
        {
            collision.GetComponent<AI>().StopBurning();
        }       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Flame>())
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.mass /= massScaler;
            rigidbody.gravityScale /= gravityScaler;
            rigidbody.drag /= dragScaler;
            rigidbody.angularDrag /= dragScaler;
            //rigidbody.velocity /= 0.5f;
        }
    }

}
