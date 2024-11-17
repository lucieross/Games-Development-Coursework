using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Array of waypoints for the platform to go between
    [SerializeField] private float speed = 2f; // Speed of platform
    [SerializeField] private float checkDistance = 0.05f;
    private Transform targetWaypoint; //target waypoint
    private int currentWaypointIndex = 0; // Index of current waypoint

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0) {
            targetWaypoint = waypoints[0]; // Sets the first waypoint as the target
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the platform towards the target waypoint
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // check if the platform is close enough to the target waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance) {
            targetWaypoint = GetNextWaypoint(); // Gets the next waypoint
        }
    }

    // Gest the next waypoint in the array
    private Transform GetNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length) {
            currentWaypointIndex = 0; 
        }

        return waypoints[currentWaypointIndex];
    }

    
}

