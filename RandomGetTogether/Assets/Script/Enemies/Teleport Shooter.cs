using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TeleportShooter : MonoBehaviour, iDamage
{
    [Header("Basics")]
    [SerializeField] Renderer Model;
    [SerializeField] int HP;
    Color colorOrig;
    public NavMeshAgent Agent;
    public Transform playerChara;
    public LayerMask Ground, WherePlayer;

    [Header("Bullet")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform Shotpostion;
    [SerializeField] float shootrate;
    [SerializeField] float shootForce;
    [SerializeField] float shootUpForce;
    bool Isshooting;

    [Header("Patrol")]
    public Vector3 WalkingPoint;
    bool IsWalk;
    public float timebetween;
    [SerializeField] float Walkpointrange;

    [Header("Range")]
    [SerializeField] float SightRange;
    public float Xrange;
    public float Yrange;
    public float Zrange;
    bool IsinSight;

    void Awake()
    {
        colorOrig = Model.material.color;
        playerChara = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        IsinSight = Physics.CheckSphere(transform.position, SightRange, WherePlayer);
        Isshooting = Physics.CheckSphere(transform.position, shootrate, WherePlayer);    
        if (!IsinSight && !Isshooting)
        {
            Patroling();

        }
        if (IsinSight && !Isshooting)
        {
            CHASE(); 
        }
        if (IsinSight && Isshooting)
        {         
            Shooting();
        }
    }
    public void Patroling()
    {
        if (!IsWalk)
        {
            SearchWalkpath();
        }
        if (IsWalk)
        {
            Agent.SetDestination(WalkingPoint);
        }

        Vector3 DistanceWalking = transform.position - WalkingPoint;

        if (DistanceWalking.magnitude < 1f)
        {
            IsWalk = false;
        }
    }
    private void SearchWalkpath()
    {
        float RandomZ = Random.Range(-Walkpointrange, Walkpointrange);
        float RandomX = Random.Range(-Walkpointrange, Walkpointrange);
        WalkingPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(WalkingPoint, -transform.up, 2f, Ground))
        {
            IsWalk = true;
        }

    }
    public void CHASE()
    {
        Agent.SetDestination(playerChara.position);
        Teleport();
    }
    private void Shooting()
    {
        //enemy does not move
        Agent.SetDestination(transform.position);
        transform.LookAt(playerChara);

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
    void Teleport()
    {
        float X = Random.Range(-Xrange, Xrange);
        float Y = Random.Range(0, Yrange);
        float Z = Random.Range(-Zrange, Zrange);
        transform.position = new Vector3(X, Y, Z);
        transform.LookAt(GameManager.Instance.Player.transform);
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
    private void ResetShooting()
    {
        Isshooting = false;
    }
}
