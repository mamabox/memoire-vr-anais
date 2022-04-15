using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogBox : MonoBehaviour
{

    private GameManager gameManager;
    public TextMeshProUGUI instructionsImg;
    public TextMeshProUGUI instructions;
    public GameObject instructionsWithImg;
    public Button dialogBoxBtn;
    public RawImage image;
    private string imagePath;
    private Texture2D myTexture;
    //private string fileName;
    private byte[] bytes;
    private string dialogBoxMode;

    private GameManager gameMngr;


    private void Awake()
    {
        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Media/Images/");
        gameMngr = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        dialogBoxMode = "not set";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadTaskInstructions()
    {
        if (!gameMngr.readTaskInstructions) // if the first instruction have not been read
        {
            gameMngr.readTaskInstructions = true;
        }

    } 

    public void OpenDialogBox(string text, string mode)
    {
        this.gameObject.SetActive(true);
        gameMngr.taskPaused = true;
        Cursor.lockState = CursorLockMode.None;
        
        //Debug.Log("Open a dialog box with text: " + text);
        instructionsWithImg.SetActive(false);
        instructions.gameObject.SetActive(true);
        instructions.text = text.Replace("|", System.Environment.NewLine);
        dialogBoxMode = mode;
        
    }

    public void OpenDialogBoxImg(string text, string image, string mode)
    {
        this.gameObject.SetActive(true);
        gameMngr.taskPaused = true;
        Cursor.lockState = CursorLockMode.None;
        
        //Debug.Log("Open a dialog box with image (" + image + ") and text: " + text);
        instructionsWithImg.SetActive(true);
        instructions.gameObject.SetActive(false);
        string fileName = image + ".png";
        Debug.Log("Filename: " + fileName);
        instructionsImg.text = text.Replace("|", System.Environment.NewLine);
        addTexture(fileName);   // Add the image to dialog box
        dialogBoxMode = mode;
    }

    public void CloseDialogBox()
    {
        Debug.Log("Close dialog box");
        Cursor.lockState = CursorLockMode.Locked;
        gameMngr.taskPaused = false;
        if (dialogBoxMode == "task")
        {
            gameMngr.StartTask(gameManager.taskNb);
        }
        else if ( dialogBoxMode == "trial")
        {
            gameMngr.StartTrial();
        }
        else if (dialogBoxMode ==  "menu")
        {
            gameMngr.OpenMenu();
        }
        else
            Debug.Log("Mode not reconized");

    }

    void addTexture(string fileName)
    {
        myTexture = new Texture2D(400, 400);
        bytes = File.ReadAllBytes(Path.Combine(imagePath, fileName));
        myTexture.LoadImage(bytes);
        myTexture.name = fileName;
        image.GetComponent<RawImage>().texture = myTexture;
    }
}
