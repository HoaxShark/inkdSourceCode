using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class for main menu, inherits from BaseUI.
/// </summary>
public class MainMenu : BaseUI
{
    public Canvas levelSelectPanel;
    public Canvas mainMenuPanel;
    public Canvas optionsMenu;
    public Canvas galleryMenu;

    //The image components for each levels medals
    /*public Image Level1Clams = null;
    public Image Level1Friend = null;
    public Image Level1Time = null;

    public Image Level2Clams = null;
    public Image Level2Friend = null;
    public Image Level2Time = null;

    public Image Level3Clams = null;
    public Image Level3Friend = null;
    public Image Level3Time = null;*/


    //The levels displayed on the menu
    public LevelUI level1_1;
    public LevelUI level1_2;
    public LevelUI level1_3;



    [SerializeField]
    private string menuName;

    //This is duplication of the code in VictoryMenu. There should be a base UI class that deals with image loading etc.
    //Sprites for the medal tick and cross
    [SerializeField]
    private Sprite tick;

    [SerializeField]
    private Sprite cross;

    public Sprite Tick
    {
        get
        {
            return tick;
        }

        set
        {
            tick = value;
        }
    }

    public Sprite Cross
    {
        get
        {
            return cross;
        }

        set
        {
            cross = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        levelSelectPanel.enabled = true;
        mainMenuPanel.enabled = true;
    }

    void Update()
    {    }

    public void LevelSelect ()
    {
        levelSelectPanel.enabled = true;
        mainMenuPanel.enabled = false;
	}

   /*
    *public void OptionsMenu ()
    {
        optionsMenu.enabled = true;
        mainMenuPanel.enabled = false;
    }
    */

    public void GalleryMenu ()
    {
        galleryMenu.enabled = true;
        mainMenuPanel.enabled = false;
    }

    public void MenuSelect()
    {
        levelSelectPanel.enabled = false;
        mainMenuPanel.enabled = true;
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene(menuName);

        mainMenuPanel.enabled = true;
        levelSelectPanel.enabled = false;
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
