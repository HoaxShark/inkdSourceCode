using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is used to activate other enviromental objects in the scene if pushed down by the player or a physics object
/// Can be set to constant where it will only activate when continually pressed, or one off where it stays pressed even when the pressing object leaves
/// </summary>
public class PressurePlate : EnvironmentObject
{
    [SerializeField]
    [Tooltip("How far to move the plate down on press.")]
    private float activatedOffset = 0.05f;

    [SerializeField]
    [Tooltip("True if the plate must be constantly held to activate it's connected objects.")]
    private bool constantPress = false;

    [SerializeField]
    [Tooltip("Array of all connected doors.")]
    private Door[] connectedDoors;

    [SerializeField]
    [Tooltip("Do we use animation to open the connected doors?")]
    private bool useAnimation;
    private Animator anim;

    // The wait time in the door movement coroutine. Smaller numbers make movement smoother but are more expensive. Could expose in inspector.
    private float moveWaitTime = 0.08f;

    // The speed the plate will move down at
    private float speed = 2.0f;

    // For storing the original position to return to
    private Vector3 originalPos;

    // For storing the destination position to move to
    private Vector3 destinationPos;

    // For Storing Audiomanager
    private AudioManager audioManager;
    
    // If the auido has been played or not
    private bool audioPlayed;

    // Use this for initialization
    void Start ()
    {
        // Store original position
        originalPos = transform.position;

        // Set base values for destination
        destinationPos = originalPos;

        // Add the door width to the z axis of destination
        destinationPos.y = originalPos.y - activatedOffset;

        // Assign audioManager if it exists
        try
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        catch
        {
            Debug.LogWarning("Could not find audio Manager");
        }
        
        // Make sure audioPlayed is set to false
        audioPlayed = false;
    }

    /// <summary>
    /// Coroutine to move the plate to its destination position.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDown()
    {
        yield return new WaitForSeconds(moveWaitTime);
        float step = speed * moveWaitTime;
        isActive = true;
        while (transform.position != destinationPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPos, step);
        }
    }

    /// <summary>
    /// Coroutine to move the plate to its original position.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(moveWaitTime);
        float step = speed * moveWaitTime;
        isActive = false;
        while (transform.position != originalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, step);
        }
    }

    /// <summary>
    /// When another collider enters check if either the player or physicsObject, if ture move platform down and mark as active
    /// </summary>
    /// <param name="other"> The other collider entering the trigger </param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PhysicsObject")
        {            
            //Move button down.
            StartCoroutine(MoveDown());

            // If auido hasn't been played and the manager exists play audio
            if (audioManager != null && audioPlayed == false)
            {
                audioManager.Play("Button");
                audioPlayed = true;
            }

            // Loop through connected doors and move them.
            for (int i = 0; i < connectedDoors.Length; i++)
            {
                if (useAnimation == true)
                {
                    // Gets the animator component of the game object this script is applied to.
                    anim = connectedDoors[i].GetComponent<Animator>(); 
                    anim.SetBool("ButtonPushed", true);
                }
                else
                {
                    connectedDoors[i].OpenDoor();
                }
            }
        }
    }

    /// <summary>
    /// When a collider exits if constantPress is not required do nothing, if it is required return pressure plate to original position and mark as not active
    /// </summary>
    /// <param name="other"> The other collider exiting the trigger </param>
    void OnTriggerExit(Collider other)
    {
        // If plate doesn't need constant pressing, then return
        if (constantPress == false)
        {
            return;
        }

        // If the exiting object is a player or physics object and the plate has reached its destination then return to original position
        else if (other.tag == "Player" || other.tag == "PhysicsObject")
        {
            if (transform.position == destinationPos)
            {
                StartCoroutine(MoveUp());

                // Loop through all connectedObjects and set not active
                for (int i = 0; i < connectedDoors.Length; i++)
                {
                    connectedDoors[i].CloseDoor();
                }
            }
        }
    }
}
