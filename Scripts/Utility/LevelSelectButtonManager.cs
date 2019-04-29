using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to manage the images on the level select buttons. Also changes if the buttons are interactable. Gets level data from the levelData script.
/// </summary>

public class LevelSelectButtonManager : MonoBehaviour
{

    [SerializeField]
    private int levelNumber; // Build index of the level, level 1 is build index 0 and so on

    [SerializeField]
    private Button thisButton; // the button this script is attached to.

    [SerializeField]
    private Image clamImage; // image for the clam medal on button

    [SerializeField]
    private Image timeMedal; // image for the time medal on button


    [SerializeField]
    private Text text; // the buttons text


    //Sprites for different medals.
    [SerializeField]
    private Sprite goldMedalSprite;

    [SerializeField]
    private Sprite silverMedalSprite;

    [SerializeField]
    private Sprite bronzeMedalSprite;


    //Sprites for the medal tick and cross
    [SerializeField]
    private Sprite tick;

    [SerializeField]
    private Sprite cross;

    [SerializeField]
    private Sprite clamYes;

    [SerializeField]
    private Sprite clamNo;

    // Use this for initialization
    void Start ()
    {
		if (LevelData.levelData.myLevels[levelNumber].unlocked)
        {
            // turn on the button
            thisButton.interactable = true;

            // enable text on the button
            text.enabled = true;

            //Change clam image if they have been collected
            if (LevelData.levelData.myLevels[levelNumber].clams)
            {
                SetImage(clamImage, clamYes);
            }

            //Check the level data to see which time medals have been earnt. Display the best medal.
            if (LevelData.levelData.myLevels[levelNumber].goldTime)
            {
                Debug.Log("Gold medal active");
                SetImage(timeMedal, goldMedalSprite);
            }

            else if (LevelData.levelData.myLevels[levelNumber].silverTime)
            {
                Debug.Log("Silver medal active");
                SetImage(timeMedal, silverMedalSprite);
            }

            else if (LevelData.levelData.myLevels[levelNumber].bronzeTime)
            {
                Debug.Log("Bronze medal active");
                SetImage(timeMedal, bronzeMedalSprite);
            }
        }

        else if (!LevelData.levelData.myLevels[levelNumber].unlocked)
        {
            // turn off button if not unlocked
            thisButton.interactable = false;
            // disable the text on the button
            text.enabled = false;
        }
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
