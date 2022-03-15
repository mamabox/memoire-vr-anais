using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> cardDir;
    public List<GameObject> POI;

    private PlayerController playerController;
    private GameObject player;


    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        player = playerController.player;
    }

    // Start is called before the first frame update
    void Start()
    {

        playerController.GotoCoord(cardDir[0].GetComponent<Hotspot>().coord);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
