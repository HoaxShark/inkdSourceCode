using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scroll main texture based on time
/// </summary>

public class ScrollTexture : MonoBehaviour
{


    [SerializeField]
    private float scrollSpeed = 0.5f;

    private float direction;

    //Renderer
    Renderer rend;

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
        direction = gameObject.GetComponent<Conveyor>().Direction.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Scroll through texture over time
        float offset = Time.time * scrollSpeed * direction;
        rend.material.SetTextureOffset("_BaseColorMap", new Vector2(offset, 0));
    }
}
