using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class TaskDataImport : MonoBehaviour
{

    private string taskDataFile;
    private string importPath;
    private GameManager gameManager;
    private TaskData tempTaskData = new TaskData();
    private Task1Manager task1Manager;
    private int totalTaskNb;

    void Awake()
    {
        //Initialisation

        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //taskManager = gameManager.GetComponent<SequenceManager>();
        importPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Data/Import/");
        taskDataFile = importPath + "TaskData.json";

        totalTaskNb = 3;

        ImportTaskData(taskDataFile);

    }


    [ContextMenu("Import Scenario Data")]
    public void ImportTaskData(string dataFile)
    {
        if (File.Exists(dataFile))
        {
            Debug.Log(taskDataFile + " exists");
            string fileContents = File.ReadAllText(dataFile);                   // Read the entire file and save its contents.
            gameManager.taskData = JsonUtility.FromJson<TaskData>(fileContents);                // Deserialize the JSON data into a pattern matching the GameData class.
            BuildMenuItems();

        }
        else
        {
            Debug.Log(taskDataFile + " does not exist");
        }

        //scenarioManager.scenario7Data = scenarioManager.ImportScenarioStdDataJson(7);
    }


    private void BuildMenuItems()
    {
        string[] dropdownMenuItems = new string[totalTaskNb];


        dropdownMenuItems[0] = gameManager.taskData.task1Data.dropdownMenuText; //Add description to menu list
        dropdownMenuItems[1] = gameManager.taskData.task2Data.dropdownMenuText; //Add description to menu list
        dropdownMenuItems[2] = gameManager.taskData.task3Data.dropdownMenuText; //Add description to menu list

        for (int x = 0; x < totalTaskNb; x++)
        {
            gameManager.TaskBtnLabels[x].text = dropdownMenuItems[x];
        }

        //Debug.Log("Dropdown menu items: " + String.Join(",", dropdownMenuItems));

    }

}
