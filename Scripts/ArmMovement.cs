﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arm class. Should probably be renamed to just Arm. Handles movement and properties of an octopus arm. 
/// 
/// Variable names need some cleanup to use the same naming conventions.
/// </summary>
public class ArmMovement : MonoBehaviour
{
    [Header("Arm Settings")]

    [SerializeField]
    [Tooltip("Defines arm as left/right arm for mouse button purposes. 0 = left, 1 = right.")]
    private int armIndex = 0;

    [SerializeField]
    [Tooltip("How fast the arm can move.")]
    private float movementSpeed = 0.6f;

    [SerializeField]
    [Tooltip("The max distance the arm can travel from the player")]
    private float maxDistanceFromPlayer = 12.0f;

    [SerializeField]
    [Tooltip("An offset to compesnate for the Z-axis of the mouse. Leave as default.")]
    private float mouseOffset = 25.0f;

    [SerializeField]
    [Tooltip("The max length of the arm while grabbed onto something. Controls the springjoint length.")]
    private float grabbedDistance = 0.2f;

    [Header("Effects")]

    [SerializeField]
    [Tooltip("Decal prefab to use for the inksplat generated by arms.")]
    private GameObject inkDecalPrefab;

    // Holder object to store inactive arms. Should be set to the capsule in octoBody
    private Transform armHolder; 

    //If the game is paused. (Or just pausing the player?)
    private bool paused = false;

    //If arm has been fired (in use).
    private bool fired = false;

    //If arm is grabbed on to anything.
    private bool grabbed = false;

    //Arm rigidbody
    private Rigidbody thisBody;

    //Spring joint between arm and body
    private SpringJoint playerSpringJoint; 

    //Movement system in use, set from Character.
    //True = Movement B, False = Movement C
    private bool useMovementB = true;

    // if true the inkDecal is allowed to spawn on collision
    private bool canSpawnInk = false;

    private AudioManager audioManager;

    //If the line renderer needs to be drawn for this arm.
    private bool drawLineRenderer; 

    public bool DrawLineRenderer
    {
        get
        {
            return drawLineRenderer;
        }

        set
        {
            drawLineRenderer = value;
        }
    }

    public bool UseMovementB
    {
        get
        {
            return useMovementB;
        }

        set
        {
            useMovementB = value;
        }
    }

    public bool Grabbed
    {
        get
        {
            return grabbed;
        }

        set
        {
            grabbed = value;
        }
    }

    public bool Fired
    {
        get
        {
            return fired;
        }

        set
        {
            fired = value;
        }
    }

    public Transform ArmHolder
    {
        get
        {
            return armHolder;
        }

        set
        {
            armHolder = value;
        }
    }

    public bool Paused
    {
        get
        {
            return paused;
        }

        set
        {
            paused = value;
        }
    }

    private void Start()
    {
        armHolder = GameObject.Find("armHolder").transform;
        thisBody = gameObject.GetComponent<Rigidbody>();
        playerSpringJoint = gameObject.GetComponent<SpringJoint>();

        try
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        catch
        {
            Debug.LogWarning("Could not   audio Manager");
        }
    }

    void Update()
    {
        //Controls for PC
        if (!paused)
        {
            #if UNITY_STANDALONE || UNITY_WEBPLAYER
            HandleStandaloneInput();

            //Controls for mobile
            #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            HandleStandaloneInput(); //This should actually be HandleMobileInput. Interestingly, this works for prototype as unity equates mousePosition to touch(0).

            #endif
        }

    }
    
