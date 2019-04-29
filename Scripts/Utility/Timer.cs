using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The victory screen time score display.")]
    private Text timeScoreText;

    [SerializeField]
    [Tooltip("The UI time display.")]
    private Text timeScoreUI;
       
    [SerializeField]
    [Tooltip("Victory screen canvas.")]
    private Canvas victoryCanvas;
       
    [SerializeField]
    [Tooltip("Canvas of the HUD UI.")]
    private Canvas uiCanvas;

    [SerializeField]
    [Tooltip("If the timer is active.")]
    private bool increaseTime = true;

    private VictoryMenu victoryMenu;

    //Current time score.
    private float timeScore = 0f;

    //Current level this timer is in.
    private int level;

    public bool IncreaseTime
    {
        get
        {
            return increaseTime;
        }

        set
        {
            increaseTime = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public Canvas VictoryCanvas
    {
        get
        {
            return victoryCanvas;
        }

        set
        {
            victoryCanvas = value;
        }
    }

    public Canvas UiCanvas
    {
        get
        {
            return uiCanvas;
        }

        set
        {
            uiCanvas = value;
        }
    }

    public VictoryMenu VictoryMenu
    {
        get
        {
            return victoryMenu;
        }

        set
        {
            victoryMenu = value;
        }
    }

    public float TimeScore
    {
        get
        {
            return timeScore;
        }

        set
        {
            timeScore = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Level = (SceneManager.GetActiveScene().buildIndex - 1); // gets the build index to reference the level (-1 because the mainmenu is the first build index)
        IncreaseTime = true;

        VictoryMenu = VictoryCanvas.GetComponent<VictoryMenu>();

        VictoryCanvas.enabled = false;
        UiCanvas.enabled = true;
       
        Time.timeScale = 1.0f;

        //Check variables have been assigned.
        if (timeScoreText == null)
        {
            Debug.LogWarning("timeScoreText variable of Timer script is null! Have you assigned it in the inspector?");
        }

        if (timeScoreUI == null)
        {
            Debug.LogWarning("timeScoreUI variable of Timer script is null! Have you assigned it in the inspector?");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //While timer is active update the time
        if (IncreaseTime == true)
        {
            //Unsure what this line does? Increments by deltaTime?
            TimeScore += Time.deltaTime;

            int seconds = (int)(TimeScore % 60);
            int minutes = (int)(TimeScore / 60) % 60;
            int hours = (int)(TimeScore / 3600) % 24;

            string timeString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            //Set time scores.
            if (timeScoreText != null)
            {
                timeScoreText.text = timeString;
            }

            if (timeScoreUI != null)
            {
                timeScoreUI.text = timeString;
            }

        }
        
    }

    /// <summary>
    /// stops the timer from counting if the pause is activated
    /// </summary>
    void flipPause()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }

        else if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
        }
    }

    /// <summary>
    /// Updates the clam images on the victory screen, and sets the clam bool in level data to true if all collected
    /// </summary>
    public void UpdateClamMedals()
    {
        if (Character.NumberFoundClams == 3)
        {
            VictoryMenu.SetImage(VictoryMenu.ClamImage1, VictoryMenu.ClamYes);
            VictoryMenu.SetImage(VictoryMenu.ClamImage2, VictoryMenu.ClamYes);
            VictoryMenu.SetImage(VictoryMenu.ClamImage3, VictoryMenu.ClamYes);
            
            if (LevelData.levelData != null)
            {
                //Unlock the next level
                LevelData.levelData.myLevels[Level].clams = true; // set clams collection to true
            }

            else
            {
                Debug.LogWarning("Unable to store clam medal data, no LevelData file exists. This won't occur when loading levels through the mainMenu.");
            }
        }

        if (Character.NumberFoundClams == 2)
        {
            VictoryMenu.SetImage(VictoryMenu.ClamImage1, VictoryMenu.ClamYes);
            VictoryMenu.SetImage(VictoryMenu.ClamImage2, VictoryMenu.ClamYes);
        }

        if (Character.NumberFoundClams == 1)
        {
            VictoryMenu.SetImage(VictoryMenu.ClamImage1, VictoryMenu.ClamYes);
        }
    }


}
