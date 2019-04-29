using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerMedals
{
    //Could make this a map of <String name : Bool earned> then iterate through and update menu images for each one that is true?
    private static bool time, friend, clam, 
                        time2, friend2, clam2;

    public static bool Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    public static bool Friend
    {
        get
        {
            return friend;
        }

        set
        {
            friend = value;
        }
    }

    public static bool Clam
    {
        get
        {
            return clam;
        }

        set
        {
            clam = value;
        }
    }

    public static bool Time2
    {
        get
        {
            return time2;
        }

        set
        {
            time2 = value;
        }
    }

    public static bool Friend2
    {
        get
        {
            return friend2;
        }

        set
        {
            friend2 = value;
        }
    }

    public static bool Clam2
    {
        get
        {
            return clam2;
        }

        set
        {
            clam2 = value;
        }
    }

}