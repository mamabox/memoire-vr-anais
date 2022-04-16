using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Barrier.CS
 * 
 * Stores the limiter’s coordinates (string - "x_y”), and ID (int)
 * 
 */

public class Barrier : MonoBehaviour
{
    private GameManager gameManager;
    public int ID;
    //public float rotation;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

}