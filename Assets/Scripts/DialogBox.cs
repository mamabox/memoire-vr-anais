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
    public TextMeshProUGUI instructions;
    public Button dialogBoxBtn;
    public RawImage image;
    private string imagePath;
    private Texture2D myTexture;
    //private string fileName;
    private byte[] bytes;


    private void Awake()
    {
        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Media/Images/");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDialogBox(string text, string image)
    {
        Debug.Log("Open a dialog box with image (" + image + ") and text: " + text);
        string fileName = image + ".png";
        instructions.text = text;
        addTexture(fileName);   // Add the image to dialog box
    }

    public void OnButtonClick()
    {

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
