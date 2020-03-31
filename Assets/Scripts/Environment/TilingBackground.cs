using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingBackground : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public Vector2 start;
    public Vector2 end;
    public bool isLessThanEnd;
    public bool compareY;
  

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
       if (compareY)
        {
            if (isLessThanEnd)
            {
                if (transform.position.y < end.y)
                {
                    transform.position = start;
                }
            }
            else
            {
                if (transform.position.y > end.y)
                {
                    transform.position = start;
                }
            }
        }
        else
        {
            if (isLessThanEnd)
            {
                if (transform.position.x < end.x)
                {
                    transform.position = start;
                }
            }
            else
            {
                if (transform.position.x > end.x)
                {
                    transform.position = start;
                }
            }
        }
    }
}
