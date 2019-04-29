using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerMusic : MonoBehaviour {


    private AudioManager audioManager;

    void Awake()
    {

        try
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        catch
        {
            Debug.LogWarning("Could not find audio Manager");
        }
    }

    private void Start()
    {
        if (audioManager != null)
        {
            audioManager.Play("DangerMusic");
            audioManager.Play("W1Ambience");
        }


    }
}
