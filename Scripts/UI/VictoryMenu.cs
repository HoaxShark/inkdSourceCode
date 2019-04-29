using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class for the victory screen, inherits from BaseUI
/// </summary>
public class VictoryMenu : BaseUI
{
    [Header("Image components for medals/clams.")]

    [SerializeField]
    private Image timeMedal;

    [SerializeField]
    private Image clamImage1;

    [SerializeField]
    private Image clamImage2;

    [SerializeField]
    private Image clamImage3;

    [Space(10)]

    [Header("Sprites for medals/clams.")]

    [SerializeField]
    [Tooltip("Sprite for the gold medal.")]
    private Sprite goldMedalSprite;

    [SerializeField]
    [Tooltip("Sprites for the silver medal.")]
    private Sprite silverMedalSprite;

    [SerializeField]
    [Tooltip("Sprites for the bronze medal.")]
    private Sprite bronzeMedalSprite;

    [SerializeField]
    [Tooltip("Sprite for an unearned medal.")]
    private Sprite noMedalSprite;

    [Space(10)]

    [SerializeField]
    [Tooltip("Sprite for found clam.")]
    private Sprite clamYes;

    [SerializeField]
    [Tooltip("Sprite for unfound clam.")]
    private Sprite clamNo;

    [Space(10)]

    [Header("Other")]

    [SerializeField]
    [Tooltip("The text display for number of clams found.")]
    private Text clamCountText;


    public Sprite ClamYes
    {
        get
        {
            return clamYes;
        }

        set
        {
            clamYes = value;
        }
    }

    public Sprite GoldMedalSprite
    {
        get
        {
            return goldMedalSprite;
        }

        set
        {
            goldMedalSprite = value;
        }
    }

    public Sprite SilverMedalSprite
    {
        get
        {
            return silverMedalSprite;
        }

        set
        {
            silverMedalSprite = value;
        }
    }

    public Sprite BronzeMedalSprite
    {
        get
        {
            return bronzeMedalSprite;
        }

        set
        {
            bronzeMedalSprite = value;
        }
    }

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

    public Image ClamImage1
    {
        get
        {
            return clamImage1;
        }

        set
        {
            clamImage1 = value;
        }
    }

    public Image ClamImage2
    {
        get
        {
            return clamImage2;
        }

        set
        {
            clamImage2 = value;
        }
    }

    public Image ClamImage3
    {
        get
        {
            return clamImage3;
        }

        set
        {
            clamImage3 = value;
        }
    }

    public Text ClamCountText
    {
        get
        {
            return clamCountText;
        }

        set
        {
            clamCountText = value;
        }
    }


    // Use this for initialization
    void Start ()
    {
        //Set clams to unfound by default.
        ClamImage1.sprite = clamNo;
        ClamImage2.sprite = clamNo;
        ClamImage3.sprite = clamNo;
	}

}
