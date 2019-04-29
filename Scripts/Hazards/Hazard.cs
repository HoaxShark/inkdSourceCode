using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The damage of the hazard. Not need for it to be higher than 1.")]
    private int damage;

    [Header("Timer")]

    [SerializeField]
    [Tooltip("If the hazard should have a timed activation.")]
    private bool timed = false;

    [SerializeField]
    [Tooltip("The timer interval")]
    private float onOffTime;
    

    //If the hazard is active and can deal damage.
    private bool isActive = true;

    //The base interval if the hazard is timed.
    private float onOffTimeBase; 

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }


    // Use this for initialization
    void Start ()
    {
        onOffTimeBase = onOffTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(timed)
        {
            CheckTimer();
        }
    }

    void CheckTimer()
    {
        onOffTime -= Time.deltaTime; // count down the timer

        if (onOffTime < 0)
        {
            IsActive = !IsActive; // flip active
            onOffTime = onOffTimeBase; // reset timer
        }
    }


}
