using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource music1;

    public AudioSource fanBG;

	// Use this for initialization
	void Start ()
    {
        music1.GetComponent<AudioSource>();

        music1.Play(0);
        fanBG.Play(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
