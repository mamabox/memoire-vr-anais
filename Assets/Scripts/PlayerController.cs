using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;    //Player's walking speed
    public float runSpeed;
    public float lookSpeed; // Player's turning speed
    private float newOrientation;
    public float startOrientation; // Player's start orientation

    private List<string> validOrientation = new List<string> { "N", "NE", "E", "SE", "S", "SO", "O", "NO" }; //List of valid orientations

    public GameObject player; // Player object
    public GameObject player2;

    private void Awake()
    {
        // Read player's  sp
        SetStartOrientation("S"); //Set the player's start direction for Task 1
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStartOrientation(string carDir)
    {
        startOrientation = GetOrientationAngle(carDir);
    }

    //Move player to the hotspot's location
    public void GotoHotspot(GameObject hotspot)
    {

        List<float> _coord = hotspot.GetComponent<Hotspot>().coord; // hotspot's coordinate
        string _hotspotOrientation = hotspot.GetComponent<Hotspot>().orientation.ToUpper();    // Upper case value of hotspot's orientation
        float _newOrientation = GetOrientationAngle(_hotspotOrientation);

        player.transform.position = new Vector3(_coord[0], 1, _coord[1]);   // Move player to the hotspot's coordinate

        Debug.Log("New Y rotation is " + _newOrientation);
        player.transform.eulerAngles = new Vector3(0, _newOrientation, 0);
        //player2.transform.eulerAngles = new Vector3(0, newOrientation.y, 0);
    }

    public float GetOrientationAngle(string carDir)
    {

        if (IsOrientationValid(carDir))    // if the hotspot's orientation is valid, rotate player to match its orientation
        {
            switch (carDir)
            {
                case "N":
                    newOrientation = 0;
                    break;
                case "NE":
                    newOrientation = 45;
                    break;
                case "E":
                    newOrientation = 90;
                    break;
                case "SE":
                    newOrientation = 135;
                    break;
                case "S":
                    newOrientation = 180;
                    break;
                case "SW":
                    newOrientation = 225;
                    break;
                case "W":
                    newOrientation = 270;
                    break;
                case "NW":
                    newOrientation = 315;
                    break;
            }
        }
        else    //if the hotspot orientation is invalid, point the player north

        {
            Debug.Log("Orientation " + carDir + "is invalid, setting orientation to N");
            newOrientation = 0;
        }
        return newOrientation;
    }

    // CHECK IF COORD orientation are valid
    private bool IsOrientationValid(string carDir)
    {

        if (validOrientation.Contains(carDir.ToUpper()))
        {
            //Debug.Log("Orientation "+ carDir + " is valid)");
            return true;
        }
        else
        {
            //Debug.Log("Orientation " + carDir + "invalid)");
            return false;
        }
    }
    
    //Move player to a specific coordinate
    public void GotoCoord(List<float> coord)
    {
        player.transform.position = new Vector3(coord[0], 1, coord[1]);
    }
}
