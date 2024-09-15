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


    private void Shooting()
    {
        // Check for a clear line of sight before shooting
        RaycastHit hit;
        Vector3 directionToPlayer = Player.position - transform.position;
        // Perform the raycast to check for obstacles between enemy and player
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, Shootrange))
        {
            // Check if the raycast hit the player
            if (hit.transform.CompareTag("Player"))
            {
                // Make the enemy face the player
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation

                Agent.SetDestination(Player.position);
                if (!Isshooting)
                {
                    // Calculate the direction towards the player
                    Vector3 shootDirection = (Player.position - Shotpostion.position).normalized;

                    // Instantiate the bullet
                    GameObject bulletInstance = Instantiate(Bullet, Shotpostion.position, Quaternion.identity);
                    Rigidbody body = bulletInstance.GetComponent<Rigidbody>();

                    Collider enemyCollider = GetComponent<Collider>();
                    Collider bulletCollider = bulletInstance.GetComponent<Collider>();

                    // Ignore collision between enemy and bullet
                    Physics.IgnoreCollision(enemyCollider, bulletCollider);

                    //enemy no shoot himself
                    Physics.IgnoreCollision(enemyCollider, bulletCollider);

                    body.AddForce(shootDirection * shootForce, ForceMode.Impulse);
                    body.AddForce(transform.up * shootUpForce, ForceMode.Impulse);


                    //
                    Isshooting = true;
                    Invoke(nameof(ResetShooting), shootrate);
                }
            }
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


}

