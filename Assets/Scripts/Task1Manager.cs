using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* 4 trials: start at N,E, S, W */

public class Task1Manager : MonoBehaviour
{
    private float angleErrorMargin;  //how mau
    private float angleToTarget;
    private float angleToValidate;

    private GameManager gameMngr;
    private GameObject targetObj;
    private PlayerController playerCtrlr;
    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private RouteManager routeMngr;

    private int targetObjIndex;
    private int trialNb;
    private int maxTrial;
    private int maxTargetObj;

    private List<string> routeN = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };
    private List<Hotspot> routeNObj;

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerCtrlr = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        routeMngr = GameObject.Find("RouteManager").GetComponent<RouteManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Configure settings for this task
    public void SetupTask()
    {
        trialNb = 0;
        maxTrial = 4;
        maxTargetObj = 6;
        StartTrial();
        routeMngr.SpawnLine(routeN, 1);

    }

    // Begin the task
    void StartTrial()
    {
        if (trialNb < maxTrial)    //if there are trials left
        {
            trialNb++;
            //TODO: Make visor visible
            Debug.Log("Task 1 - Trial #: " + trialNb + " / " + maxTrial);
            targetObjIndex = 0; //Set the target object to the first POI
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
        Debug.Log("TargetObj() index: " + (targetObjIndex +1) + " / " + maxTargetObj);
        targetObj = gameMngr.POI[targetObjIndex];
        
    }

    void EndTask()
    {

        Debug.Log("End of task 1");
    }

    public void OnValidation()
    {
        if (true)//TODO: Check if pointing to target obj
        {
            //Debug.Log("Inside Task1 OnValidation()");
            if (targetObjIndex < maxTargetObj-1) // if there are target objects left in this trial
            {
                targetObjIndex++;
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
