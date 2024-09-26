using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] MovingPatht movingPatht;
    [SerializeField] float speed;  
    [SerializeField] bool SlowDownOnNextPoint;

    float timeToWaypoint;
    float elapsedTime;
    int targetWaypointIndex;
     
    Transform targetWaypoint; 
    Transform previousWayPoint;


    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    void FixedUpdate()
    {
        if (previousWayPoint == null || targetWaypoint == null)
        {
            Debug.LogError("Waypoints are not set correctly.");
            return; // Exit if waypoints are not initialized
        }

        elapsedTime += Time.deltaTime;
        float elapsedPercentage = elapsedTime / timeToWaypoint;

        if (SlowDownOnNextPoint)
        {
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        }

        transform.position = Vector3.Lerp(previousWayPoint.position, targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(previousWayPoint.rotation, targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    void TargetNextWaypoint()
    {
        previousWayPoint = movingPatht.GetWayPoint(targetWaypointIndex);
        if (previousWayPoint == null)
        {
            Debug.LogError($"Previous waypoint at index {targetWaypointIndex} is null!");
            return; // Exit if previous waypoint is null
        }

        targetWaypointIndex = movingPatht.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = movingPatht.GetWayPoint(targetWaypointIndex);
        if (targetWaypoint == null)
        {
            Debug.LogError($"Target waypoint at index {targetWaypointIndex} is null!");
            return; // Exit if target waypoint is null
        }

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWayPoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}