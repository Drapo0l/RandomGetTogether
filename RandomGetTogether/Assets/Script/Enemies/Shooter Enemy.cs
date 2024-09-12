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

        //Agent.SetDestination(GameManager.Instance.Player.transform.position);        
        //if (!Isshooting)
        //{
        //    StartCoroutine(shooting());
        //}
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
            Shooting();

        }

    }

    //private void shooting()
    //{
    //    Agent.SetDestination(transform.position);
    //    transform.LookAt(Player);

    //    if (!Isshooting)
    //    {
    //        Debug.Log("Shooting");
    //        //  Debug.Log("Shooting");
    //        //Shooting(); 
    //        //Isshooting = true;

    //        Rigidbody rb = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //        rb.AddForce(transform.forward * 32, ForceMode.Impulse);
    //        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
    //        Isshooting = true; 

    //    }
    //}

    private void Shooting()
    {
        //enemy does not move
        Agent.SetDestination(transform.position);
        transform.LookAt(Player);

        if (!Isshooting)
        {
            //attack code 
            // Instantiate the bullet
            GameObject bulletInstance = Instantiate(Bullet, transform.position, Quaternion.identity);
            Rigidbody body = bulletInstance.GetComponent<Rigidbody>();

            Collider enemyCollider = GetComponent<Collider>();
            Collider bulletCollider = bulletInstance.GetComponent<Collider>();
            //enemy no shoot himself
            Physics.IgnoreCollision(enemyCollider, bulletCollider);

            body.AddForce(transform.forward * 32f, ForceMode.Impulse);
            body.AddForce(transform.up * 8f, ForceMode.Impulse);


            //
            Isshooting = true;
            Invoke(nameof(ResetShooting), shootrate);
        }
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
