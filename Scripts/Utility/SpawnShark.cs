using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShark : MonoBehaviour
{
    public GameObject sharkObject; //trying to spawn the Shark after exiting a Trigger Box

    // Use this for initialization
    void Start ()
    {
        sharkObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sharkObject.SetActive(true);
        }
    }
}
