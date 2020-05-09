using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private GameObject windowSegment;
    [SerializeField] private int windowHeight = 1;

    public int health = 3;

    void OnValidate()
    {
        
    }
}
