using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables a collider at the end of the entrance shaft to stop the player reentering the shaft after leaving
/// </summary>
public class CloseShaft : MonoBehaviour {

    [SerializeField]
    [Tooltip("Collider to close off the shaft after player leaves")]
    private BoxCollider closeCollider;

    // Reference to pushScript the gameObject this is attached to has 
    private Push pushScript;

    // Use this for initialization
    void OnTriggerExit(Collider Other)
    {
        // Get attached pushScript
        pushScript = GetComponent<Push>(); 
        // Destroy pushScript
        Destroy(pushScript);
        // Close off the shaft
        closeCollider.enabled = true;
    }
}
