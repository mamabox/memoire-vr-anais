using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Task3Manager : MonoBehaviour
{
    private float degreesToTarget;    //Correct angle to target
    private float correctRotationToTarget;
    private List<float> savedTrials = new List<float>();
    private List<string> savedTrialsUI = new List<string>();

    private GameManager gameMngr;

    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private GameObject targetLocation;

    private PlayerController2 playerCtrlr;

    private DialogBox dialogBox;

    //public TextMeshProUGUI TaskText;
    public GameObject task3UI;
    public TextMeshProUGUI trialTxt;
    public TextMeshProUGUI targetTxt;
    public TextMeshProUGUI distanceToTargetTxt;
    public TextMeshProUGUI playerRotationTxt;
    public TextMeshProUGUI angleToTargetTxt;
    public TextMeshProUGUI savedTrialsTxt;
    public TextMeshProUGUI avgDegreesToTargetTxt;

    private int targetLocationIndex;
    private string targetLocationName;
    private int trialNb;    // 1 to 4
    private int targetNb; // 1 to 6
    private int maxTrial; // how many routes
    private int maxTargetObj;   // 6
    private float distanceToTarget;   // how far the player is to the target object
    private float totalDegreesToTarget;    // Sum of rotation error for all trials
    private float avgDegreesToTarget; // Avg rotation error, updated after each trial

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
        if (gameMngr.taskStarted && gameMngr.taskNb == 3 && trialNb > 0)    //Update UI
        {
            distanceToTarget = Vector3.Distance(playerCtrlr.player.transform.position, targetLocation.transform.position); //update the distance only if a trial has started
            degreesToTarget = Math.Abs(correctRotationToTarget - gameMngr.playerRot[0]);
            UpdateUI();
        }
    }

    private void SetupTask()
    {
        maxTrial = gameMngr.taskData.task3Data.task3Trials.Count();
        task3UI.SetActive(false);
        startHotspot = gameMngr.cardDir[4]; //Set start hotspot to the centre
    }

    public void StartTask()
    {
        Debug.Log("Task 3 - Start()");
        gameMngr.taskNb = 3;
        task3UI.SetActive(true);
        trialNb = 0;
        savedTrials = new List<float>();
        savedTrialsUI = new List<string>();
        dialogBox.OpenDialogBox(gameMngr.taskData.task3Data.instructions.start, "trial");
        playerCtrlr.calculatingRotation = true;

    }



    public void EndTask()
    {
        Debug.Log("End of task 3");
        dialogBox.OpenDialogBox(gameMngr.taskData.task3Data.instructions.end, "menu");
        task3UI.SetActive(false);

        gameMngr.endTime = Time.time;
        gameMngr.taskStarted = false;
        gameMngr.taskEnded = true;
        gameMngr.taskNb = 0;

        gameMngr.visor.SetActive(false);
    }

    public void StartTrial()
    {
        string instructions;
        string image;

        gameMngr.taskPaused = false;
        Debug.Log("StartTrial");
        if (trialNb < maxTrial)    //if there are trials left
        {
            trialNb++;
            //TODO: Make visor visible

            targetNb = 1; //Set the target object to the first POI
            maxTargetObj = gameMngr.taskData.task3Data.task3Trials[trialNb - 1].targetLocations.Count();
            playerCtrlr.GotoHotspot(startHotspot);
            SetTargetObj();
            instructions = gameMngr.taskData.task3Data.instructions.attempts[0] + gameMngr.taskData.task3Data.instructions.attempts[1] + " " + targetLocationName + gameMngr.taskData.task3Data.instructions.attempts[2];
            image = gameMngr.taskData.task1Data.locations[targetLocationIndex].filename;
            dialogBox.OpenDialogBoxImg(instructions, image, "none");


        }
        else // notrials left
        {
            EndTask();
        }

    }

    void SetTargetObj()
    {
        targetLocationIndex = gameMngr.taskData.task3Data.task3Trials[trialNb - 1].targetLocations[targetNb - 1] - 1; //Index of target location for current trial
        Debug.Log("Target location index is " + targetLocationIndex);
        //Debug.Log("TargetObj() index: " + targetLocationIndex + " / " + maxTargetObj);
        targetLocation = gameMngr.POI[targetLocationIndex];
        targetLocationName = gameMngr.taskData.task1Data.locations[targetLocationIndex].pronoun + " " + gameMngr.taskData.task1Data.locations[targetLocationIndex].name;
    }

    public void OnValidation()
    {
        string instructions;
        string image;

        //Debug.Log("Inside Task1 OnValidation()");
        if (targetNb < maxTargetObj) // if there are target objects left in this trial
        {
            CalculateDegreeToTarget();
            // Save trial data
            totalDegreesToTarget += degreesToTarget;    //Sum
            avgDegreesToTarget = totalDegreesToTarget / trialNb;
            savedTrials.Add(degreesToTarget);
            savedTrialsUI.Add(degreesToTarget.ToString("F2"));
            targetNb++;
            SetTargetObj();
            instructions = gameMngr.taskData.task3Data.instructions.attempts[1] + " " + targetLocationName + gameMngr.taskData.task3Data.instructions.attempts[2];
            image = gameMngr.taskData.task1Data.locations[targetLocationIndex].filename;
            dialogBox.OpenDialogBoxImg(instructions, image, "none");
        }
        else
        {
            StartTrial(); // start the next trial
        }

    }
    private void CalculateDegreeToTarget()
    {
        playerCtrlr.rotationCheck.transform.LookAt(targetLocation.transform);
        correctRotationToTarget = playerCtrlr.rotationCheck.transform.rotation.eulerAngles.y;
        degreesToTarget = Math.Abs(correctRotationToTarget - gameMngr.playerRot[0]);
    }

    private void UpdateUI()
    {
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial;
        targetTxt.text = "Target: " + targetNb + " / " + maxTargetObj + ": " + targetLocationName;
        //playerRotationTxt.text = "temp";
        playerRotationTxt.text = "Player rot: " + gameMngr.playerRot[0].ToString("F2");
        distanceToTargetTxt.text = "Distance to target: " + distanceToTarget.ToString("F2");
        angleToTargetTxt.text = "Degrees to target: " + degreesToTarget.ToString("F2");
        savedTrialsTxt.text = "Recorded degrees to target: " + String.Join(",", savedTrialsUI);
        avgDegreesToTargetTxt.text = "Degrees to target (avg): " + avgDegreesToTarget.ToString("F2");
    }
}