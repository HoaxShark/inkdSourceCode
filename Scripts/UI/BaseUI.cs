using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Base UI class. This should contain methods and attributes common to all UI groups. 
/// This should be kept as clean and non-cluttered as possible so it is easy to add the basic UI funcitonality to any object.
/// Avoid dependency on things that have to be set by hand in the inspector.
/// </summary>
public class BaseUI : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Load a new scene
    /// </summary>
    /// <param name="levelName"> The name of the level to load. String </param>
    public void LoadLevel(string levelName)
    {
        // Make sure time scale is default, since its paused during the victory canvas
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Load the next scene in the project.
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    /// <summary>
    /// Change the sprite of an image component.
    /// </summary>
    /// <param name="image"> The Image to Change. </param>
    /// <param name="sprite"> The sprite to load as the new image. </param>
    public void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
}
