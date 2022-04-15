using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Task2Manager : MonoBehaviour
{
    private float degreesToTarget;    //Correct angle to target
    private float correctRotationToTarget;
    private List<float> savedTrials = new List<float>();
    private List<string> savedTrialsUI = new List<string>();

    private GameManager gameMngr;
    private GameObject targetLocation;
    private GameObject startLocation;
    private PlayerController playerCtrlr;
    private GameObject startHotspot;    // Where the player starts at the beginning of the trial
    private DialogBox dialogBox;

    //public TextMeshProUGUI TaskText;
    public GameObject task2UI;
    public TextMeshProUGUI trialTxt;
    public TextMeshProUGUI targetTxt;
    public TextMeshProUGUI distanceToTargetTxt;
    public TextMeshProUGUI playerRotationTxt;
    public TextMeshProUGUI angleToTargetTxt;
    public TextMeshProUGUI savedTrialsTxt;

    private int targetLocationIndex;
    private string targetLocationName;
    private int startLocationIndex;
    private string startLocationName;
    private int trialNb;    // 1 to 4
    private int targetNb; // 1 to 6
    private int maxTrial; // how many routes
    private int maxTargetObj;   // 6
    private float distanceToTarget;   // how far the player is to the target object

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
        
    }

    // Configure settings for this task
    public void SetupTask()
    {
        //gameMngr.taskNb = 2;
        //trialNb = 0;
        //targetNb = 0;
        //gameMngr.taskPaused = true;
        //task1UI.SetActive(true);
        //dialogBox.OpenDialogBox(gameMngr.taskData.task1Data.instructions.start, "task");

        maxTrial = gameMngr.taskData.task2Data.task2Trials.Count();
        //maxTargetObj = gameMngr.taskData.task2Data.locations.Count();

        //routeMngr.SpawnLine(routeN, 1);
        //DrawRoutes();

        //task2UI.SetActive(false);
        //task3UI.SetActive(false);
        //gameMngr.dialogBox.SetActive(true);

    }
}
