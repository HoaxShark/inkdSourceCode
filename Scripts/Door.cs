using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls doors in the game so they can be opened and closed, controlled by buttons
/// </summary>
public class Door : EnvironmentObject
{
    [SerializeField]
    [Tooltip("How fast the door opens and closes.")]
    private float speed = 2.0f;

    // The wait time in the door movement coroutine. Smaller numbers make movement smoother but are more expensive. Could expose in inspector.
    private float moveWaitTime = 0.04f;

    // Width of the door, used to calculate where the door has to open to.
    private float doorWidth;

    // The original (closed) position of the door.
    private Vector3 originalPos; 

    //Position the door moves to when open.
    private Vector3 destinationPos;

    // Refernce to audioManager
    private AudioManager audioManager;

    // If the audio is to be played
    private bool audioPlayed;

    // Coroutine to open the door
    private IEnumerator openDoor;

    // Coroutine to close the door
    private IEnumerator closeDoor;

    // Use this for initialization
    void Start ()
    {
        // Store original position
        originalPos = transform.position;

        // Get width of door
        doorWidth = transform.lossyScale.z;

        // Set base values for destination
        destinationPos = originalPos;

        // Add the door width to the z axis of destination
        destinationPos.z = originalPos.z + doorWidth;

        // Define coroutines for opening and closing door.
        openDoor = MoveDoor(destinationPos, speed, moveWaitTime);
        closeDoor = MoveDoor(originalPos, speed, moveWaitTime);

        // Apply audio manager if avaliable
        try
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        catch
        {
            Debug.LogWarning("Could not find audio Manager");
        }

        // Set that the audio has not played
        audioPlayed = false;
    }

    /// <summary>
    /// Returns door to original position
    /// </summary>
    public void CloseDoor()
    {
        StartCoroutine(closeDoor);
    }

    /// <summary>
    /// Moves door along the z axis to its set destination. Audio component might need more work.
    /// </summary>
    public void OpenDoor()
    {
        StartCoroutine(openDoor);

        // Play audio if not already played and the audio manager exists 
        if (audioManager != null && audioPlayed == false)
        {
            audioManager.Play("Door");
            audioPlayed = true;
        }
    }

    /// <summary>
    /// Coroutine to move the door to a target position. Used to open and close the door.
    /// </summary>
    /// <param name="targetPosition">The position to move the door to.</param>
    /// <param name="distancePerStep">How far the door moves each frame, this is equivilent to the speed of movement.</param>
    /// <param name="waitTime">The step time for the coroutine, smaller value are smoother but more expensive.</param>
    /// <returns></returns>
    IEnumerator MoveDoor(Vector3 targetPosition, float distancePerStep, float waitTime)
    {
        while (transform.position != targetPosition)
        {
            yield return new WaitForSeconds(waitTime);
            float step = distancePerStep * waitTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    } 
}
