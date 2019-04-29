using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoArmRagdoll : MonoBehaviour
{
    public float defaultSpringValue;
    public float pullInStrength;

    private bool isBeingDragged;
    private bool isClinging;
    private Rigidbody armRigidbody;
    private Collider armCollider;
    private SpringJoint armSpring;
    private Transform armParent;
    private Transform armTransform;
    private Vector3 armAttachPosition;
    private ConfigurableJoint clingJoint;

    public bool IsBeingDragged
    {
        get
        {
            return isBeingDragged;
        }

        set
        {
            isBeingDragged = value;
        }
    }

    public bool IsClinging
    {
        get
        {
            return isClinging;
        }

        set
        {
            isClinging = value;
        }
    }

    public Rigidbody ArmRigidbody
    {
        get
        {
            return armRigidbody;
        }

        set
        {
            armRigidbody = value;
        }
    }

    public Collider ArmCollider
    {
        get
        {
            return armCollider;
        }

        set
        {
            armCollider = value;
        }
    }

    public SpringJoint ArmSpring
    {
        get
        {
            return armSpring;
        }

        set
        {
            armSpring = value;
        }
    }

    public Transform ArmParent
    {
        get
        {
            return armParent;
        }

        set
        {
            armParent = value;
        }
    }

    public Transform ArmTransform
    {
        get
        {
            return armTransform;
        }

        set
        {
            armTransform = value;
        }
    }

    public Vector3 ArmAttachPosition
    {
        get
        {
            return armAttachPosition;
        }

        set
        {
            armAttachPosition = value;
        }
    }

    public ConfigurableJoint ClingJoint
    {
        get
        {
            return clingJoint;
        }

        set
        {
            clingJoint = value;
        }
    }



    /*--------------------------------
                 Methods
    --------------------------------*/

    private void Awake()
    {
        IsBeingDragged = false;
        IsClinging = false;
    }

    // Use this for initialization
    void Start ()
    {
        ArmRigidbody = gameObject.GetComponent<Rigidbody>();
        //ArmSpring = gameObject.GetComponent<SpringJoint>();
        ArmTransform = ArmRigidbody.GetComponent<Transform>();
        ArmParent = ArmTransform.parent;
        //ArmSpring.spring = defaultSpringValue;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        //If clinging, release all arms on keypress (could be number keys for arms? Or a cylce through from last to first)
        if (Input.GetKey(KeyCode.B) && (IsClinging == true))
        {
            Release();
        }

        //If clinging and right mouse is held increase spring strength to reel in
        if (Input.GetMouseButton(1) && (IsClinging == true))
        {
            //ArmSpring.spring = pullInStrength;
        }

        // If right mouse is released set spring strength to default
        if (Input.GetMouseButtonUp(1))
        {
            //ArmSpring.spring = defaultSpringValue;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isClinging == false)
        {
            Cling(collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isClinging == false)
        {
            Cling(collision);
        }
    }

    private void Cling(Collision collision)
    {
        //Check if arm should cling to the object and reset Spring value
        if ((collision.collider.tag == "Climbable") && (!Input.GetMouseButton(0)) && (isBeingDragged == true)) 
        {
            Debug.Log("cling");
            //ArmRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            ArmRigidbody.isKinematic = true;


            IsClinging = true;        
        }
    }

    public void Release()
    {
        //ArmRigidbody.constraints &= ~RigidbodyConstraints.FreezeAll;

        ArmRigidbody.isKinematic = false;

        IsClinging = false;
        Debug.Log("Uncling");        
    }
}
