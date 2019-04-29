using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Notes = "Note: Default piston position is retracted. Piston should be placed with this in mind.";

    [Header("Settings")]
    [Tooltip("The speed at which the piston extends.")]
    [SerializeField]
    private float extendSpeed;

    [Tooltip("The speed at which the piston retracts.")]
    [SerializeField]
    private float retractSpeed;

    [Tooltip("The delay after the piston has extended before it begins to retract.")]
    [SerializeField]
    private float resetTime;

    [Tooltip("The delay after the piston has retracted before it begins to extend.")]
    [SerializeField]
    private float activateDelay;

    [Tooltip("The step time when moving the piston. Smaller values makes smoother movement.")]
    [SerializeField]
    float moveWaitTime = 0.01f;

    [Tooltip("The extended position of the piston. Currently just have to set this manually.")]
    [SerializeField]
    private Vector3 destinationPos;

    //If the piston is in the process of extending. It will only squish the octo during this period.
    private bool isExtending = false;

    //The starting position of the piston.
    private Vector3 originalPos;

    //If the piston cycle should continue looping.
    private bool loopCoroutine = true;

    public bool IsActive
    {
        get
        {
            return isExtending;
        }

        set
        {
            isExtending = value;
        }
    }

    //Declare coroutines.
    private IEnumerator extendPiston;
    private IEnumerator retractPiston;
    private IEnumerator pistonCycle;

    private void Start()
    {
        originalPos = transform.position;

        //Define coroutines for opening and closing door.
        extendPiston = MovePiston(destinationPos, extendSpeed, moveWaitTime, resetTime);
        retractPiston = MovePiston(originalPos, retractSpeed, moveWaitTime, activateDelay);

        pistonCycle = PistonCycle();
        StartCoroutine(pistonCycle);
    }



    IEnumerator MovePiston(Vector3 targetPosition, float distancePerStep, float waitTime, float reverseTime)
    {
        while (transform.position != targetPosition)
        {
            yield return new WaitForSeconds(waitTime);

            //Not 100% sure how necessary this is. Simply using distancePerStep also seems to work, 
            //although this way means nicer numbers are used in inspector.
            float step = distancePerStep * waitTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }

        yield return new WaitForSeconds(reverseTime);
        isExtending = !isExtending;
}

    IEnumerator PistonCycle()
    {
        while (loopCoroutine)
        {
            if (isExtending == false)
            {
                extendPiston = MovePiston(destinationPos, extendSpeed, moveWaitTime, resetTime);
                yield return StartCoroutine(extendPiston);
            }

            else
            {
                retractPiston = MovePiston(originalPos, retractSpeed, moveWaitTime, activateDelay);
                yield return StartCoroutine(retractPiston);
            }


            // yield return new WaitForSeconds();
        }
    }

}
