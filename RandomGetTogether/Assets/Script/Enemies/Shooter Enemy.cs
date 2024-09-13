using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterEnemy : MonoBehaviour, iDamage
{
    [Header("Basics")]    
    public NavMeshAgent Agent;   
    [SerializeField] int HP;
    [SerializeField] Renderer Model;
    public Transform Player;
    public LayerMask Ground, WherePlayer;

    [Header("Bullet")]
    [SerializeField] Transform Shotpostion;
    [SerializeField] GameObject Bullet;
    [SerializeField] float shootrate;
    [SerializeField] float shootForce;
    [SerializeField] float shootUpForce;
    Color colorOrig;
    bool Isshooting; 
    
    //Patroling
    [Header("Patroll")]
    public Vector3 WalkPoint;
    bool IsWalking;
    [SerializeField] float walkpointRange;

    //States
    [Header("States")]
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

            body.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            body.AddForce(transform.up * shootUpForce, ForceMode.Impulse);


            //
            Isshooting = true;
            Invoke(nameof(ResetShooting), shootrate);
        }
    }
    IEnumerator flashColor()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(.15f);
        Model.material.color = colorOrig;
    }
    public void takeDamage(int amount)
    {
        
        HP -= amount;
        StartCoroutine(flashColor());
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
