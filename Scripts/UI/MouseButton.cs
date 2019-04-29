using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseButton : MonoBehaviour
{
    private Image currentButtonImage;

    [SerializeField]
    private Sprite noneDown;

    [SerializeField]
    private Sprite leftDown;

    [SerializeField]
    private Sprite rightDown;

    [SerializeField]
    private Sprite bothDown;

    // Use this for initialization
    void Start ()
    {
        currentButtonImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            currentButtonImage.sprite = noneDown;
        }

        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            currentButtonImage.sprite = leftDown;
        }

        if (!Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            currentButtonImage.sprite = rightDown;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            currentButtonImage.sprite = bothDown;
        }
    }
}
