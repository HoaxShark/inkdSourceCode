using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialscript1 : MonoBehaviour {
    private Animator anim;
    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        GetComponent<Animator>();
        anim.SetBool("ButtonPushed", true);
    }
}
	