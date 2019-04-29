using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The light on this checkpoint.")]
    private Light checkPointLight;

    [SerializeField]
    [Tooltip("The colour of the checkpoint light when the checkpoint is not activated.")]
    private Color checkpointOffColour = Color.red;

    [SerializeField]
    [Tooltip("The colour of the checkpoint light when the checkpoint is activated.")]
    private Color checkpointOnColour = Color.green;

    [SerializeField]
    [Tooltip("The sound to play when activating a checkpoint.")]
    private AudioSource checkpointSound;

    
    public AudioSource CheckpointSound
    {
        get
        {
            return checkpointSound;
        }

        set
        {
            checkpointSound = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        //Grab first light from children if no light is assigned.
        if (checkPointLight == null)
        {
            checkPointLight = gameObject.GetComponentInChildren<Light>();
        }

        //Grab first audiosource from children if no audiosource is assigned.
        if (checkpointSound == null)
        {
            checkpointSound = gameObject.GetComponentInChildren<AudioSource>();
        }

        //Default light to off on start.
        checkPointLight.color = checkpointOffColour;
	}

    /// <summary>
    /// Enabled or disable the checkpoint light.
    /// </summary>
    /// <param name="isActivated">True activates the light, false deactivates it.</param>
    public void LightIsActivated(bool isActivated)
    {
        if (isActivated == true)
        {
            checkPointLight.color = checkpointOnColour;
        }
        else
        {
            checkPointLight.color = checkpointOffColour;
        }
    }

}
