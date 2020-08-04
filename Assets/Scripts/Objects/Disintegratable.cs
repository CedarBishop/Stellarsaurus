using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disintegratable : MonoBehaviour
{
    public string[] objectTags;
    public float timeTillDestroy;
    public float animationTime;

    private bool startedDisintegrating;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.collider.tag;
        
        foreach (var tag in objectTags)
        {
            if (collisionTag == tag)
            {
                if (startedDisintegrating == false)
                {
                    if (collision.collider.transform.position.y - transform.position.y > 0)
                    {
                        startedDisintegrating = true;
                        StartCoroutine("Disintegrate");
                    }                    
                }
            }
        }
    }

    IEnumerator Disintegrate()
    {
        yield return new WaitForSeconds(timeTillDestroy);
        Destroy(gameObject);

    }

}
