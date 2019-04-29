using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code taken from https://answers.unity.com/questions/894796/how-to-make-object-follow-path.html

public class WayPoints : MonoBehaviour
{
    [SerializeField]
    private bool alwaysForwardFacing = true;
    // put the points from unity interface
    [SerializeField]
    private Transform[] wayPointList;

    //Index of current waypoint
    [SerializeField]
    private int currentWayPoint = 0;

    //Speed of movement between waypoints
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private bool isLooping = false;

    //Next waypoint to walk to
    Transform targetWayPoint;

    //private bool isForward;
    private Direction currentDirection = Direction.Forwards;
    private Direction lastDirection;

    enum Direction {Forwards, Backwards, None};

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If the waypoint system is looping
        if (isLooping == true)
        {
            //Get a current direction if known.
            currentDirection = CheckDirection();

            if (currentDirection == Direction.Backwards)
            {
                TraverseBackwards();
            }

            else if (currentDirection == Direction.Forwards)
            {
                TraverseForwards();
            }

            //Othewise use last known direction
            else
            {
                Traverse(lastDirection);
            }
        }

        //When not looping, simply go forwards
        else
        {
            TraverseForwards();
        }

        /*
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
            {
                targetWayPoint = wayPointList[currentWayPoint];
            }

            Walk();
        }

        //If waypoints are on a loop, reset to start when the end of the waypoints are reached
        else if ((currentWayPoint == this.wayPointList.Length) && (isLooping == true))
        {
            //targetWayPoint = wayPointList[0];

            //Walk in reverse
            Walk(true);
        }*/
    }

    /// <summary>
    /// Check the direction of traversal
    /// </summary>
    /// <returns></returns>
    Direction CheckDirection()
    {
        //At end of waypoints
        if (currentWayPoint == wayPointList.Length - 1)
        {
            lastDirection = Direction.Backwards;
            return Direction.Backwards;
        }

        else if (currentWayPoint == 0)
        {
            lastDirection = Direction.Forwards;
            return Direction.Forwards;
        }

        return Direction.None;
    }

    /// <summary>
    /// Traverse through the list of waypoints.
    /// </summary>
    /// <param name="direction">The direction to traverse in.</param>
    void Traverse(Direction direction)
    {
        if (direction == Direction.Forwards)
        {
            TraverseForwards();
        }

        else if (direction == Direction.Backwards)
        {
            TraverseBackwards();
        }
    }

    /// <summary>
    /// Traverse forwards through the waypoints.
    /// </summary>
    void TraverseForwards()
    {
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
            {
                targetWayPoint = wayPointList[currentWayPoint];
            }

            Walk();
        }
    }

    /// <summary>
    /// Traverse backwards through the waypoints.
    /// </summary>
    void TraverseBackwards()
    {
        // check if we have somewere to walk
        if (currentWayPoint >= 0)
        {
            if (targetWayPoint == null)
            {
                targetWayPoint = wayPointList[currentWayPoint];
            }

            Walk(Direction.Backwards);
        }
    }

    /// <summary>
    /// Walk to the next waypoint.
    /// </summary>
    /// <param name="isReversed">If true, the waypoints will be walked through in reverse.</param>
    void Walk(Direction direction=Direction.Forwards)
    {
        if (alwaysForwardFacing)
        {
            // rotate towards the target
            transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);
        }
        
        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
        {
            //Iterate over waypoints in reverse
            if (direction == Direction.Backwards)
            {
                currentWayPoint--;
            }

            //Otherwise iterate forwards
            else
            {
                currentWayPoint++;
            }

            //Update the target waypoint
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
}
