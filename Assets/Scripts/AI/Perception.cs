using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public bool detectsTarget;
    public Transform targetTransform;

    public string detectionTag;
    [Range(0f, 20f)]
    public float hearingRadius;
    [Range(0f, 20f)]
    public float viewingDistance;
    [Range(0f,360f)]
    public float fieldOfView;
    [Range(4,12)]
    public int numOfRays = 4;
    public bool isFacingRight;

    private void Start()
    {
        int num = Random.Range(0,2);
        isFacingRight = (num > 0) ? true : false;
    }

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
                    targetTransform = colliders[i].transform;
                    return true;
                }
            }
           
        }

        return false;
    }

    private bool Vision ()
    {
        for (int i = 0; i < numOfRays; i++)
        {
            float viewScaler = -0.5f;
            viewScaler += (i * (1.0f / ((float)numOfRays - 1)));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionFromAngle(fieldOfView * viewScaler), viewingDistance);
            if (hit)
            {
                if (hit.collider.CompareTag(detectionTag))
                {
                    targetTransform = hit.transform;
                    return true;
                }
            }
        }


        return false;
    }

    private Vector3 DirectionFromAngle (float angle)
    {
        angle += (isFacingRight) ? 90f : -90f;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad) , 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
        Gizmos.color = Color.red;

        for (int i = 0; i < numOfRays; i++)
        {
            float viewScaler = -0.5f;
            viewScaler += (i *(1.0f / ((float)numOfRays - 1)));
            Gizmos.DrawLine(transform.position, transform.position + (DirectionFromAngle(fieldOfView * viewScaler) * viewingDistance));
        }            
    }


}
