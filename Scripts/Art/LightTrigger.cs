using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    [Tooltip("If the light is off as it's default state.")]
    private bool offByDefault = true;

    [SerializeField]
    [Tooltip("Script to flicker lights after trigger.")]
    private FlickerLights flickerLightsScript;

    [Header("Light components.")]
    [SerializeField]
    private Light light1;

    [SerializeField]
    private Light light2;

    [SerializeField]
    private Light light3;
    
    // Use this for initialization
    void Start ()
    {
        //Enable or disable lights based on the default state. (Opposite to default state to flip the light on/off)
        light1.enabled = !offByDefault;
        light2.enabled = !offByDefault;
        light3.enabled = !offByDefault;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player has entered trigger zone.
        if (other.tag == "Player")
        {    
            //Enable or disable lights based on the default state. (Opposite to default state to flip the light on/off)
            light1.enabled = offByDefault;
            light2.enabled = offByDefault;
            light3.enabled = offByDefault;

            if (flickerLightsScript != null)
            {
                flickerLightsScript.StartFlickerRoutines();
            }
        }
    }

}
