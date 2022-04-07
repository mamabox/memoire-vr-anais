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

    private int targetObjIndex;
    private int trialNb;
    private int maxTrial;
    private int maxTargetObj;

    

    private List<string> trialStart = new List<string> { "N", "E", "S", "W" };
    private List<string> routeN = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };
    private List<string> routeE = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };
    private List<string> routeS = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };
    private List<string> routeW = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6" };

    private List<string> instructions = new List<string> { "« Vous allez parcourir cette ville à l’aide de flèches au sol. Vous verrez une image apparaître devant vous qui vous indiquera le lieu que vous allez rejoindre grâce aux flèches. Une fois que vous aurez atteint le premier lieu, vous cliquerez sur une pancarte et une nouvelle image de lieu apparaîtra. Vous rejoindrez ce lieu en suivant de nouvelles flèches. Vous allez faire cela pour 6 lieux différents. Une fois que vous aurez vu les 6 lieux, vous apparaîtrez à un nouveau point de la ville et vous recommencerez la même chose. La consigne vous sera rappelez. Lorsque vous suivrez les chemins, soyez attentif à votre environnement et regardez bien autour de vous. Pour début l’expérience, cliquer sur X ».", "Il faut rejoindre ce lieu", "Tâche terminée" };

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
        trialTxt.text = "Trial: " + trialNb + " / " + maxTrial;
        targetTxt.text = "Target: " + (targetObjIndex + 1) + " / " + maxTargetObj;
        angleToTargetTxt.text = "AngleToTarget: " + angleToTarget;
        
    }

    // Configure settings for this task
    public void SetupTask()
    {
        gameMngr.taskNb = 1;
        trialNb = 0;
        maxTrial = 4;
        maxTargetObj = 6;
        dialogBox.OpenDialogBox("First instructions");
        routeMngr.SpawnLine(routeN, 1);
        task1UI.SetActive(true);
        //gameMngr.dialogBox.SetActive(true);
        

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
        task1UI.SetActive(false);
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
