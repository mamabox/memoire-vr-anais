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

    private GameManager gameMngr;


    private void Awake()
    {
        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Media/Images/");
        gameMngr = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
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

    public void OpenDialogBox(string text)
    {
        this.gameObject.SetActive(true);
        //Debug.Log("Open a dialog box with text: " + text);
        instructionsWithImg.SetActive(false);
        instructions.gameObject.SetActive(true);
        instructions.text = text;
    }

    public void OpenDialogBoxImg(string text, string image)
    {
        this.gameObject.SetActive(true);
        //Debug.Log("Open a dialog box with image (" + image + ") and text: " + text);
        instructionsWithImg.SetActive(true);
        instructions.gameObject.SetActive(false);
        string fileName = image + ".png";
        Debug.Log("Filename: " + fileName);
        instructions.text = text;
        addTexture(fileName);   // Add the image to dialog box
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
