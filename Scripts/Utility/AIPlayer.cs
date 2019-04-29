using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UnityEngine;

// Struct for the mouse pointer x and y
public struct Point
{
    public int x;
    public int y;
}

public class AIPlayer : MonoBehaviour
{

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point pos);

    public static AIPlayer aiPlayer; // So this can be easily refrenced by other scripts mostly duplicates of this script

    [Tooltip("Current time the mouse button is held down for in seconds")]
    [SerializeField] private float holdTime = 0.7f; // Current time the mouse button is held down for in seconds

    [Tooltip("Minimum distance we want to move for it to be worth storing")]
    [SerializeField] private float goodDistanceToMove = 1500.0f; // Minimum distance we want to move for it to be worth storing

    [Tooltip("Wait time between swings")]
    [SerializeField] private float timeBetweenSwings = 0.8f; // Wait time between swings

    [Tooltip("Distance of the cameras nearclip")]
    [SerializeField] private float camNearClipDistance = 70.0f; // Distance of the cameras nearclip

    [Tooltip("Maximum distance the arm can grab")]
    [SerializeField] private float maxArmDistance = 11; // Max reach of the arm

    [Tooltip("Min distance that the AI will bother to grab")]
    [SerializeField] private float minHitDistance = 4; // Min distance that the AI will bother to grab

    [Tooltip("Add all gateways in order")]
    [SerializeField] private Transform[] gateList; // Put the points from unity interface

    [Tooltip("If not clicking to frop down this is the length of time to wait")]
    [SerializeField] private float noClickWaitTime; // If not clicking to frop down this is the length of time to wait

    private Camera cam; // Stores the camera
    private GameObject playerBody; // GameObject that holds the players body

    private Vector3 gateLocation; // Location of the next goal
    private Vector3 bodyPosition; // Position of the players body
    private Vector3 gateDistance; // Distance between the next gate and players body
    private Vector3 worldClickPosition; // Position in world space to click
    private Vector2 screenClickPosition; // Pixel Position of where to click on screen

    private int currentGate = 0; //Index of current waypoint
    private bool gateTotheRight; // True if the exit is to the right
    private bool isRunning = false; // If the AI is running
    private bool stabliseMouse = false; // If the mouse position should be locked to latest worldPosition
    private bool newClickLocationFound = false; // Determines if the click location is new, if it is then we click

    private void Awake()
    {
        // Assign the player body
        playerBody = GameObject.Find("Octopus Body");

        // Set initial click position to the player location to avoid odd initial clicks
        worldClickPosition = playerBody.transform.position;

        // if not here yet make this the aiplayer. Stops there from being multiple instances of this script
        if (aiPlayer == null)
        {
            DontDestroyOnLoad(gameObject);
            aiPlayer = this;
        }
        else if (aiPlayer != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Locate and store the camera
        cam = Camera.main;

        // Locate the first gate used as the goal
        gateLocation = gateList[currentGate].transform.position;
    }

    void Update()
    {
        // Calculate the distance from exit every update
        DistanceFromGate();

        // Keeps the mouse in posistion
        if (stabliseMouse)
        {
            TranslateMousePos();
            SetCursorPos(Mathf.RoundToInt(screenClickPosition.x), Mathf.RoundToInt(screenClickPosition.y));
        }

        // Stop the AI
        if (Input.GetKeyDown("s"))
        {
            isRunning = false;
            StopAllCoroutines();
            stabliseMouse = false;
        }

        // Start the AI
        if (Input.GetKeyDown("a"))
        {
            isRunning = true;
            StartAIRun();
            stabliseMouse = true;
        }
    }

    // If colliding with a gate update gateLocation to the next gate
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gate")
        {
            // Disable the gate so we can't double trigger
            other.enabled = false;
            // Increment gate number
            currentGate++;
            // Set gate location to the next gate
            gateLocation = gateList[currentGate].transform.position;
        }
    }

    void StartAIRun()
    {
        // Make sure all coroutines are stopped
        StopAllCoroutines();
        // Start the coroutine that controls the AI
        StartCoroutine(LeftClickForever());
    }

    // Get position of the players body
    void GetBodyPosition()
    {
        // Update the body position variable
        bodyPosition = playerBody.transform.position;
    }

    void GetRayClickLocation()
    {
        // If we have to move right
        if (gateTotheRight)
        {
            RaycastHit hit;
            // Check that not right next to an object, if we are grab up.
            if (Physics.Raycast(playerBody.transform.position, playerBody.transform.TransformDirection(new Vector3(1, 0, 0)), out hit, maxArmDistance))
            {
                if (hit.collider.tag == "Climbable" && hit.distance <= 0.2f)
                {
                    if (Physics.Raycast(playerBody.transform.position, playerBody.transform.TransformDirection(new Vector3(0.1f, 1, 0)), out hit, maxArmDistance))
                    {
                        if (hit.collider.tag == "Climbable" && hit.distance >= minHitDistance)
                        {
                            worldClickPosition = hit.point;
                            TranslateMousePos();
                            newClickLocationFound = true;
                            return;
                        }
                    }
                }
            }
            // Ray at ne
            if (CastRayAtVector(new Vector3(1, 1, 0)))
            {
                return;
            }
            // Ray at nne
            if (CastRayAtVector(new Vector3(1, 0.5f, 0)))
            {
                return;
            }
            // Ray at nee
            if (CastRayAtVector(new Vector3(0.5f, 1, 0)))
            {
                return;
            }
            // Ray just off true north
            if (CastRayAtVector(new Vector3(0.1f, 1, 0)))
            {
                return;
            }
        }
        // Else if we have to move left
        else if (!gateTotheRight)
        {
            RaycastHit hit;
            // Check that not right next to an object, if we are grab up.
            if (Physics.Raycast(playerBody.transform.position, playerBody.transform.TransformDirection(new Vector3(-1, 0, 0)), out hit, maxArmDistance))
            {
                if (hit.collider.tag == "Climbable" && hit.distance <= 0.2f)
                {
                    if (Physics.Raycast(playerBody.transform.position, playerBody.transform.TransformDirection(new Vector3(-0.1f, 1, 0)), out hit, maxArmDistance))
                    {
                        if (hit.collider.tag == "Climbable" && hit.distance >= minHitDistance)
                        {
                            worldClickPosition = hit.point;
                            TranslateMousePos();
                            newClickLocationFound = true;
                            return;
                        }
                    }
                }
            }
            // Ray at nw
            if (CastRayAtVector(new Vector3(-1, 1, 0)))
            {
                return;
            }
            // Ray at nnw
            if (CastRayAtVector(new Vector3(-1, 0.5f, 0)))
            {
                return;
            }
            // Ray at nww
            if (CastRayAtVector(new Vector3(-0.5f, 1, 0)))
            {
                return;
            }
            // Ray just off true north
            if (CastRayAtVector(new Vector3(-0.1f, 1, 0)))
            {
                return;
            }
        }
    }

    /// <summary>
    /// Ray casts in a direction from the player location. If we hit a 'Climbable' object then move mouse to the hit location
    /// </summary>
    /// <param name="direction"> Vector3 of the direction to cast the ray </param>
    /// <returns></returns>
    bool CastRayAtVector(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerBody.transform.position, playerBody.transform.TransformDirection(direction), out hit, maxArmDistance))
        {
            if (hit.collider.tag == "Climbable" && hit.distance >= minHitDistance)
            {
                Debug.DrawRay(playerBody.transform.position, playerBody.transform.TransformDirection(direction) * hit.distance, Color.yellow, 20.0f);
                worldClickPosition = hit.point;
                Debug.Log("setting new world pos: " + worldClickPosition);
                TranslateMousePos();
                newClickLocationFound = true;
                return true;
            }
        }
        Debug.Log("failed at setting new world pos: ");
        return false;
    }

    /// <summary>
    /// Translates world position into screen position
    /// </summary>
    void TranslateMousePos()
    {
        var screenPos = cam.WorldToScreenPoint(worldClickPosition);
        screenClickPosition.x = screenPos.x;
        screenClickPosition.y = Screen.height - screenPos.y;
    }

    /// <summary>
    /// Returns the distance between the playerBody and the currentGate
    /// </summary>
    /// <returns></returns>
    Vector3 DistanceFromGate()
    {
        GetBodyPosition();
        gateDistance = gateLocation - bodyPosition;
        // If the exit is to the right of the player set gateTotheRight to true
        if (gateDistance.x >= 0)
        {
            gateTotheRight = true;
        }
        // Else if to the left set as false
        else if (gateDistance.x < 0)
        {
            gateTotheRight = false;
        }
        return gateDistance;
    }

    /// <summary>
    /// Send a left click with a hold time defined by "holdTime". Used to find a new random location to click and store if moved towards goal.
    /// </summary>
    /// <returns></returns>
    IEnumerator LeftClickForever()
    {
        while(isRunning)
        {
            // If not above the gate
            if (gateLocation.x - bodyPosition.x >= 3 || gateLocation.x - bodyPosition.x <= -3)
            {
                // Get a random location to try
                GetRayClickLocation();

                // If a new click location was found, then click
                if (newClickLocationFound)
                {
                    // Hold left click on the location
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                    // Wait for holdTime
                    yield return new WaitForSeconds(holdTime);
                    // Let go of left click
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                    // Wait for longer
                    yield return new WaitForSeconds(timeBetweenSwings);
                    // Reset click location bool
                    newClickLocationFound = false;
                }
                else
                {
                    // Wait for noClickWaitTime
                    yield return new WaitForSeconds(noClickWaitTime);
                }
            }
            // If above the gate
            else
            {
                // Wait for noClickWaitTime
                yield return new WaitForSeconds(noClickWaitTime);
            }
        }
    }
}

// Code below allows the script to move the mouse of the computer and click
// Code taken from https://stackoverflow.com/questions/2416748/how-do-you-simulate-mouse-click-in-c
public class MouseOperations
{
    [Flags]
    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010
    }

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    public static void SetCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    public static void SetCursorPosition(MousePoint point)
    {
        SetCursorPos(point.X, point.Y);
    }

    public static MousePoint GetCursorPosition()
    {
        MousePoint currentMousePoint;
        var gotPoint = GetCursorPos(out currentMousePoint);
        if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
        return currentMousePoint;
    }

    public static void MouseEvent(MouseEventFlags value)
    {
        MousePoint position = GetCursorPosition();

        mouse_event
            ((int)value,
             position.X,
             position.Y,
             0,
             0)
            ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}