using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.IO;

public class Task2Manager : MonoBehaviour
{
    private float degreesToTarget;    //Correct angle to target
    private float correctRotationToTarget;
    private List<float> savedTrials = new List<float>();
    private List<string> savedTrialsUI = new List<string>();

    private GameManager gameMngr;

    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private GameObject targetLocation;
    public List<GameObject> allStartHotspots;  // Hotspots for all - added in Unity Editor
    
    private PlayerController2 playerCtrlr;

    private DialogBox dialogBox;

    //public TextMeshProUGUI TaskText;
    public GameObject task2UI;
    public TextMeshProUGUI trialTxt;
    public TextMeshProUGUI startTargetTxt;
    public TextMeshProUGUI distanceToTargetTxt;
    public TextMeshProUGUI playerRotationTxt;
    public TextMeshProUGUI angleToTargetTxt;
    public TextMeshProUGUI savedTrialsTxt;
    public TextMeshProUGUI avgDegreesToTargetTxt;

    private int targetLocationIndex;
    private string targetLocationName;
    private int startLocationIndex;
    private string startLocationName;

    private int trialNb;    // 1 to 12
    private int targetNb; // 1 to 6
    private int maxTrial; // how many routes (6 easy and 6 hard)
    //private int maxTargetObj;   // 6
    private float distanceToTarget;   // how far the player is to the target object

    //Calculation variables
    private float startRotation;    // Player's start rotation
    private float correctRotation;  // Correct player rotation is they are pointing at the target object
    private float endRotation;      // Player's validated rotation
    private float rotationError;    // Absolute value of difference between the correct rotation and the validated rotation
    private float totalDegreesToTarget;    // Sum of rotation error for all trials
    private float avgDegreesToTarget; // Avg rotation error, updated after each trial



