using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
    public GameObject debugUI;
    public GameObject menuUI;

    //UI Elements
    public TextMeshProUGUI totalTimeTxt;
    public TextMeshProUGUI taskTimeTxt;

    //Time
    private float startTime;
    public float taskStartTime;
    public float endTime;
    private TimeSpan totalTime;
    private TimeSpan taskTime;

    private bool sessionStarted;
    private bool sessionPaused;
    private bool taskStarted;
    private bool taskEnded;

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

        HideUI();
    }


    private void UIupdate()
    {
        taskTimeTxt.text = taskTime.ToString(@"mm\:ss");
    }

    //Hide UI elements
    private void HideUI() {
        Debug.Log("Hide UI");
        dialogBox.SetActive(false);
        debugUI.SetActive(false);
        menuUI.SetActive(false);
    }

    public void StartSession()
    {
        sessionStarted = true;
        startTime = Time.time;
    }

    public void EndTask()
    {
        endTime = Time.time;
        taskEnded = true;
        //TODO: Calculate end time
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

    public void CloseDialogBox()
    {
        dialogBox.SetActive(false);
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
