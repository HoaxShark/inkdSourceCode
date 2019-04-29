using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // used to read and write to files
using System.Runtime.Serialization.Formatters.Binary; // used to serialize data

public class LevelData : MonoBehaviour
{
    //Allow other scripts to easily reference this one.
    public static LevelData levelData;

    //Number of levels in the game.
    public int numberOfLevels;

    //List of all levels.
    public List<Level> myLevels = new List<Level>(); 

    // Use this for initialization
    void Awake ()
    {
        //If no levelData yet make this the levelData
        if (levelData == null)
        {
            DontDestroyOnLoad(gameObject);
            levelData = this;
            ReadList();
        }

        else if (levelData != this)
        {
            Destroy(gameObject);
        }        
	}

    /// <summary>
    /// Saves current list of levels to the save.dat file
    /// </summary>
    public void SaveList()
    {
        FileStream fs = new FileStream("save.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, myLevels);
        fs.Close();
    }

    /// <summary>
    /// Reads the data from the save file and stores it in the myLevels list, if file doesn't exist populates list of levels and saves the file
    /// </summary>
    void ReadList()
    {
        if (System.IO.File.Exists("save.dat"))
        {
            using (Stream stream = File.Open("save.dat", FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                stream.Position = 0;
                myLevels = (List<Level>)bformatter.Deserialize(stream);
                Debug.Log("all levels loaded from list");
            }
        }
        else
        {
            PopulateList();
        }
    }

    /// <summary>
    /// Populates myLevels list to the number of levels and saves to save.dat
    /// </summary>
    void PopulateList()
    {
        for (int i = 0; i < numberOfLevels; i++)
        {
            Level newLevel = new Level();
            myLevels.Add(newLevel);
            Debug.Log("Level added");
        }

        // Make sure level one is unlocked
        myLevels[0].unlocked = true;
        SaveList();
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            Debug.Log("saving list");
            SaveList();
        }

        if (Input.GetKeyDown("p"))
        {
            Debug.Log(myLevels[0].clams);
        }
    }
}
