using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCamera : MonoBehaviour
{
    public float climbSpeed;

    public int towerHeight;
    public Grid bottomPiece;
    public Grid[] middlePieces;
    public Grid topPiece;

    private void Start()
    {
        GenerateTower();
    }

    private void FixedUpdate()
    {
        if (transform.position.y >= (towerHeight * 10) + 5)
        {
            return;
        }
        transform.position += Vector3.up * Time.fixedDeltaTime * climbSpeed;
    }

    void GenerateTower ()
    {
        Instantiate(bottomPiece, new Vector3(0, 0, 0), Quaternion.identity);

        for (int i = 1; i < towerHeight; i++)
        {
            Instantiate(middlePieces[Random.Range(0, middlePieces.Length)], new Vector3(0, i * 10, 0 ), Quaternion.identity);
        }
        
        Instantiate(topPiece, new Vector3(0, towerHeight * 10, 0), Quaternion.identity);
    }
}
