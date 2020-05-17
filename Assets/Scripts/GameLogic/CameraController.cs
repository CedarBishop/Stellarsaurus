using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public List<PlayerMovement> playersInGame = new List<PlayerMovement>();
    public Transform tracker;
    private Vector3 middlePoint;
    public CinemachineVirtualCamera cvc;
    [SerializeField] private float cameraZoomSpeed;
    [SerializeField] private float minZoom;

    private Camera mainCam;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        GetAveragePositionOfPlayers();
    }


    void FixedUpdate()
    {
        GetAveragePositionOfPlayers();
        UpdateOrthographicSize();
    }

    private void GetAveragePositionOfPlayers()
    {
        // Check if there are any players in the game.
        if (playersInGame.Count != 0)
        {
            middlePoint = Vector3.zero;
            foreach (PlayerMovement pm in playersInGame)     // for each player, add their positions together then divide for the middle point.
            {
                middlePoint += pm.transform.position;
            }
            middlePoint /= playersInGame.Count;

            tracker.position = middlePoint;   // Set middle point to transform.
        }
    }

    private void UpdateOrthographicSize()
    {
        if (playersInGame.Count <= 0)
            return;
        float dist = minZoom;
        float temp;
        for (int i = 0; i < playersInGame.Count; i++)
        {
            for (int m = i; m < playersInGame.Count; m++)
            {
                temp = Vector3.Distance(playersInGame[i].transform.position, playersInGame[m].transform.position);
                dist = (temp > dist) ? temp : dist;
            }
        }
        cvc.m_Lens.OrthographicSize = Mathf.Lerp(cvc.m_Lens.OrthographicSize, dist / 3.5f * mainCam.aspect, cameraZoomSpeed * Time.deltaTime);
        if (cvc.m_Lens.OrthographicSize < minZoom)
            cvc.m_Lens.OrthographicSize = minZoom;

        /// TODO:   Get Horizontal distances and find the largest
        ///         Get Vertical distances and times by aspect ratio to find largest
        ///         compart values
        ///         set orthographic size accordingly with specific magic number for both hor/vert.
    }
}
