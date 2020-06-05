using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Background Elements (Order from furthest back to closest)")]
    [SerializeField] private RectTransform[] backgroundSprites;
    [SerializeField] [Range(0, 1f)] private float parallaxSpeedX;
    [SerializeField] [Range(0, 1f)] private float parallaxSpeedY;

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
            backgroundSprites[i].position -= new Vector3(deltaMovement.x * parallaxSpeedX/100 * Mathf.Pow(2, i), deltaMovement.y * parallaxSpeedY/100 * Mathf.Pow(2, i));
        }

        // Reset last camera position
        lastCameraPosition = cameraTransform.position;
    }
}
