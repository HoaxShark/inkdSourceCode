using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [Header("Speed medal times. All times are in seconds.")]

    [Tooltip("Time limit for a gold speed medal.")]
    [SerializeField]
    private float goldTime = 60.0f;

    [Tooltip("Time limit for a silver speed medal.")]
    [SerializeField]
    private float silverTime = 120.0f;

    [Tooltip("Time limit for a bronze speed medal.")]
    [SerializeField]
    private float bronzeTime = 180.0f;


    public float GoldTime
    {
        get
        {
            return goldTime;
        }

        set
        {
            goldTime = value;
        }
    }

    public float SilverTime
    {
        get
        {
            return silverTime;
        }

        set
        {
            silverTime = value;
        }
    }

    public float BronzeTime
    {
        get
        {
            return bronzeTime;
        }

        set
        {
            bronzeTime = value;
        }
    }
}
