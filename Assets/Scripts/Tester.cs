using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;

public class Tester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AStar aStar = FindObjectOfType<AStar>();
            List<Node> path = aStar.FindPath(new Vector2(0, 0), new Vector2(10, 3));
            if (path == null)
            {
                print("Path is null");
                return;
            }
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log("Path index " + i + ": " + path[i].worldPosition.ToString());
            }
        }        
    }
}
