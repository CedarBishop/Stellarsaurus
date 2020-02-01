using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    public PlayerMovement player;
    public LayerMask groundLayer;
    public LayerMask platformLayer;


    void FixedUpdate()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(player.transform.position,Vector2.down,20,groundLayer);
        RaycastHit2D platformHit = Physics2D.Raycast(player.transform.position, Vector2.down, 20, platformLayer);
        if (platformHit != null)
        {
            transform.position = platformHit.point;
        }
        else if (groundHit != null)
        {
            transform.position = groundHit.point;
        }
    }
}
