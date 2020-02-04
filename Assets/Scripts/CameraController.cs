using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<PlayerMovement> playersInGame = new List<PlayerMovement>();
    public float minOrthographicSize;
    public float maxOrthographicSize;
    public float orthographicSizeScaler;
    float furthestDistanceFromCentre;
    Camera camera;


    private void Start()
    {
        camera = GetComponent<Camera>();
    }


    void FixedUpdate()
    {
        if (playersInGame == null)
        {
            return;
        }

        furthestDistanceFromCentre = 0;


        for (int i = 0; i < playersInGame.Count; i++)
        {
            float currentPlayerDistance = Vector2.Distance(Vector2.zero,playersInGame[i].transform.position);
            if (currentPlayerDistance > furthestDistanceFromCentre)
            {
                furthestDistanceFromCentre = currentPlayerDistance;
            }
        }

        float size = furthestDistanceFromCentre * orthographicSizeScaler;
        if (size > maxOrthographicSize)
        {
            size = maxOrthographicSize;
        }
        else if (size < minOrthographicSize)
        {
            size = minOrthographicSize;
        }
        camera.orthographicSize = size;

    }
}
