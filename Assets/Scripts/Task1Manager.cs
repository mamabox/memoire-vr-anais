using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using System.IO;

/* 4 trials: start at N,E, S, W */

public class Task1Manager : MonoBehaviour
{
    private float degreesToTarget;    //Correct angle to target
    private float correctRotationToTarget;
    private List<float> savedTrials = new List<float>();
    private List<string> savedTrialsUI = new List<string>();

    private GameManager gameMngr;
    private GameObject targetLocation;
    private PlayerController2 playerCtrlr;
    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private RouteManager routeMngr;
    private DialogBox dialogBox;
    public GameObject hotspots;

    //public TextMeshProUGUI TaskText;
    public GameObject task1UI;
    public TextMeshProUGUI trialTxt;
    public TextMeshProUGUI targetTxt;
    public TextMeshProUGUI distanceToTargetTxt;
    public TextMeshProUGUI playerRotationTxt;
    public TextMeshProUGUI angleToTargetTxt;
    public TextMeshProUGUI savedTrialsTxt;
    

    private int targetLocationIndex;
    private string targetLocationName;
    private string routeName;
    private int trialNb;    // 1 to 4
    private int targetNb; // 1 to 6
    private int maxTrial; // how many routes
    private int maxTargetObj;   // 6
    private float distanceToTarget;   // how far the player is to the target object

    

