using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public bool detectsTarget;

    public string detectionTag;
    public float hearingRadius;
    public float viewingDistance;
    public float fieldOfView;


    private void FixedUpdate()
    {
        detectsTarget = (Vision() || Hearing());
    }

    private bool Hearing ()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hearingRadius);

        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag(detectionTag))
                {
                    return true;
                }
            }
           
        }

        return false;
    }

    private bool Vision ()
    {
        return false;
    }

    private Vector2 Endpoint ()
    {
        Vector2 v = new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * fieldOfView), (float)Mathf.Cos(Mathf.Deg2Rad * fieldOfView));
        v *= viewingDistance;
        v += new Vector2(transform.position.x, transform.position.y);
        return v;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, Endpoint());

    }


}
