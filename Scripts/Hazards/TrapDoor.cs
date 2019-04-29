using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour {

    private Rigidbody rb; // Ridgidybody of the door
    private Vector3 startPosition; //  Where the door started
    private bool notPushed; // If the door is currently being pushed

	void Start () {
        rb = GetComponent<Rigidbody>(); // Set the rigidbody
        startPosition = transform.position; // Set start pos
	}
	
	void Update () {
        // If back at start pos and not being pushed then make kinematic
		if (transform.position == startPosition && notPushed)
        {
           rb.isKinematic = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        // If being pushed by the player turn off kinematic and mark as being pushed
        if (other.tag == "Player")
        {
            rb.isKinematic = false;
            notPushed = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player stops colliding mark as not pushed
        if (other.tag == "Player")
        {
            notPushed = true;
        }
    }
}
