using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


/// <summary>
/// Deprecated. Use BaseUI and its derived classes instead!
/// </summary>
public class UIScript : MonoBehaviour
{
    [SerializeField]
    private string Level1Name = "";

    [SerializeField]
    private string Level2Name = "";

    [SerializeField]
    private string MenuName = "";


    public void LoadMenu()
    {
        SceneManager.LoadScene(MenuName);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(Level1Name);
    }
}
