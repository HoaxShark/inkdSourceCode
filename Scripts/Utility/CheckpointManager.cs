using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Character to restart at checkpoints. Use Octo body not the parent!")]
    private GameObject character;

    //Defining coroutine.
    private IEnumerator enableConstraints;

    /// <summary>
    /// Restarts the player to their last "starting position". This position should be set on level start and checkpoints.
    /// </summary>
    /// <param name="restartPosition">Position to restart the character in.</param>
    public void Restart(Vector3 restartPosition)
    {
        //Freeze x movement to stop character shooting off.
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        //Update character's position to their last starting position.
        character.transform.position = restartPosition;

        //Call coroutine to disable constraint.
        enableConstraints = EnableConstraintsAfterTime(0.2f);
        StartCoroutine(enableConstraints);
    }

    /// <summary>
    /// Coroutine to delay the constraint release by a fraction of a second to prevent octo shooting off under momentum.
    /// </summary>
    /// <param name="time"> How long to wait. This should be as small as possible without messing things up. 0.2 seconds works well.</param>
    /// <returns></returns>
    IEnumerator EnableConstraintsAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        //Reset constraints
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }
}
