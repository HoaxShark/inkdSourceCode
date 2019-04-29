using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtons : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas levelCanvas;
    public Canvas optionsCanvas;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        levelCanvas.enabled = false;
        //menuCanvas.enabled = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            optionsCanvas.enabled = false;
            anim.SetTrigger("BackOptions");

            levelCanvas.enabled = false;
            anim.SetTrigger("BackLevel");
        }
    }

    // Update is called once per frame
    public void OptionsMenu()
    {
        Invoke("OpenOptions", 1);

        anim.SetTrigger("ForwardOptions");
    }

    void OpenOptions()
    {
        optionsCanvas.enabled = true;
    }

    public void LevelMenu()
    {
        Invoke("OpenLevel", 1);

        anim.SetTrigger("ForwardLevel");
    }

    void OpenLevel()
    {
        levelCanvas.enabled = true;
    }

}
