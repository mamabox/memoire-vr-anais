using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;    //Player's walking speed
    public float runSpeed;
    public float lookSpeed; // Player's turning speed

    public GameObject player; // Player object

    private void Awake()
    {
        // Read player's  sp
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Move player to the hotspot's location
    public void GotoHotspot(string hotspot)
    {

    }
    
    //Move player to a specific coordinate
    public void GotoCoord(List<float> coord)
    {
        player.transform.position = new Vector3(coord[0], 1, coord[1]);
    }
}
