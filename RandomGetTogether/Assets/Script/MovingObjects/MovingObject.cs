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
        targetWaypointIndex = movingPatht.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = movingPatht.GetWayPoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWayPoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
