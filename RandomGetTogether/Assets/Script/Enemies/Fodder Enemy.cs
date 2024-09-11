using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FodderEnemy : MonoBehaviour, DamageFE
{
    [SerializeField] Renderer Model;
    [SerializeField] int HP;
    public NavMeshAgent agent;
    public Transform player;  
    public LayerMask Ground, WherePlayer; 
    //Patroling
    public Vector3 WalkPoint;
    bool IsWalking;
    [SerializeField] float walkpointRange;
    Color colorOrig;        
    //States
    [SerializeField] float Sightrange;  
     bool isinSight;


    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        colorOrig = Model.material.color;
        //agent.SetDestination(GameManager.Instance.Player.transform.position);      
    }

    // Update is called once per frame
    void Update()
    {
        isinSight = Physics.CheckSphere(transform.position, Sightrange,WherePlayer);
        if (!isinSight)
        {
            Patroling(); 

        }
        if (isinSight)
        {
            Chase();
        }
    }

    public void takeDamge(int amount)
    {
        HP -= amount;
        flashColor();
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void Patroling()
    { 
        if (!IsWalking) 
        {
            SearchWalkpath();
        }
        if (IsWalking)
        {
            agent.SetDestination(WalkPoint);
        }

        Vector3 DistanceWalking = transform.position - WalkPoint;

        if (DistanceWalking.magnitude < 1f)
        {
            IsWalking = false;
        }
    }
    private void SearchWalkpath()
    {
        float RandomZ = Random.Range(-walkpointRange, walkpointRange);
        float RandomX = Random.Range(-walkpointRange, walkpointRange);
        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, Ground))
        {
            IsWalking = true;
        }

    }
    public void Chase()
    {
        agent.SetDestination(player.position); 
    }

    IEnumerator flashColor()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        Model.material.color = colorOrig;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
           
    //    }
    //}

    //void resetInvincibility()
    //{
    //    Isbump = false;
    //}
}