    //SaveData
    private string dateTime;
    private StreamWriter sw;
    private string fileName;
    private char fileNameDelimiter = '-';
    private char delimiter = ',';
    private string difficulty;

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerCtrlr = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController2>();
        dialogBox = FindObjectOfType<GameManager>().dialogBox.GetComponent<DialogBox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupTask();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMngr.taskStarted && gameMngr.taskNb == 2 && trialNb > 0)    //Update UI
        {
            distanceToTarget = Vector3.Distance(playerCtrlr.player.transform.position, targetLocation.transform.position); //update the distance only if a trial has started
            degreesToTarget = Math.Abs(correctRotationToTarget - gameMngr.playerRot[0]);
            UpdateUI();
        }
    }

    // Configure settings for this task - called once when program is launched
    public void SetupTask()
    {
        //gameMngr.taskNb = 2;
        //trialNb = 0;
        //targetNb = 0;
        //gameMngr.taskPaused = true;
        
        maxTrial = gameMngr.taskData.task2Data.task2Trials.Count();
        task2UI.SetActive(false);
    }

    // Runs each time the task is selected from the menu
    public void StartTask()
    {
        Debug.Log("Task 2 - Start()");

        //Initialise  variables
        trialNb = 0;
        savedTrials = new List<float>();
        savedTrialsUI = new List<string>();
        totalDegreesToTarget = 0;
        avgDegreesToTarget = 0;

        task2UI.SetActive(true);
        dialogBox.OpenDialogBox(gameMngr.taskData.task2Data.instructions.start, "trial");
        playerCtrlr.player.GetComponent<FirstPersonMovement>().freezeMovement = true;
        StartSavingData();
    }

    public void EndTask()
    {
        Debug.Log("End of task 2");
        //gameMngr.EndTask();
        dialogBox.OpenDialogBox(gameMngr.taskData.task3Data.instructions.end, "menu");
        task2UI.SetActive(false);
        playerCtrlr.player.GetComponent<FirstPersonMovement>().freezeMovement = false;

        gameMngr.endTime = Time.time;
        gameMngr.taskStarted = false;
        gameMngr.taskEnded = true;
        gameMngr.taskNb = 0;
        //Cursor.lockState = CursorLockMode.None;


        gameMngr.visor.SetActive(false);
        StopSavingData();
    }

    public void StartTrial()
    {
        string instructions;
        string image;

        Debug.Log("StartTrial");
        gameMngr.taskPaused = false;
        
        if (trialNb < maxTrial)    //if there are trials left
        {
            //Cursor.lockState = CursorLockMode.Locked;
            trialNb++;
            //TODO: Make visor visible
            //Debug.Log("Task 1 - Trial #: " + trialNb + " / " + maxTrial);

            //targetNb = 1; //Set the target object to the first POI
            startHotspot = allStartHotspots[trialNb - 1];
            playerCtrlr.GotoHotspot(startHotspot);
            difficulty = gameMngr.taskData.task2Data.task2Trials[trialNb - 1].difficulty;

            SetTargetObj();
            CalculateDegreesToTarget();
            instructions = gameMngr.taskData.task2Data.instructions.attempts[0] + targetLocationName + gameMngr.taskData.task2Data.instructions.attempts[1];
            image = gameMngr.taskData.task1Data.locations[targetLocationIndex].filename;
            dialogBox.OpenDialogBoxImg(instructions, image, "none");
        }
        else // notrials left
        {

            EndTask();
        }
    }

    private void SetTargetObj()
    {

        //Debug.Log("SetTargetObj");
        startLocationIndex = gameMngr.taskData.task2Data.task2Trials[trialNb - 1].startLocationID - 1;
        targetLocationIndex = gameMngr.taskData.task2Data.task2Trials[trialNb - 1].targetLocationID-1; //Index of target location for current trial
        Debug.Log("Target location index is " + targetLocationIndex);

        //Debug.Log("TargetObj() index: " + targetLocationIndex + " / " + maxTargetObj);
        targetLocation = gameMngr.POI[targetLocationIndex]; //Set target location index

        targetLocationName = gameMngr.taskData.task1Data.locations[targetLocationIndex].pronoun + " " + gameMngr.taskData.task1Data.locations[targetLocationIndex].name;

        if (startLocationIndex == -1)
            startLocationName = "n/a";
        else
            startLocationName = gameMngr.taskData.task1Data.locations[startLocationIndex].pronoun + " " + gameMngr.taskData.task1Data.locations[startLocationIndex].name;

    }

    public void OnValidation()
    {
        Debug.Log("On Validation");

        // Save trial data
        SaveData();
        totalDegreesToTarget += degreesToTarget;    //Sum
        avgDegreesToTarget = totalDegreesToTarget / trialNb;
        savedTrials.Add(degreesToTarget);
        savedTrialsUI.Add(degreesToTarget.ToString("F2"));

        StartTrial(); // start the next trial

    }

    // Freeze player's movement and rotation, then calculates the angle to the active target location
    private void CalculateDegreesToTarget()
    {
        //gameMngr.taskPaused = true;
        playerCtrlr.calculatingRotation = true;
        playerCtrlr.rotationCheck.transform.LookAt(targetLocation.transform);
        correctRotationToTarget = playerCtrlr.rotationCheck.transform.rotation.eulerAngles.y;
        Debug.Log("correctionRotationToTarget = " + correctRotationToTarget);
        //gameMngr.taskPaused = false;
        playerCtrlr.calculatingRotation = false;


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
        string trialHeader = "trialNb" + delimiter + "difficulty" + "startLocationIndex" + delimiter + "startLocation" + delimiter + "targetLocationIndex" + delimiter + "targetLocation";
        string playerHeader = "playerRotation" + delimiter +  "startRotation" + delimiter + "correctRotationToTarget" + delimiter + "degreesToTarget" + delimiter + "distanceToTarget";
        string calculationsHeader = "trialsDegreeToTarget" + delimiter + "totalDegreesToTarget" + delimiter + "avgDegreestToTarget";

        return sessionHeader + delimiter + trialHeader + delimiter + playerHeader + delimiter + calculationsHeader;
    }

    private void SaveData()
    {
        string sessionData = dateTime + delimiter + gameMngr.taskTime.ToString(@"mm\:ss") +  delimiter + gameMngr.participantID + delimiter + gameMngr.taskNb;
        string trialData = ""+ trialNb + delimiter + difficulty +  startLocationIndex + delimiter + startLocationName + delimiter + targetLocationIndex + delimiter + targetLocationName;
        string playerData = gameMngr.playerRot[0].ToString("F2") + delimiter + "starRotation" + delimiter + correctRotationToTarget + delimiter + degreesToTarget + delimiter + distanceToTarget;
        string calculationsData = "" + degreesToTarget + delimiter + totalDegreesToTarget + delimiter + avgDegreesToTarget;

        sw.WriteLine(sessionData + delimiter  + trialData + delimiter + playerData + delimiter + calculationsData);
    }

    public void StopSavingData()
    {
        sw.Close();
    }

    private void UpdateUI()
    {
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial + " (" + difficulty +")";
        startTargetTxt.text = "Start: "  + startLocationName + " - Target: " + targetLocationName;
        playerRotationTxt.text = "Player rot: " + gameMngr.playerRot[0].ToString("F2");
        distanceToTargetTxt.text = "Distance to target: " + distanceToTarget.ToString("F2");
        angleToTargetTxt.text = "Degrees to target: " + degreesToTarget.ToString("F2");
        savedTrialsTxt.text = "Recorded degrees to target: " + String.Join(",", savedTrialsUI);
        avgDegreesToTargetTxt.text = "Degrees to target (avg): " + avgDegreesToTarget.ToString("F2");

    }

}
