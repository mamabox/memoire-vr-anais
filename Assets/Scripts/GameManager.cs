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
    private GameObject playerCam;
    public GameObject dialogBox;
    public GameObject debugUI;
    public GameObject menuUI;

    //UI Elements
    public TextMeshProUGUI taskNbTxt;
    public TextMeshProUGUI totalTimeTxt;
    public TextMeshProUGUI taskTimeTxt;
    public TextMeshProUGUI playerPositionTxt;
    public TextMeshProUGUI playerRotationTxt;
    public List<TextMeshProUGUI> TaskBtnLabels;
    public GameObject visor;
    private bool enableVisor;

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
    public bool readTrialInstructions;

    private Task1Manager task1;

    //Player Data
    public List<float> playerPos;
    public List<float> playerRot;
    
    //private Task2Manager task2;
    //private Task3Manager task3;

    public TaskData taskData;

    public int taskNb;

    void Start()
    {
        
        StartSession();
    }

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        task1 = GameObject.Find("TaskManager").GetComponent<Task1Manager>();
        //task2 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task2Manager>();
        //task3 = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<Task3Manager>();
        player = playerController.player;
        playerCam = playerController.playerCam;

        playerPos = new List<float> { 0,0};
        playerRot = new List<float> { 0,0 };
    

        taskNb = 0; // The first task is task #1

        foreach (Transform child in routeHotspotsParent.transform) // for each route hotspot
        {
            routeHotspots.Add(child.gameObject);
        }

        OpenMenu();
    }

    private void Update()
    {
        UpdateClock();
        UpdatePlayerCoord();
        UIupdate();
    }

    private void UpdateClock()
    {
        totalTime = TimeSpan.FromSeconds(Time.time - startTime);
        if (taskStarted && !taskEnded)  //if task is ongoing
        {
            taskTime = TimeSpan.FromSeconds(Time.time - taskStartTime);
            //Debug.Log("TimeSpan" + TimeSpan.FromSeconds(0));
        }
        else
            taskTime = TimeSpan.FromSeconds(0);

    }

    private void UpdatePlayerCoord()
    {
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.z;
        if (playerCam.transform.rotation.eulerAngles.x > 180)
            playerRot[1] = 360 - playerCam.transform.rotation.eulerAngles.x;
        else
            playerRot[1] = -playerCam.transform.rotation.eulerAngles.x;
        playerRot[0] = player.transform.rotation.eulerAngles.y;
    }

    // Manages the User interface not specific to tasks 
    private void UIupdate()
    {
        totalTimeTxt.text = totalTime.ToString(@"mm\:ss");
        taskTimeTxt.text = taskTime.ToString(@"mm\:ss");
        taskNbTxt.text = "Task " + taskNb;
        playerPositionTxt.text = playerPos[0].ToString("F2") + " , " + playerPos[1].ToString("F2");
        playerRotationTxt.text = playerRot[0].ToString("F2") + " , " + playerRot[1].ToString("F2");

    }

    //Hide UI elements
    public void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        //taskPaused = true;
        dialogBox.SetActive(false);
        
        //debugUI.SetActive(false);
        menuUI.SetActive(true);
        if (taskStarted)
            EndTask();
    }

    
    public void StartSession()
    {
        Debug.Log("StartSession()");
        sessionStarted = true;
        //sessionPaused = false;
        startTime = Time.time;
        playerController.SetFirstPersonSettings();
        SetVisorSize();

    }

    private void SetVisorSize()
    {
        float _visorSize = taskData.task1Data.visorSize;
        if (_visorSize > 0){
            //visor.SetActive(true);
            visor.GetComponent<RectTransform>().localScale = new Vector3(_visorSize, _visorSize, 1);
            
        }
    }

    public void StartTask(int taskNb)
    {
        
        if (!taskStarted)   //if no task has started, start task
        {
            Debug.Log("Game Manager - StartTask()");
            taskStarted = true;
            taskStartTime = Time.time;
            taskPaused = true;
            visor.SetActive(true);
            //Cursor.lockState = CursorLockMode.Locked;
            if (taskNb == 1)
            {
                task1.StartTask();
            }
            else
                Debug.Log("Task 2 or 3 not defined");
            readTaskInstructions = false;
        }
    }

    public void StartTrial()
    {
        taskPaused = false;
        if (taskNb == 1)
        {

            task1.StartTrial();
        }
        else
            Debug.Log("Task 2 or 3 not defined");
    }

    public void PauseTask()
    {
        //Cursor.lockState = CursorLockMode.None;
        taskPaused = true;
    }

    public void ResumeTask()
    {
        Cursor.lockState = CursorLockMode.Locked;
        taskPaused = false;
    }

    public void EndTask()
    {
        Debug.Log("End task");
        endTime = Time.time;
        taskStarted = false;
        taskEnded = true;
        //Cursor.lockState = CursorLockMode.None;
        SaveData();
        //OpenMenu();
        visor.SetActive(false);
    }

    public void EndSession()
    {
        Debug.Log("EndSession");
    }


    // Update is called once per frame
    public void Test()
    {
        playerController.GotoHotspot(cardDir[0]);

    }

    private void SaveData()
    {
        //TODO: Calculate end time
        Debug.Log("Save data");
        Debug.Log("Total task time: " + taskTime);
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
