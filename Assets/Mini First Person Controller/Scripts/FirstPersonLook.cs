using UnityEngine;

/** Added line 32 and 47 (FMD) **/

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    public Vector2 velocity; // (FMD)
    Vector2 frameVelocity;

    private PlayerController2 playerController;
    private GameManager gameMngr;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController2>();
        gameMngr = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        //Cursor.lockState = CursorLockMode.Locked; //Moved to GameManager.cs (FMD)
        //velocity.x = playerController.startOrientation; // Initialise player to start orientation set for Task 1 in PlayerController.Awake() (FMD)
    }

    void Update()
    {

        //Vector2 mouseDelta;
        Vector2 rawFrameVelocity;

        if (gameMngr.taskStarted && !gameMngr.taskPaused)
        {
            // Get smooth velocity.
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        }
        else
        {
            //mouseDelta = new Vector2(0, 0);
            rawFrameVelocity = new Vector2(0, 0);
        }


        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        //Debug.Log("FirstPersonLook() > Update: transform rotation = " + -velocity.x); // (FMD)
    }

    void UpdateOld()
    {

        if (gameMngr.taskStarted && !gameMngr.taskPaused)
        {
            // Get smooth velocity.
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);


            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Rotate camera up-down and controller left-right from velocity.
            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
            //Debug.Log("FirstPersonLook() > Update: transform rotation = " + -velocity.x); // (FMD)
        }
    }
}
