﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraController : MonoBehaviour
{
    public List<Transform> playersInGame = new List<Transform>();
    public Transform tracker;
    private Vector3 middlePoint;
    public CinemachineVirtualCamera cvc;
    [SerializeField] private float cameraZoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float sizeBuffer;

    private Camera mainCam;

    private Vector2 targetBounds;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        GetAveragePositionOfPlayers();
    }


    void LateUpdate()
    {
        GetAveragePositionOfPlayers();

        if (playersInGame.Count != 0)
        {
            //UpdateOrthographicSize();
            UpdateOrthographicSizeNEW();
        }
    }

    private void GetAveragePositionOfPlayers()
    {
        // Check if there are any players in the game.
        if (playersInGame.Count != 0)
        {
            middlePoint = Vector2.zero;
            foreach (Transform pm in playersInGame)     // for each player, add their positions together then divide for the middle point.
            {
                middlePoint += pm.position;
            }
            middlePoint /= playersInGame.Count;

            tracker.position = new Vector2(middlePoint.x, middlePoint.y);   // Set middle point to transform.
        }
    }

    private void UpdateOrthographicSize()
    {
        if (playersInGame.Count <= 0)
            return;
        float dist = minZoom;
        float temp;
        float newOrthographicSize;  // This is the new size being calculated this frame

        // Calculate which 2 players are the furthest apart
        for (int i = 0; i < playersInGame.Count; i++)
        {
            for (int m = i; m < playersInGame.Count; m++)
            {
                temp = Vector2.Distance(playersInGame[i].position, playersInGame[m].position);
                //float distX = Mathf.Abs(playersInGame[i].transform.position.x - playersInGame[m].transform.position.x);
                //float distY = Mathf.Abs(playersInGame[i].transform.position.y - playersInGame[m].transform.position.y);

                dist = (temp > dist) ? temp : dist;
            }
        }
        cvc.m_Lens.OrthographicSize = Mathf.Lerp(cvc.m_Lens.OrthographicSize, dist / 3f * mainCam.aspect, cameraZoomSpeed * Time.deltaTime);
        if (cvc.m_Lens.OrthographicSize < minZoom)
            cvc.m_Lens.OrthographicSize = minZoom;

        /// TODO:   Get Horizontal distances and find the largest
        ///         Get Vertical distances and times by aspect ratio to find largest
        ///         compart values
        ///         set orthographic size accordingly with specific magic number for both hor/vert.
    }

    // https://answers.unity.com/questions/1231701/fitting-bounds-into-orthographic-2d-camera.html
    private void UpdateOrthographicSizeNEW()
    {
        if (playersInGame.Count > 1)
        {
            float xBounds = 0;
            float yBounds = 0;

            for (int i = 0; i < playersInGame.Count; i++)
            {
                for (int m = i; m < playersInGame.Count; m++)
                {
                    float distX = Mathf.Abs(playersInGame[i].position.x - playersInGame[m].position.x);
                    float distY = Mathf.Abs(playersInGame[i].position.y - playersInGame[m].position.y);

                    xBounds = (distX > xBounds) ? distX : xBounds;
                    yBounds = (distY > yBounds) ? distY : yBounds;

                    // catches when the players are all in the same line, prevents unwanted camera zooming
                    if (xBounds == 0)
                        xBounds = 0.1f;
                    if (yBounds == 0)
                        yBounds = 0.1f;
                }
            }
            targetBounds = new Vector2(xBounds, yBounds);
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = targetBounds.x / targetBounds.y;
            //Debug.Log("Screen Ratio: " + screenRatio +
            //          "\nTarget Ratio: " + targetRatio);
            if (targetRatio > 1000)
                targetRatio = 1000;
            
            if (screenRatio >= targetRatio)
            {
                //cvc.m_Lens.OrthographicSize = (targetBounds.y / 2);
                cvc.m_Lens.OrthographicSize = Mathf.Lerp(cvc.m_Lens.OrthographicSize, (targetBounds.y / 2) + sizeBuffer, cameraZoomSpeed * Time.deltaTime);
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                //cvc.m_Lens.OrthographicSize = (targetBounds.y / 2 * differenceInSize);
                cvc.m_Lens.OrthographicSize = Mathf.Lerp(cvc.m_Lens.OrthographicSize, (targetBounds.y / 2 * differenceInSize) + sizeBuffer, cameraZoomSpeed * Time.deltaTime);
            }
        }
        if (cvc.m_Lens.OrthographicSize < minZoom)
            cvc.m_Lens.OrthographicSize = minZoom;

    }
}
