using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotspot: MonoBehaviour
{
    public string ID;
    public string article;
    public string desc;
    public List<float> coord;   // the hotspot's X and Z coordinates
    public string orientation;  // in what cardinal direction to orient the player if are moved to this hotspot

    // Start is called before the first frame update
    void Awake()
    {
        SaveCoord();
    }

    // Store hotspot X and Z coordinates to public variables
    void SaveCoord()
    {
        coord = new List<float> { transform.position.x, transform.position.z};
    }
}