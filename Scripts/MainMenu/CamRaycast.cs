using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CamRaycast : MonoBehaviour
{
    Animator anim;

    public Canvas textCanvas;


    public Canvas levelSelectCanvas;

    public Canvas optionsCanvas;

    public GameObject optionsBlock;


    // Use this for initialization
    void Start()
    {


        anim = gameObject.GetComponent<Animator>();

        textCanvas.enabled = true;
        levelSelectCanvas.enabled = true;
        optionsCanvas.enabled = true;

        optionsBlock.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Invoke("BacktoDefault", 1.5f);

            anim.SetTrigger("Back");

            optionsCanvas.enabled = true;

            optionsBlock.SetActive(true);
        }

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.name == "Options")
            {
                Debug.Log("Go to Whiteboard");

                anim.SetTrigger("OptionsGo");

                optionsCanvas.enabled = true;

                optionsBlock.SetActive(false);
            }

            if (hit.collider.name == "Levels")
            {
                Debug.Log("Go to Levels");

                anim.SetTrigger("LevelGo");

                Invoke("GotoLevels", 1f);
            }

            if (hit.collider.name == "Quit")
            {
                Debug.Log("Go to Exit Door");

                anim.SetTrigger("ExitGo");

                Invoke("QuitGame", 2f);
            }

            if (hit.collider.name == "Credits")
            {
                Debug.Log("Go to Stack of Papers");

                anim.SetTrigger("CreditsGo");
            }

            if (hit.collider.name == "Gallery")
            {
                Debug.Log("Go to Wall with Pictures");

                anim.SetTrigger("GalleryGo");
            }
        }
    }

    void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit Game");
    }

    void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial V3");
    }

}
