using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Background Elements (Order from furthest back to closest)")]
    [SerializeField] private Transform[] backgroundSprites;
    [SerializeField] [Range(0, 0.1f)] private float parallaxSpeedX;
    [SerializeField] [Range(0, 0.1f)] private float parallaxSpeedY;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;


    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        for(int i = 0; i < backgroundSprites.Length; i++)
        {
            backgroundSprites[i].position -= new Vector3(deltaMovement.x * parallaxSpeedX * i, deltaMovement.y * parallaxSpeedY * i);
        }

        // Reset last camera position
        lastCameraPosition = cameraTransform.position;

        // Adjust scale to camera's orthographic size
        float size = Camera.main.orthographicSize * Camera.main.aspect / 9;
        transform.localScale = new Vector2(size, size);
    }

    private void Update()
    {

    }
}
