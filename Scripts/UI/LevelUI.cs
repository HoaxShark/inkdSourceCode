using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class that handles the UI elements for each individual level on the level select screen. 
/// This is mainly concered with displaying the medals the player has earned.
/// </summary>
public class LevelUI : MonoBehaviour
{
    public string levelName;

    //The image components to display the medals for this level
    //**Take care not to rearrange the order of these medals in the inspector.** 
    [SerializeField]
    private Image timeMedal;

    [SerializeField]
    private Image clamMedal;


    public Image TimeMedal
    {
        get
        {
            return timeMedal;
        }

        set
        {
            timeMedal = value;
        }
    }

    public Image ClamMedal
    {
        get
        {
            return clamMedal;
        }

        set
        {
            clamMedal = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        //Get all image components on object
        Image[] medals = gameObject.GetComponentsInChildren<Image>();

        //Assign each image based on the array. I am not super comfortable with this. Array *should* always return in order of inspector but I don't like it. 
        //However, the alternative is having to manually add these for every level. 
        //**Take care not to rearrange these medals in the inspector.** 
        //*NOTE* GetComponentsInChildren includes the image in the parent (the button) so we start at 1 to avoid this.
        TimeMedal = medals[1];
        ClamMedal = medals[2];
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