    //Process input for standalone builds.
    void HandleStandaloneInput()
    {
        //calculating the mouse position.
        var myMousePosition = Input.mousePosition; //taking input for the positioning of the mouse
        myMousePosition.z = mouseOffset; // mouse offset to put the click on the right z axis
        myMousePosition = Camera.main.ScreenToWorldPoint(myMousePosition); //setting the position reading to local

        //Arm lets go if middle mouse click is pressed and the arm is grabbing.
        if (Input.GetMouseButtonDown(2) && grabbed)
        {
            grabbed = false;
        }

        //If mouse released and arm is not grabbed, reset the arm.
        if (!Input.GetMouseButton(armIndex) && !grabbed)
        {
            fired = false;
            grabbed = false;
        }

        //If mouse released and arm is grabbed, either leave arm (movement C) or unfreeze arm (movement B).
        if (Input.GetMouseButtonUp(armIndex) && grabbed)
        {
            //Movement B mechanics
            if (UseMovementB)
            {
                grabbed = false;
                fired = false;

                //Unfreeze the arm, allowing it to retract to body.
                thisBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
        }

        //Return arm to octopus body when not in use.
        if (!fired && !grabbed)
        {
            //Move the arm to the armHolder (behind the octopus body).
            transform.position = Vector3.MoveTowards(transform.position, armHolder.position, 1);

            //Disable line renderer when the arm in behind the octopus.
            if (transform.position == armHolder.position)
            {
                DrawLineRenderer = false;
            }
        }

        //This intialises the arm ready for movement but does not handle control of the arm. Also resets if we can spawn ink.
        if (Input.GetMouseButtonDown(armIndex) && !fired)
        {
            canSpawnInk = true;
            fired = true;
        }

        //Control an arm while the mouse is pressed down.
        if (fired && Input.GetMouseButton(armIndex) && !grabbed)
        {
            //Draw arm.
            DrawLineRenderer = true;

            //Allow arm to move away at decent range
            playerSpringJoint.maxDistance = 20.0f;

            //Follow cursor.
            Vector3 direction = myMousePosition - armHolder.position;
            direction = Vector3.ClampMagnitude(direction, maxDistanceFromPlayer);
            transform.position = Vector3.MoveTowards(transform.position, armHolder.position + direction, movementSpeed);
        }

        //When arm is active in any way, show the arm spheres, otherwise, hide them to prevent clipping.
        if (fired == true || grabbed == true)
        {
            ShowArm(true);
        }

        else
        {
            ShowArm(false);
        }

    }

    /// <summary>
    /// Show or hide the arm sphere.
    /// </summary>
    /// <param name="isShown">Boolean to set if the arm sphere is visible.</param>
    void ShowArm(bool isShown)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = isShown;
    }

    //Process input for mobile devices. 
    void HandleMobileInput()
    {
        //When touch occurs.
        if (Input.touchCount > 0)
        {
            //Get position of a touch
            Vector2 touchPositionRaw = Input.GetTouch(0).position;
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touchPositionRaw);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Colliding with climable object
        if (collision.gameObject.tag == "Climbable" && Fired)
        {
            //Play splat sound when grabbing surfaces.
            if (audioManager != null)
            {
                audioManager.Play("InkSplat");
            }

            grabbed = true; //defining that the arm object is colliding with the GrabSpot
            fired = false; //setting Fired to false so that arm will stop moving and stay where it is

            playerSpringJoint.maxDistance = grabbedDistance; //setting the maxDistance for SpringJoint to pull the player in

            //Freeze the arm in place
            thisBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }

        if (collision.gameObject.tag == "Climbable" && Grabbed && canSpawnInk)
        {
            // spawn an ink decal where the arm collided
            spawnInkDecal(collision);
            canSpawnInk = false;
        }
    }

    //Flips the bool value of paused. Was used in fungus but might be the cause of some bugs.
    void flipPause()
    {
        paused = !paused;
    }

    //Sets the puased boolean. Intended for use in fungus to replace flipPause but fungus' inability to CallMethod with parameter makes this redundant.
    public void SetPaused(bool isPaused)
    {
        paused = isPaused;
    }

    //Used by fungus to pause the arms
    void pauseArm()
    {
        paused = true;
    }

    void unpauseArm()
    {
        paused = false;
    }

    /// <summary>
    /// Spawns the ink decal on the arm position and rotates it slightly depending on its initial z rotation
    /// </summary>
    /// <param name="collision">The collision that triggered</param>
    void spawnInkDecal(Collision collision)
    {
        // normal of the contact point 
        Vector3 direction = collision.contacts[0].normal;
        // set spawn decal location on the arm position
        Vector3 spawnPosition = transform.position;
        // create a Quaternion and rotate given the direction normal
        Quaternion quatRotation = Quaternion.FromToRotation(Vector3.up, direction);
        // translate Quaternion to Vector3 for working on
        Vector3 current_rotation = quatRotation.eulerAngles;

        // change the rotation of the object so it projects side on
        if (current_rotation.z == 0 || current_rotation.z == -180 || current_rotation.z == 180)
        {
            current_rotation.x =- 30;
        }
        else if (current_rotation.z >= 10 && current_rotation.z <= 181)
        {
            current_rotation.y = -50;
        }
        else if (current_rotation.z >= 185 && current_rotation.z <= 320)
        {
            current_rotation.y = 50;
        }
      
        // translate back to Quaternion
        quatRotation = Quaternion.Euler(current_rotation);
        // finally spawn the decal
        GameObject newInk = Instantiate(inkDecalPrefab, spawnPosition, quatRotation);
        // rotate the ink around the Y axis by a random number
        float newRotation = Random.Range(0.0f, 360.0f);
        newInk.transform.Rotate(Vector3.up, newRotation, Space.Self);
    }
}


