using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class CustomSceneManager : MonoBehaviour
{


    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Delegate function, called when a new scene is loaded. 
    /// This should tell Fungus that a new scene has started.
    /// </summary>
    /// <param name="scene">Name of the scene being loaded.</param>
    /// <param name="mode">Mode of loading.</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        //Tell fungus a scene has been loaded.

        if (scene.name == "1-1")
        {
            //Fungus.SceneLoader
            Fungus.Flowchart.BroadcastFungusMessage("SceneLoaded");
        }
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



}
