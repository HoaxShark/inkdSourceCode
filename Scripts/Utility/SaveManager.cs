using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to save progress to file when a new scene starts. Place in an empty gameobject in each scene.
/// </summary>
public class SaveManager : MonoBehaviour
{

	void Start ()
    {
        Debug.Log("Saving data using the save manager.");

        //If levelData exists (created in the mainMenu) save the game
        if (LevelData.levelData != null)
        {
            LevelData.levelData.SaveList();
        }

        else
        {
            Debug.LogWarning("Unable to save, no levelData file exists. This won't occur when loading levels through the mainMenu.");
        }
	}
}
