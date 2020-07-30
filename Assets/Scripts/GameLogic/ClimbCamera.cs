using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCamera : MonoBehaviour
{
    public float climbSpeed;

    public float timeBeforeStarting;
    public int towerHeight;
    public Grid bottomPiece;
    public Grid[] middlePieces;
    public Grid[] twentyBlockMiddlePieces;
    public Grid topPiece;
    public Vector2Int chanceOfTwentyBlockPiece;

    private bool isClimbing;
    private float currentYPosition;

    private void Start()
    {
        GenerateTower();
        StartCoroutine("StartClimbing");
    }

    private void FixedUpdate()
    {
        if (transform.position.y >= currentYPosition + 5)
        {
            return;
        }
        if (isClimbing == false)
        {
            return;
        }
        transform.position += Vector3.up * Time.fixedDeltaTime * climbSpeed;
    }

    void GenerateTower ()
    {
        Instantiate(bottomPiece, new Vector3(0, 0, 0), Quaternion.identity);
        currentYPosition = 10;
        for (int i = 0; i < towerHeight; i++)
        {
            if (twentyBlockMiddlePieces == null)
            {
                // ten tall peices
                Instantiate(middlePieces[Random.Range(0, middlePieces.Length)], new Vector3(0, currentYPosition, 0), Quaternion.identity);
                currentYPosition += 10;
            }
            else if (middlePieces == null)
            {
                // twenty tall peices
                Instantiate(twentyBlockMiddlePieces[Random.Range(0, twentyBlockMiddlePieces.Length)], new Vector3(0, currentYPosition, 0), Quaternion.identity);
                currentYPosition += 20;
            }
            else
            {
                int num = Random.Range(0, chanceOfTwentyBlockPiece.y);
                if (num < chanceOfTwentyBlockPiece.x)
                {
                    // twenty tall peices
                    Instantiate(twentyBlockMiddlePieces[Random.Range(0, twentyBlockMiddlePieces.Length)], new Vector3(0, currentYPosition, 0), Quaternion.identity);
                    currentYPosition += 20;
                }
                else
                {
                    // ten tall peices
                    Instantiate(middlePieces[Random.Range(0, middlePieces.Length)], new Vector3(0, currentYPosition, 0), Quaternion.identity);
                    currentYPosition += 10;
                }
            }
                       
        }
        
        Instantiate(topPiece, new Vector3(0, currentYPosition, 0), Quaternion.identity);
    }

    IEnumerator StartClimbing ()
    {
        yield return new WaitForSeconds(timeBeforeStarting);
        isClimbing = true;
    }
}
