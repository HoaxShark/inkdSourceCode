using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    //Material using the dissolve shader
    private Material material;

    private float dissolveTarget;
    private float dissolveCurrent;

    private void Awake()
    {
        //Grab the shader material
        material = gameObject.GetComponent<Renderer>().material;
        dissolveTarget = 1.0f;
        dissolveCurrent = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        dissolveCurrent = Mathf.Lerp(dissolveCurrent, dissolveTarget, Time.deltaTime);
        material.SetFloat("_dissolveSlider", dissolveCurrent);
	}
}
