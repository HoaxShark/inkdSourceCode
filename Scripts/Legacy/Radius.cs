using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{

    public SpriteRenderer radiusSprite;

	// Use this for initialization
	void Start ()
    {
        radiusSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.Mouse0))
        {
            radiusSprite.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            radiusSprite.enabled = false;
        }
	}
}
