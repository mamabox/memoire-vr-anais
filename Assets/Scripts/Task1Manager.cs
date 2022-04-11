using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

/* 4 trials: start at N,E, S, W */

public class Task1Manager : MonoBehaviour
{
    private float angleErrorMargin;  //how many degrees the player can be off for validation
    private float angleToTarget;    //Correct angle to target
    private float angleToValidate;
    private float playerAngle;

    private GameManager gameMngr;
    private GameObject targetObj;
    private PlayerController playerCtrlr;
    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private RouteManager routeMngr;
    private DialogBox dialogBox;

    //public TextMeshProUGUI TaskText;
    public GameObject task1UI;
    public TextMeshProUGUI trialTxt;
    public TextMeshProUGUI targetTxt;
    public TextMeshProUGUI angleToTargetTxt;
    public TextMeshProUGUI playerRotation;

    private int targetLocationIndex;
    private string targetLocationName;
    private string routeName;
    private int trialNb;    // 1 to 4
    private int targetNb; // 1 to 6
    private int maxTrial;
    private int maxTargetObj;

    

    private List<string> trialStart = new List<string> { "N", "E", "S", "W" };
    private List<string> routeN = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };


    private List<Hotspot> routeNObj;

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerCtrlr = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        routeMngr = GameObject.Find("RouteManager").GetComponent<RouteManager>();
        dialogBox = FindObjectOfType<GameManager>().dialogBox.GetComponent<DialogBox>();
    }


    // Start is called before the first frame update
    public void StartTask()
    {
        Debug.Log("TASK 1 START");
        //gameMngr.taskNb = 1;
        //SetupTask();
        task1UI.SetActive(true);
        StartTrial();

    }

    // Update is called once per frame
    void Update()
    {
       if (gameMngr.taskStarted && gameMngr.taskNb == 1)    //Update UI
        {
            UpdateUI();
        }

    }

    private void UpdateUI()
    {
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial + ": "+ routeName;
        targetTxt.text = "Target: " + targetNb + " / " + maxTargetObj + ": " + targetLocationName;
        angleToTargetTxt.text = "AngleToTarget: " + angleToTarget;
        
    }

    // Configure settings for this task
    public void SetupTask()
    {
        gameMngr.taskNb = 1;
        trialNb = 0;
        //targetNb = 0;
        maxTrial = gameMngr.taskData.task1Data.task1Trials.Count();
        maxTargetObj = gameMngr.taskData.task1Data.locations.Count();
        dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.start);
        //routeMngr.SpawnLine(routeN, 1);
        DrawRoutes();
        task1UI.SetActive(true);
        //task2UI.SetActive(false);
        //task3UI.SetActive(false);
        //gameMngr.dialogBox.SetActive(true);

    }
    public void SaveData()
    {

    }

    private void DrawRoutes()
    {
        for (int x = 0; x < maxTrial; x++)
        {
            routeMngr.SpawnLine(gameMngr.taskData.task1Data.task1Trials[x].routeCoord, 1);
        }
    }


    // Begin the task
    public void StartTrial()
    {
        //dialogBox.OpenDialogBoxImg("this is a test", "none");
        if (trialNb < maxTrial)    //if there are trials left
        {
     
            trialNb++;
            //TODO: Make visor visible
            Debug.Log("Task 1 - Trial #: " + trialNb + " / " + maxTrial);
            //SetRoute()
            routeName = gameMngr.taskData.task1Data.task1Trials[trialNb-1].routeName;
            if (trialNb >= 1)    //if this isn't the first trial, hide the previous routeDrawn
                routeMngr.task1Routes[trialNb-1].SetActive(false);
            routeMngr.task1Routes[trialNb-1].SetActive(true);

            targetNb = 1; //Set the target object to the first POI
            startHotspot = gameMngr.cardDir[trialNb-1];
            playerCtrlr.GotoHotspot(startHotspot);
            SetTargetObj();
        }
        else // notrials left
        {
            EndTask();
        }

    }

    void SetTargetObj()
    {
        Debug.Log("SetTargetObj");
        targetLocationIndex = gameMngr.taskData.task1Data.task1Trials[trialNb-1].targetLocations[targetNb-1]-1;
        Debug.Log("TargetObj() index: " + targetLocationIndex + " / " + maxTargetObj);
        targetObj = gameMngr.POI[targetLocationIndex];
        targetLocationName = gameMngr.taskData.task1Data.locations[targetLocationIndex].name;
        
    }

    void EndTask()
    {
        task1UI.SetActive(false);
        gameMngr.EndTask();
        Debug.Log("End of task 1");
    }

    public void OnValidation()
    {
        if (true)//TODO: Check if pointing to target obj
        {
            //Debug.Log("Inside Task1 OnValidation()");
            if (targetNb < maxTargetObj) // if there are target objects left in this trial
            {
                targetNb++;
                SetTargetObj();
            }
            else
            {
                StartTrial(); // start the next trial
            }
        }
        else
        {
            Debug.Log("Not pointing at target. Angle to target: " + angleToTarget + ",  angle to validate: " + angleToValidate);
        }
    }
}
