using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flips to an animated camera for the length of the animation then back to the main camera
/// </summary>
public class SpeedLevelCam1 : MonoBehaviour
{
    [Tooltip("This referes to the Gamecam")]
    [SerializeField]
    private GameObject gameCam;

    [Tooltip("This referes to the speed cam that is animated")]
    [SerializeField]
    private GameObject speedCam;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Wait());
    }

    /// <summary>
    /// waits for 4.5 secounds for the animation to play , then disable speed cam and enables the Game Cam
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4.5f);

        speedCam.SetActive(false);
        gameCam.SetActive(true);
    }
}