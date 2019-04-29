using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

    [SerializeField]
    private float thrust; // thurst applied to the rigidbodies

    [SerializeField]
    private Vector3 direction; // direction the thrust will be applied in

    [SerializeField]
    [Tooltip("Higher is slower")]
    private float armVelocity;

    private Rigidbody rb; // Rigidbody of entered colliders

    public Vector3 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    /// <summary>
    /// Moves the bodies along the conveyor
    /// </summary>
    /// <param name="other">Collider entering the trigger</param>
    private void OnTriggerStay(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();

        float beltVelocity = thrust * Time.deltaTime;
        rb.velocity = beltVelocity * Direction;

        if (other.tag == "Arm")
        {
            Transform location = other.GetComponent<Transform>();
            other.transform.position = location.position + (Direction / armVelocity);
        }
    }

    /// <summary>
    /// If an arm exits the trigger and mouse buttons are not held then arm returns to thte players body
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Arm")
        {
            if (!other.GetComponent<ArmMovement>().Fired)
            {
                other.GetComponent<ArmMovement>().Grabbed = false;
            }
        }
    }
}
