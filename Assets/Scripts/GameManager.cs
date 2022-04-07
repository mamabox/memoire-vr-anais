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
    public TextMeshProUGUI taskNbTxt;
    public TextMeshProUGUI totalTimeTxt;
    public TextMeshProUGUI taskTimeTxt;

    //Time
    private float startTime;
    public float taskStartTime;
    public float endTime;
    private TimeSpan totalTime;
    private TimeSpan taskTime;

    public bool sessionStarted;
    public bool sessionPaused;
    public bool taskStarted;
    public bool taskEnded;
    public bool taskPaused;

    public bool readTaskInstructions;

    private Task1Manager task1;
    //private Task2Manager task2;
    //private Task3Manager task3;

    public int taskNb;

    void Start()
    {
        //StartSession();
    }

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        task1 = GameObject.Find("TaskManager").GetComponent<Task1Manager>();
        //task2 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task2Manager>();
        //task3 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task3Manager>();
        player = playerController.player;
        taskNb = 0; // The first task is task #1

        foreach (Transform child in routeHotspotsParent.transform) // for each route hotspot
        {
            routeHotspots.Add(child.gameObject);
        }

        OpenMenu();
    }

    private void Update()
    {
        UIupdate();
    }

    private void UIupdate()
    {
        totalTimeTxt.text = TimeSpan.FromSeconds(Time.time - startTime).ToString(@"mm\:ss");
        taskTimeTxt.text = taskTime.ToString(@"mm\:ss");
        taskNbTxt.text = "Task " + taskNb;

    }

    //Hide UI elements
    private void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        //taskPaused = true;
        dialogBox.SetActive(false);
        //debugUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void StartSession()
    {
        Debug.Log("StartSession()");
        sessionStarted = true;
        //sessionPaused = false;
        startTime = Time.time;

    }

    public void StartTask()
    {
        if (!taskStarted)   //if no task has started
        {
            taskStarted = true;
            Cursor.lockState = CursorLockMode.Locked;
            if (taskNb == 1)
            {
                task1.StartTask();
            }
            else
                Debug.Log("Task 2 or 3 not defined");
            readTaskInstructions = false;
        }
    }

    public void PauseTask()
    {
        Cursor.lockState = CursorLockMode.None;
        taskPaused = true;
    }

    public void ResumeTask()
    {
        Cursor.lockState = CursorLockMode.Locked;
        taskPaused = false;
    }

    public void EndTask()
    {
        endTime = Time.time;
        taskEnded = true;
        Cursor.lockState = CursorLockMode.None;
        //TODO: Calculate end time
    }

    public void ExitTask()
    {
        Debug.Log("ExitTask");
        OpenMenu();
        //SaveData();
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
