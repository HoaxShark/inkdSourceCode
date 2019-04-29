using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BezierCurve script taken from:
/// http://www.theappguruz.com/blog/bezier-curve-in-games
/// 
/// 
/// 
/// </summary>

//[RequireComponent(typeof(LineRenderer))]

public class SimpleBezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;

    private ArmMovement arm;

    private int curveCount = 0;
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;

    bool doLineReset = true;

    void Start()
    {
        arm = gameObject.GetComponent<ArmMovement>();

        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.sortingLayerID = layerOrder;

        //Split the points into curves of 3 points each, this allows us to use a cubic function on each curve instead of something more complex.
        curveCount = (int)controlPoints.Length / 3;
    }

    void Update()
    {
        //Checks if the line renderer should be drawn
        if (arm.DrawLineRenderer == true)
        {
            DrawCurve();

            //Once line has been drawn, the line is ready to be reset the next time the line isn't updated.
            doLineReset = true;
        }

        //Reset the line
        else if (doLineReset == true)
        {
            //Draws a dummy curve outside of the camera view
            DrawPlaceholderCurve();
            doLineReset = false;
        }        
    }

    /// <summary>
    /// Draws a bezier curve with the line renderer, based on the control points.
    /// </summary>
    void DrawCurve()
    {
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;

                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, 
                                                             controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);

                lineRenderer.positionCount = ((j * SEGMENT_COUNT) + i);
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
            }
        }
    }

    /// <summary>
    /// Draws the line renderer but just draws it at a position past the camera viewport. This effectively "clears" the line renderer from the screen.
    /// Used when we don't want to update the line renderer to avoid lag.
    /// Some code duplication from DrawCurve, should look into making this better.
    /// </summary>
    void DrawPlaceholderCurve()
    {
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                Vector3 pixel = new Vector3(0.0f, 0.0f, 100.0f);

                lineRenderer.positionCount = ((j * SEGMENT_COUNT) + i);
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
            }
        }
    }

    /// <summary>
    /// Calculate placement of points for a cubic bezier curve
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"> First control point. In the octopus arm, this will always be the body.</param>
    /// <param name="p1"> Second point. In the octopus arm, this will be ControlPoint1.</param>
    /// <param name="p2"> Third point. In the octopus arm, this will be ControlPoint2.</param>
    /// <param name="p3"> Fourth point. In the octopus arm, this will always be the end of the arm.</param>
    /// <returns></returns>
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}