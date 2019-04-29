using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    /*
    private Transform arm;
    private ArmMovement armScript;
    private Rigidbody thisBody;
    
    bool hasPlayer = false;
    bool beingCarried = false;

    void Start()
    {
        thisBody = GetComponent<Rigidbody>();
    }

    // on collider enter if an arm
    private void OnTriggerEnter(Collider other)
    {
        if (!beingCarried)
        {
            if (other.tag == "Arm")
            {
                Debug.Log("arm triggered");
                // store transform for telling the item where to be
                arm = other.transform;
                hasPlayer = true;
                // set variable for octo arm script
                armScript = other.GetComponent<ArmMovement>();
                if (!armScript.HasItem)
                {
                    Debug.Log("arm doesnt have object so pickup");
                    beingCarried = true;
                    armScript.HasItem = true;
                    thisBody.isKinematic = true;
                    armScript.IsActive = false; // deactivate the grabbing arm
                    //armScript.OtherArm.isActive = true; // activate the other arm
                }
            }
        }
    }

    // on collider exit if its an arm set hasplayer to false
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Arm")
        {
            hasPlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If right mouse button is pressed let go of object
        if ((Input.GetMouseButtonDown(1)) && (beingCarried == true))
        {
            armScript.HasItem = false;
            beingCarried = false;
            thisBody.isKinematic = false;
            
        }

        // if not being carried stop following the parent
        if (!beingCarried)
        {
            transform.parent = null;
        }

        // if being carried follow the parent
        else if (beingCarried)
        {
            transform.parent = arm; 
        }
    }*/
}

