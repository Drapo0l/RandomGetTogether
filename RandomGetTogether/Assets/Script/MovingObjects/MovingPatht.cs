using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPatht : MonoBehaviour
{
   public Transform GetWayPoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWayPointIndex = currentWaypointIndex + 1;

        if (nextWayPointIndex == transform.childCount)
        {
            nextWayPointIndex = 0;
        }
        return nextWayPointIndex;
    }
}
