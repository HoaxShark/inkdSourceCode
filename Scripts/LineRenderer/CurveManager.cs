using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveManager : MonoBehaviour
{
    //The player's body.
    public Transform Player;

    //The script that handles drawing the line renderer curve
    //public SimpleBezierCurve curve;
    private LineRenderer lineRenderer;

    //The arm that this curve manager is responsible for.
    private ArmMovement arm;

    //If the curve should try to adapt based on the direction of arm movement.
    [SerializeField]
    private bool useSmartCurve = true;

    //The width for the start and end of the arm.
    [SerializeField]
    private float lineWidthStart = 0.5f;

    [SerializeField]
    private float lineWidthEnd = 0.25f;

    //The control points that are used to deform the curve.
    [SerializeField]
    private Transform controlPoint1;

    [SerializeField]
    private Transform controlPoint2;

    //Distance of control point from the bodyToArm vector
    [SerializeField]
    private float controlPoint1Distance = 0.5f;

    [SerializeField]
    private float controlPoint2Distance = 0.5f;

    //Position of the control points on the arm as a percentage.
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float controlPoint1Position = 33.3f;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float controlPoint2Position = 66.6f;

    //Distance during which the arm will curve tightly instead of sine waving.
    [SerializeField]
    private float tightCurveDistance = 5.0f;

    //The vector between the character's body and arm tip.
    private Vector3 bodyToArm;

    //A vector perpendicular to the bodyToArm vector. 
    //It is rotated to give the vector on which to move the control points.
    private Vector3 perpendicularToArm;

    //The position of the control point on the bodyToArm vector.
    private Vector3 control1XPos;
    private Vector3 control2XPos;

    //The position of the control point at right angles to the bodyToArm vector.
    private float control1YPos;
    private float control2YPos;


    // Use this for initialization
    void Start ()
    {
        //Get components
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        arm = gameObject.GetComponent<ArmMovement>();

        //Setup the line renderer default values and create the control points.
        InitLineRenderer();
        CreateControlPoints();

        //curve.lineRenderer = lineRenderer;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (arm.DrawLineRenderer == true)
        {
            UpdateControlPoints();
        }        
	}

    public void ResetControlPoints()
    {
        //Reset control points to body 
        controlPoint1.position = (transform.position - Player.position) * 0.33f;
        controlPoint2.position = (transform.position - Player.position) * 0.66f;
    }

    //Setup line renderer
    void InitLineRenderer()
    {
        //Set sorting order (need to look into this more)
        lineRenderer.sortingLayerName = "OnTop";
        lineRenderer.sortingOrder = 5;

        //Set length of the position array
        lineRenderer.positionCount = 2; //nodes.Length;

        //Update the start of the arm to the body's position
        lineRenderer.SetPosition(0, Player.position);

        //Set the end of the arm to the body (i.e. no arm shown)
        lineRenderer.SetPosition(1, Player.position);

        //Set width of line
        lineRenderer.startWidth = lineWidthStart;
        lineRenderer.endWidth = lineWidthEnd;

        lineRenderer.useWorldSpace = true;

    }

    /// <summary>
    /// Initalise the control points.
    /// </summary>
    void CreateControlPoints()
    {
        //Create two new control points so a cubic bezier curve can be applied
        controlPoint1.position = (Player.position);
        controlPoint2.position = (Player.position);
    }

    /// <summary>
    /// Moves the control points to update the curve for the line renderer.
    /// </summary>
    void UpdateControlPoints()
    {
        //Caluclate vector between body and arm.
        bodyToArm = (transform.position - Player.position);

        //Place controls on arm
        control1XPos = bodyToArm * (controlPoint1Position / 100);
        control2XPos = bodyToArm * (controlPoint2Position / 100);

        //Get vector perpendicular to the vector between body and arm
        perpendicularToArm = Vector3.Cross(bodyToArm, Vector3.up).normalized;
        perpendicularToArm = RotateAroundPoint(bodyToArm, perpendicularToArm, Quaternion.Euler(0, 0, 90));

        //Check if the curve should try to adapt to arm location
        if (useSmartCurve)
        {
            CheckCurveDirection();
        }

        else
        {
            control1YPos = controlPoint1Distance;
            control2YPos = controlPoint2Distance;
        }


        //Get current control point positions
        Vector3 currentPos1 = controlPoint1.position;
        Vector3 currentPos2 = controlPoint2.position;

        //Get target positions
        Vector3 target1 = (Player.position + control1XPos) + (perpendicularToArm * control1YPos);
        Vector3 target2 = (Player.position + control2XPos) + (perpendicularToArm * control2YPos);

        Vector3 lerpPos = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 lerpPos2 = new Vector3(0.0f, 0.0f, 0.0f);

        //Lerp between current control point pos and target control point pos, using delta time to achieve smooth movement.
        lerpPos = new Vector3(Mathf.Lerp(currentPos1.x, target1.x, Time.deltaTime), Mathf.Lerp(currentPos1.y, target1.y, Time.deltaTime), controlPoint1.position.z);
        lerpPos2 = new Vector3(Mathf.Lerp(currentPos2.x, target2.x, Time.deltaTime), Mathf.Lerp(currentPos2.y, target2.y, Time.deltaTime), controlPoint2.position.z);

        //Update control point positions. This will cause the line renderer to move to adhere to the control points, creating the wavy arm movement.
        controlPoint1.position = lerpPos;
        controlPoint2.position = lerpPos2;

        //Draw bodyToArm vector
        Debug.DrawLine(transform.position, Player.position);
    }

    /// <summary>
    /// Check the direction of the bodyToArm vector so the arm curve can be changed to look more natural depending on direction./// 
    /// </summary>
    void CheckCurveDirection()
    {
        int sign = 1;
        lineRenderer.material.SetTextureOffset("_BaseColorMap", new Vector2(0.5f, 0.0f));

        if (bodyToArm.x < 0)
        {
            sign = -1;
            lineRenderer.material.SetTextureOffset("_BaseColorMap", new Vector2(0.0f, 0.0f));
        }

        //If the arm is grabbed onto something
        if (arm.Grabbed == true)
        {
            
        }

        //The arms should do a small tight curve when near the body
        if((bodyToArm.x <= tightCurveDistance) && (bodyToArm.x >= -tightCurveDistance))
        {
            if (bodyToArm.y >= 0)
            {
                control1YPos = -controlPoint1Distance * sign;
                control2YPos = -controlPoint2Distance * sign;
            }

            else if (bodyToArm.y < 0)
            {
                control1YPos = controlPoint1Distance * sign;
                control2YPos = controlPoint2Distance * sign;
            }

            return;
        }

        //Arms do a longer sine wave curve when further away
        if (bodyToArm.y >= 0)
        {
            control1YPos = -controlPoint1Distance * sign;
            control2YPos = controlPoint2Distance * sign;
        }

        else if (bodyToArm.y < 0)
        {
            control1YPos = controlPoint1Distance * sign;
            control2YPos = -controlPoint2Distance * sign;
        }

        return;
    }

    /// <summary>
    /// Roatate a vector around a pivot. Source: https://answers.unity.com/questions/47115/vector3-rotate-around.html
    /// This is used to add the arm's control points perpendicular to the arm. 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="pivot"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }

}
