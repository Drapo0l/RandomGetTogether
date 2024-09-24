using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shootingboss : MonoBehaviour
{
    [SerializeField] NavMeshAgent Agent; 
    [SerializeField] float EnemyDistance;
    void Start()    
    {
        Agent.SetDestination(GameManager.Instance.Player.transform.position); 
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);  // calucates the distance of the player
        Debug.Log("Distance: " + distance); // to see how far you are from the boss
        if (distance < EnemyDistance) {  // if it gets close to the player it goes to its new postition from it
          Vector3 DistancePlayer =transform.position - GameManager.Instance.Player.transform.position;
            Vector3 NewPos = transform.position + DistancePlayer;
            Agent.SetDestination(NewPos); 

        }
    }
}



