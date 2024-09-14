using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterEnemy : MonoBehaviour, DamageFE 
{
    [SerializeField] Renderer Model;

    public NavMeshAgent Agent;
    [SerializeField] Transform Shotpostion;
    [SerializeField] int HP;
    public Transform Player;
    [SerializeField] GameObject Bullet;
    [SerializeField] float shootrate;
    Color colorOrig;
    bool Isshooting; 
    public LayerMask Ground, WherePlayer;
    //Patroling
    public Vector3 WalkPoint;
    bool IsWalking;
    [SerializeField] float walkpointRange;

    //States
    [SerializeField] float Sightrange;
    [SerializeField] float Shootrange;
    bool isinSight;
    bool isinRange;
    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
        colorOrig = Model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        isinSight = Physics.CheckSphere(transform.position, Sightrange, WherePlayer);
        isinRange = Physics.CheckSphere(transform.position, Shootrange, WherePlayer);

        if (!isinSight)
        {
            PatrolingArea();

        }
        if (isinSight && !isinRange)
        {
            Chasing();
        }
        if (isinSight && isinRange)
        {
            StartCoroutine( Shooting());  

        }

    }

   
    IEnumerator Shooting()
    {
        Agent.SetDestination(transform.position);
        transform.LookAt(Player);
        Isshooting = true;
        Instantiate(Bullet, Shotpostion.position, transform.rotation);
        yield return new WaitForSeconds(shootrate);
        Isshooting = false;

    }
    IEnumerator flashColor()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        Model.material.color = colorOrig;
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

    public void PatrolingArea()
    {
        if (!IsWalking)
        {
            SearchWalkroad();
        }
        if (IsWalking)
        {
            Agent.SetDestination(WalkPoint);
        }

        Vector3 DistanceWalking = transform.position - WalkPoint;

        if (DistanceWalking.magnitude < 1f)
        {
            IsWalking = false;
        }
    }
    private void SearchWalkroad()
    {
        float RandomZ = Random.Range(-walkpointRange, walkpointRange);
        float RandomX = Random.Range(-walkpointRange, walkpointRange);
        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, Ground))
        {
            IsWalking = true;
        }

    }
    public void Chasing()
    {
        Agent.SetDestination(Player.position);
    }

    private void ResetShooting()
    {
        Isshooting = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, Shootrange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, Sightrange);
    //}
}
