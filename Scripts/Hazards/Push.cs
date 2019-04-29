using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    [SerializeField]
    private float thrust; // thurst applied to the rigidbodies

    [SerializeField]
    private GameObject targetGO; // target for the fan push direction

    [SerializeField]
    private GameObject fanGO; // target for the fan push direction

    private Rigidbody rb; // Rigidbody of entered colliders

    private Vector3 direction; // direction the thrust will be applied in

    // Use this for initialization
    void Start () {
        Vector3 heading = targetGO.transform.position - fanGO.transform.position;
        direction = heading.normalized;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Adds force to all rigidbodies that enter in the direction the fan is facing
    /// </summary>
    /// <param name="other">Collider entering the trigger</param>
    private void OnTriggerStay(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();

        if (other.tag != "Arm")
        {
            rb.AddForce(direction * thrust);
        }
    }
}
