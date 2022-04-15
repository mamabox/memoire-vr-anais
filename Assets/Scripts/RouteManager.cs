using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RouteManager : MonoBehaviour
{
    public GameObject routeIndicatorPrefab;
    public List<Material> lineMaterials;
    public GameObject lineDrawn;
    public List<GameObject> task1Routes;
    private GameManager gameMgr;


    void Awake()
    {
        gameMgr = GameObject.FindObjectOfType <GameManager>().GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Renders a line with 3 parameters: coordinates, material index for color and the name of the route
    public void SpawnLine(List<string> route, int material, string name)
    {
        int _hotspotIndex;
        Vector3 _vectorToDraw;
        GameObject newLineGen = Instantiate(routeIndicatorPrefab);

        // Select material
        if (material < lineMaterials.Count)
            newLineGen.GetComponent<Renderer>().material = lineMaterials[material];
        else
            newLineGen.GetComponent<Renderer>().material = lineMaterials[0];

        //Animate the line texture
        newLineGen.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(.5f, .5f);

        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        //List<string> lineToDraw = new List<string>(route);
        //lineToDraw.RemoveAt(0); //remove the starting position
        lRend.positionCount = route.Count();    //set length of line renderer to the number of coordinates on the path 
        
        for (int i = 0; i < route.Count(); i++)
        {
            /*string[] _coord = lineToDraw[i].Split(char.Parse(xyCoordSeparator));
            //Debug.Log("Draw at (" + string.Join(", ", _coord) + ")");
            lRend.SetPosition(i, new Vector3(float.Parse(_coord[0]) * gameManager.blockSize, 0.01f, float.Parse(_coord[1]) * gameManager.blockSize));
            */
            //Debug.Log("Line to draw: # " + i + ": " + route[i] + " is hotspot[" + gameMgr.ReturnHotspotIndex(route[i])+"]");
            _hotspotIndex = gameMgr.ReturnHotspotIndex(route[i]);
            _vectorToDraw = new Vector3(gameMgr.routeHotspots[_hotspotIndex].GetComponent<Hotspot>().coord[0], .3f, gameMgr.routeHotspots[_hotspotIndex].GetComponent<Hotspot>().coord[1]);
            lRend.SetPosition(i,_vectorToDraw);
        }

        lineDrawn = newLineGen; //Object of the line drawn
        task1Routes.Add(lineDrawn);
        lineDrawn.name = "Task1 - Route " + name;
        lineDrawn.SetActive(false);
        
    }
}
