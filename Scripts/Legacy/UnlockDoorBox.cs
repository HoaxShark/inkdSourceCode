using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorBox : MonoBehaviour {

    public GameObject Door;

    public GameObject Door2;

	// Use this for initialization
	void Start ()
    {
        Door = GameObject.Find("Locked Door");

        Door2 = GameObject.Find("Locked Door 2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PressurePlate1")
        {
            Debug.Log("Unlock Door 1");

            Destroy(gameObject);

            Destroy(Door);
        }

        if (other.gameObject.name == "PressurePlate2")
        {
            Debug.Log("Unlock Door 2");

            Destroy(gameObject);

            Destroy(Door2);
        }



    }
}
