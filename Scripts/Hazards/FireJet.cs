using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJet : Hazard
{
    [Header("Firejet settings.")]

    [SerializeField]
    [Tooltip("Time between fire jets.")]
    private float intervalTime = 3.0f;

    [SerializeField]
    [Tooltip("Duration of the particle system emission.")]
    private float emissionDuration = 4.0f;

    [SerializeField]
    [Tooltip("Grace period before hazard can kill player.")]
    private float startGracePeriod = 0.75f;

    [SerializeField]
    [Tooltip("Grace period before hazard can kill player.")]
    private float endGracePeriod = 0.5f;

    private ParticleSystem fire;
    private ParticleSystem.MainModule mainFire;

    private Collider fireCollider;

    private Light fireLight;

    // Use this for initialization
    void Start ()
    {
        fire = GetComponent<ParticleSystem>();
        fireCollider = GetComponent<Collider>();
        fireLight = GetComponentInChildren<Light>();

        //Set the particle system duration to the inspector value.
        mainFire = fire.main;
        mainFire.duration = emissionDuration;

        //Start the emission.
        fire.Play();
	}

    //Enables the fire particle emission.
    void PlayFireJet()
    {
        //Start the particle emission.
        fire.Play();
        fireLight.enabled = true;


        //Enable the hazard collider a short grace period.
        Invoke("EnableHazard", startGracePeriod);

        //Disable the hazard collider with a short grace period.
        Invoke("DisableHazard", emissionDuration - endGracePeriod);
    }

    void EnableHazard()
    {
        //Debug.Log("Hazard enabled.");
        IsActive = true;
    }

    void DisableHazard()
    {
        //Debug.Log("Hazard disabled.");
        IsActive = false;
    }

    //Called when the particle system stops emmiting. Restarts the emission after the interval duration.
    public void OnParticleSystemStopped()
    {
        fireLight.enabled = false;

        //Restart the emission after an interval.
        Invoke("PlayFireJet", intervalTime);
    }

    //Doesn't work with billboards!
    private void OnParticleCollision(GameObject other)
    {
        print("Particle collision!");

        if (other.tag == "Player")
        {
            other.GetComponent<Character>().HazardCheck(fireCollider);
            print("Particle collision with player!");
        }
    }

}