    private List<string> trialStart = new List<string> { "N", "E", "S", "W" };
    private List<string> routeN = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };


    private List<Hotspot> routeNObj;

    //SaveData
    private string dateTime;
    private StreamWriter sw;
    private string fileName;
    private char fileNameDelimiter = '-';
    private char delimiter = ',';

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerCtrlr = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController2>();
        routeMngr = GameObject.Find("RouteManager").GetComponent<RouteManager>();
        dialogBox = FindObjectOfType<GameManager>().dialogBox.GetComponent<DialogBox>();
    }

    public void Start()
    {
        SetupTask();
    }

    // Start is called before the first frame update
    public void StartTask()
    {
        Debug.Log("TASK 1 - Start()");
        gameMngr.taskNb = 1;
        //SetupTask();
        task1UI.SetActive(true);
        trialNb = 0;
        savedTrials = new List<float>();
        savedTrialsUI = new List<string>();
        //distanceToTarget = 0;
        //gameMngr.taskStarted = true;
        //gameMngr.taskPaused = true;

        dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.start, "trial");
        //StartTrial();
        //gameMngr.taskPaused = true;
        //StartTrial();
        playerCtrlr.calculatingRotation = true;
        StartSavingData();

    }

    // Update is called once per frame
    void Update()
    {
       if (gameMngr.taskStarted && gameMngr.taskNb == 1 && trialNb > 0)    //Update UI
        {
            distanceToTarget = Vector3.Distance(playerCtrlr.player.transform.position, targetLocation.transform.position); //update the distance only if a trial has started
            CalculateDegreeToTarget();
            UpdateUI();
            
        }

    }



    // Configure settings for this task - called once when program is launched
    public void SetupTask()
    {
        //gameMngr.taskNb = 1;
        //trialNb = 0;
        //gameMngr.taskPaused = true;
        //task1UI.SetActive(true);
        //dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.start, "task");
        //targetNb = 0;
        maxTrial = gameMngr.taskData.task1Data.task1Trials.Count();
        maxTargetObj = gameMngr.taskData.task1Data.locations.Count();
        task1UI.SetActive(false);
        //routeMngr.SpawnLine(routeN, 1);
        DrawRoutes();
        
        //task2UI.SetActive(false);
        //task3UI.SetActive(false);
        //gameMngr.dialogBox.SetActive(true);

    }

    private void DrawRoutes()
    {
        for (int x = 0; x < maxTrial; x++)
        {
            //Debug.Log("Draw line " + x + " out of " + maxTrial);
            routeMngr.SpawnLine(gameMngr.taskData.task1Data.task1Trials[x].routeCoord, x+1, gameMngr.taskData.task1Data.task1Trials[x].routeName);

        }
    }


    // Begin the task
    public void StartTrial()
    {
        string instructions;
        string image;

        gameMngr.taskPaused = false;
        Debug.Log("StartTrial");
        if (trialNb < maxTrial)    //if there are trials left
        {
            //Cursor.lockState = CursorLockMode.Locked;
            trialNb++;
            //TODO: Make visor visible
            //Debug.Log("Task 1 - Trial #: " + trialNb + " / " + maxTrial);
            //SetRoute()
            routeName = gameMngr.taskData.task1Data.task1Trials[trialNb-1].routeName;
            DisplayRoute(trialNb);  //Display the current route and hide all others

            targetNb = 1; //Set the target object to the first POI
            startHotspot = gameMngr.cardDir[trialNb-1];
            playerCtrlr.GotoHotspot(startHotspot);
            SetTargetObj();
            instructions = gameMngr.taskData.task1Data.instructions.attempts[0] + gameMngr.taskData.task1Data.instructions.attempts[1] + " " + targetLocationName + gameMngr.taskData.task1Data.instructions.attempts[2];
            image = gameMngr.taskData.task1Data.locations[targetLocationIndex].filename;
            dialogBox.OpenDialogBoxImg(instructions, image, "none");
            

        }
        else // notrials left
        {
            EndTask();
        }

    }

    private void DisplayRoute(int routeNb)
    {
        for (int x = 0; x < maxTrial; x++)  // for each route
        if (x != routeNb-1)    
            routeMngr.task1Routes[x].SetActive(false);
        else
            routeMngr.task1Routes[x].SetActive(true);
    }

    private void HideAllRoutes()
    {
        for (int x = 0; x < maxTrial; x++)  // for each route
                    routeMngr.task1Routes[x].SetActive(false);
           
    }

    void SetTargetObj()
    {

        //Debug.Log("SetTargetObj");
        targetLocationIndex = gameMngr.taskData.task1Data.task1Trials[trialNb-1].targetLocations[targetNb-1]-1; //Index of target location for current trial
//        Debug.Log("Target location index is " + targetLocationIndex);

        //Debug.Log("TargetObj() index: " + targetLocationIndex + " / " + maxTargetObj);
        targetLocation = gameMngr.POI[targetLocationIndex];
        
        targetLocationName = gameMngr.taskData.task1Data.locations[targetLocationIndex].pronoun + " " + gameMngr.taskData.task1Data.locations[targetLocationIndex].name;

    }

    private void CalculateDegreeToTarget()
    {
        //gameMngr.taskPaused = true;
        //playerCtrlr.calculatingRotation = true;
        playerCtrlr.rotationCheck.transform.LookAt(targetLocation.transform);
        correctRotationToTarget = playerCtrlr.rotationCheck.transform.rotation.eulerAngles.y;
        //gameMngr.taskPaused = false;
        //playerCtrlr.calculatingRotation = false;
        degreesToTarget = Math.Abs(correctRotationToTarget - gameMngr.playerRot[0]);

    }

    public void EndTask()
    {
        playerCtrlr.calculatingRotation = false;
        dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.end, "menu");
        task1UI.SetActive(false);
        HideAllRoutes();
        //gameMngr.EndTask();
        Debug.Log("End of task 1");

        gameMngr.endTime = Time.time;
        gameMngr.taskStarted = false;
        gameMngr.taskEnded = true;
        gameMngr.taskNb = 0;
        StopSavingData();
        //Cursor.lockState = CursorLockMode.None;


        gameMngr.visor.SetActive(false);
    }

    public void OnValidation()
    {
        string instructions;
        string image;

        SaveData();
        //Debug.Log("Inside Task1 OnValidation()");
        if (targetNb < maxTargetObj) // if there are target objects left in this trial
            {
            CalculateDegreeToTarget();
            
            savedTrials.Add(degreesToTarget);
            savedTrialsUI.Add(degreesToTarget.ToString("F2"));
                targetNb++;
                SetTargetObj();
            instructions = gameMngr.taskData.task1Data.instructions.attempts[1] + " " + targetLocationName + gameMngr.taskData.task1Data.instructions.attempts[2];
            image = gameMngr.taskData.task1Data.locations[targetLocationIndex].filename;
            dialogBox.OpenDialogBoxImg(instructions, image, "none");
           
        }
            else
            {

                StartTrial(); // start the next trial
            }
        
    }

    private void StartSavingData()
    {
        SetFileName();
        sw = File.AppendText(gameMngr.filePath + fileName);
        sw.WriteLine(HeadersConstructor()); //Add Headers to the file
    }

    private void SetFileName()
    {
        string participantID = "0";
        string sessionSummaryText = "ID" + participantID + fileNameDelimiter + "TSK" + gameMngr.taskNb;
        dateTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        fileName = dateTime + fileNameDelimiter + sessionSummaryText + ".csv";

    }

    private string HeadersConstructor()
    {
        string sessionHeader = "dateTime" + delimiter + "time" + delimiter + "participantID" + delimiter + "task";
        string trialHeader = "trialNb" + delimiter + "startLocation" + delimiter + "targetNb" + delimiter + "targetLocationIndex" + delimiter + "targetLocation";
        string playerHeader = "playerRotation" + delimiter + "startRotation" + delimiter + "correctRotationToTarget" + delimiter + "degreesToTarget" + delimiter + "distanceToTarget";

        return sessionHeader + delimiter + trialHeader + delimiter + playerHeader;
    }

    private void SaveData()
    {
        string sessionData = dateTime + delimiter + gameMngr.taskTime.ToString(@"mm\:ss") + delimiter + gameMngr.participantID + delimiter + gameMngr.taskNb;
        string trialData = "" + trialNb + delimiter + startHotspot.GetComponent<Hotspot>().desc + delimiter + targetNb + delimiter + targetLocationIndex + delimiter + targetLocationName;
        string playerData = gameMngr.playerRot[0].ToString("F2") + delimiter + "starRotation" + delimiter + correctRotationToTarget + delimiter + degreesToTarget + delimiter + distanceToTarget;

        sw.WriteLine(sessionData + delimiter + trialData + delimiter + playerData);
    }

    public void StopSavingData()
    {
        sw.Close();
    }

    private void UpdateUI()
    {
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial + ": " + routeName;
        targetTxt.text = "Target: " + targetNb + " / " + maxTargetObj + ": " + targetLocationName;
        //playerRotationTxt.text = "temp";
        playerRotationTxt.text = "Player rot: " + gameMngr.playerRot[0].ToString("F2");
        distanceToTargetTxt.text = "Distance to target: " + distanceToTarget.ToString("F2");
        angleToTargetTxt.text = "Degrees to target: " + degreesToTarget.ToString("F2");
        savedTrialsTxt.text = "Recorded degrees to target: " + String.Join(",", savedTrialsUI);

    }
}
