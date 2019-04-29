using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add to light prefabs to apply flickering effect to the light.
/// Light components need to be dragged onto this script. Currently supports exactly three lights, needs to be updated to handle any number!
/// </summary>
public class FlickerLights : MonoBehaviour
{
    [Header("Light flicker settings.")]
    [SerializeField]
    [Tooltip("Minimum time between light flickers")]
    private float minFlickerDelay = 0.1f;

    [SerializeField]
    [Tooltip("Maximum time between light flickers")]
    private float maxFlickerDelay = 0.3f;

    [SerializeField]
    [Tooltip("Duration of a burst of flickering in seconds")]
    private int flickerBurstDuration = 1;

    [SerializeField]
    [Tooltip("Minimum time between flicker bursts")]
    private float minPauseTime = 0.5f;

    [SerializeField]
    [Tooltip("Maximum time between flicker bursts")]
    private float maxPauseTime = 1.0f;

    [SerializeField]
    [Tooltip("If the light is off between flicker bursts")]
    private bool offByDefault = false;

    [SerializeField]
    [Tooltip("If the flicker script should run on start")]
    private bool startActive = true;

    [Header("Light components.")]
    [SerializeField]
    private Light light1;

    [SerializeField]
    private Light light2;

    [SerializeField]
    private Light light3;

    //Keeps the flickerCycle coroutine running. Set this to false to break out.
    private bool loopCoroutine = true;

    IEnumerator flicker;
    IEnumerator flickerCycle;

    private void Start()
    {
        //Define coroutines.
        flicker = Flicker();
        flickerCycle = FlickerCycle();

        if (startActive == true)
        {
            StartFlickerRoutines();
        }

    }

    public void StartFlickerRoutines()
    {
        //Start the flickerCycle coroutine.
        StartCoroutine(flickerCycle);
    }

    /// <summary>
    /// Flickers a light on and off for a duration.
    /// </summary>
    /// <returns></returns>
    IEnumerator Flicker()
    {
        //Flicker lights up to the burst duration
        for (int i = 0; i <= flickerBurstDuration; i++)
        {
            //Enable or disable lights based on the default state. (default state is the one done last)
            light1.enabled = offByDefault;
            light2.enabled = offByDefault;
            light3.enabled = offByDefault;

            yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));

            light1.enabled = !offByDefault;
            light2.enabled = !offByDefault;
            light3.enabled = !offByDefault;

            yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));
        }
    }

    /// <summary>
    /// The flicker cycle. Runs the flicker coroutine to create bursts of flickering and then pauses between them. 
    /// Will run forever until loopCoroutine is set to false;
    /// </summary>
    /// <returns></returns>
    IEnumerator FlickerCycle()
    {
        while (loopCoroutine)
        {
            yield return StartCoroutine(Flicker());

            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
        }
    }

}

