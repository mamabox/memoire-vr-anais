using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

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
    
    private PlayerController playerCtrlr;

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

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerCtrlr = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
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
    }

    public void EndTask()
    {
        Debug.Log("End of task 1");
        gameMngr.EndTask();
        dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.end, "menu");
        task2UI.SetActive(false);
        playerCtrlr.player.GetComponent<FirstPersonMovement>().freezeMovement = false;
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

            SetTargetObj();
            CalculateDegreesToTarget();
            instructions = gameMngr.taskData.task1Data.instructions.attempts[0] + targetLocationName + gameMngr.taskData.task1Data.instructions.attempts[1];
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
        totalDegreesToTarget += degreesToTarget;    //Sum
        avgDegreesToTarget = totalDegreesToTarget / trialNb;
        savedTrials.Add(degreesToTarget);
        savedTrialsUI.Add(degreesToTarget.ToString("F2"));

        StartTrial(); // start the next trial

    }

    // Freeze player's movement and rotation, then calculates the angle to the active target location
    private void CalculateDegreesToTarget()
    {
        gameMngr.taskPaused = true;
        playerCtrlr.playerCam.transform.LookAt(targetLocation.transform);
        correctRotationToTarget = playerCtrlr.playerCam.transform.rotation.eulerAngles.y;
        gameMngr.taskPaused = false;
        

    }

    private void SaveData()
    {

    }

    private void UpdateUI()
    {
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial + " (" + gameMngr.taskData.task2Data.task2Trials[trialNb-1].difficulty +")";
        startTargetTxt.text = "Start: "  + startLocationName + " - Target: " + targetLocationName;
        playerRotationTxt.text = "Player rot: " + gameMngr.playerRot[0].ToString("F2");
        distanceToTargetTxt.text = "Distance to target: " + distanceToTarget.ToString("F2");
        angleToTargetTxt.text = "Degrees to target: " + degreesToTarget.ToString("F2");
        savedTrialsTxt.text = "Recorded degrees to target: " + String.Join(",", savedTrialsUI);
        avgDegreesToTargetTxt.text = "Degrees to target (avg): " + avgDegreesToTarget.ToString("F2");

    }
}
