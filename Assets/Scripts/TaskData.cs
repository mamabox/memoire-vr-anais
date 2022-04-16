using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TaskData
{
    public FirstPersonSettings firstPersonSettings;
    public Task1Data task1Data;
    public Task2Data task2Data;
    public Task3Data task3Data;

}


[System.Serializable]
public class FirstPersonSettings
{

    public float playerSpeed;
    public float lookSensitivity;
    public float lookSmoothing;
}


[System.Serializable]
public class Task1Data
{
    public int scenarioID;
    public string description;
    public string dropdownMenuText;
    public Instructions instructions;
    public float visorSize;
    public List<Location> locations;
    public bool randomTrialsOrder;
    //public List<Trial> trials;
    public List<Task1Trial> task1Trials;
}

[System.Serializable]
public class Task2Data
{
    public int scenarioID;
    public string description;
    public string dropdownMenuText;
    public Instructions instructions;
    public List<Task2Trial> task2Trials;
}

[System.Serializable]
public class Task3Data
{
    public int scenarioID;
    public string description;
    public string dropdownMenuText;
    public Instructions instructions;
}

[System.Serializable]
public class Instructions
{
    public string start;
    public string end;
    public List<string> attempts;
}

[System.Serializable]
public class Trial
{
    public string position;
    public int startObj;
    public int targetObj;

}

[System.Serializable]
public class Task1Trial
{
    public string routeName;
    public List<string> routeCoord;
    public string startOrientation;
    public List<int> targetLocations;
}

[System.Serializable]
public class Task2Trial
{
    public string difficulty;
    public int startLocationID;
    public int targetLocationID;
    public string startOrientation;
}

[System.Serializable]
public class Location
{
    public string locationID;
    public string name;
    public string pronoun;
    public string filename;
    //public string nearbyIntersection;
}
