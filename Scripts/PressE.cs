using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Script activates a fungus trigger if the player presses E inside the trigger of the gameObject this is attached too
/// Set blockToExecute in the inspector to the fungus flowchart name you want to be triggered
/// </summary>
public class PressE : MonoBehaviour
{
    [Tooltip("Fungus flowchart reference.")]
    [SerializeField]
    private Fungus.Flowchart flowchart;

    [Tooltip("Text to be displayed.")]
    [SerializeField]
    private GameObject text;

    [Tooltip("The name of the flowchart block to execute when E is pressed.")]
    [SerializeField]
    private string blockToExecute = "";

    //If the player is inside the trigger zone.
    private bool hasCollided;

    private void Start()
    {
        // Make sure text isn't active at the start
        text.SetActive(false);
    }

    void Update()
    {
        // When the player presses E inside the trigger zone, play the email.
        if (hasCollided == true && Input.GetKeyDown(KeyCode.E))
        {
            flowchart.ExecuteBlock(blockToExecute);
        }
    }

    /// <summary>
    /// Record if player is in the trigger zone. When you are within the Trigger Box and not only when you first enter it.
    /// </summary>
    /// <param name="other">Collider in the trigger area</param>
    void OnTriggerStay(Collider other)
    { 
        if (other.gameObject.tag == "Player")
        {
            hasCollided = true;
            text.SetActive(true);
        }
    }

    /// <summary>
    /// Update if the player leaves the trigger zone.
    /// </summary>
    /// <param name="other">Collider leaving the trigger area</param>
    void OnTriggerExit(Collider other)
    {
        hasCollided = false;
        text.SetActive(false);
    }
}



