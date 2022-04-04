using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> cardDir;
    public List<GameObject> POI;
    public GameObject routeHotspotsParent;
    public List<GameObject> routeHotspots;

    public List<string> POINames;

    private PlayerController playerController;
    private GameObject player;
    public GameObject dialogBox;

    private Task1Manager task1;
    //private Task2Manager task2;
    //private Task3Manager task3;

    public int taskNb;


    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        task1 = GameObject.Find("TaskManager").GetComponent<Task1Manager>();
        //task2 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task2Manager>();
        //task3 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task3Manager>();
        player = playerController.player;
        taskNb = 1; // The first task is task #1

        foreach (Transform child in routeHotspotsParent.transform) // for each route hotspot
        {
            routeHotspots.Add(child.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        task1.SetupTask();
    
        //playerController.GotoCoord(cardDir[0].GetComponent<Hotspot>().coord);
    }

    // Update is called once per frame
    public void Test()
    {
        playerController.GotoHotspot(cardDir[0]);

    }

    public void OnValidation()
    {
        if (taskNb == 1)
            task1.OnValidation();
            
    }

    public int ReturnHotspotIndex(string name)
    {

        for (int x = 0; x < routeHotspots.Count; x++) // for each route hotspot
        {
            if (name == routeHotspots[x].GetComponent<Hotspot>().ID)
                return x;
        }

        return -1;
    }
}
