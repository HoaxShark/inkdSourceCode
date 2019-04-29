using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages death animations, model swaps, sounds etc for the gameobject it is placed on. 
/// Gameobject requires a character script to die.
/// </summary>
public class DeathManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab placed here will be used for the explode death sequence.")]
    private GameObject explodeModel;

    [SerializeField]
    [Tooltip("The prefab placed here will be used for the dissolve death sequence.")]
    private GameObject dissolveModel;

    [SerializeField]
    [Tooltip("Time before the player respawns.")]
    private float respawnTime;

    //Array of blood emitters on a death model
    private ParticleSystem[] bloodEmitters;

    //Script to hide the player's render components.
    private HidePlayer hidePlayer;

    //Collider that is the cause of death.
    private Collider hazardCollider;

    //Actual deathmodel
    GameObject newDeathModel;

    public float RespawnTime
    {
        get
        {
            return respawnTime;
        }

        set
        {
            respawnTime = value;
        }
    }

    public Collider HazardCollider
    {
        get
        {
            return hazardCollider;
        }

        set
        {
            hazardCollider = value;
        }
    }


    /// <summary>
    /// Instantiates the first death model at the start of the level.
    /// </summary>
    private void Awake()
    {
        hidePlayer = gameObject.GetComponent<HidePlayer>();
    }

    /// <summary>
    /// Handles the death of a character.
    /// </summary>
    public void Die(EnumDefinitions.DeathTypes deathType)
    {
        CreateModel(deathType);

        //Move the death model into place and unhide
        newDeathModel.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
        newDeathModel.SetActive(true);

        //Get all particle systems on the death model
        bloodEmitters = newDeathModel.GetComponentsInChildren<ParticleSystem>();

        //Iterate over particle systems and play them
        foreach (ParticleSystem bloodEmitterSys in bloodEmitters)
        {
            //For dissolve-type deaths, set the emitter trigger (for the blood-in-water effect).
            if (deathType == EnumDefinitions.DeathTypes.Dissolve)
            {
                bloodEmitterSys.trigger.SetCollider(0, hazardCollider);
            }

            //Start the emitter.
            bloodEmitterSys.Play();
        }

        //Destroy deathModel
        Destroy(newDeathModel, RespawnTime);
        Debug.Log("After Destroy death Model call");
    }

    /// <summary>
    /// Creates a death model to take the place of the player during the death sequence.
    /// </summary>
    /// <param name="deathType">A DeathType as declared in EnumDefinitions. Defines what model will be used for the death sequence.</param>
    public void CreateModel(EnumDefinitions.DeathTypes deathType)
    {
        //Set death model to correct model
        switch (deathType)
        {
            case EnumDefinitions.DeathTypes.Explode:
                newDeathModel = Instantiate(explodeModel);
                break;

            case EnumDefinitions.DeathTypes.Dissolve:
                newDeathModel = Instantiate(dissolveModel);
                break;

            case EnumDefinitions.DeathTypes.Burn:
                newDeathModel = Instantiate(dissolveModel);
                break;

            case EnumDefinitions.DeathTypes.Default:
                newDeathModel = Instantiate(explodeModel);
                break;

            default:
                newDeathModel = Instantiate(explodeModel);
                break;
        }

        newDeathModel.SetActive(false);
    }

}
