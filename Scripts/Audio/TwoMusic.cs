using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoMusic : MonoBehaviour {


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
            audioManager.Play("W2Music");
            audioManager.Play("W2Ambience");
        }


    }
}
