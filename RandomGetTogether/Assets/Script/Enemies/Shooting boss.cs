using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shootingboss : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject player;
    public float EnemyDistance = 4.0f;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);  
        Debug.Log("Distance: " + distance);
        if (distance < EnemyDistance) { 
          Vector3 DistancePlayer =transform.position - player.transform.position;
            Vector3 NewPos = transform.position + DistancePlayer;
            Agent.SetDestination(NewPos); 

        }
    }
}



