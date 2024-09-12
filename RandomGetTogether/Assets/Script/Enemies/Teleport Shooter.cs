using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TeleportShooter : MonoBehaviour, DamageFE
{
    [SerializeField] Renderer Model;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform Shotpostion;
    [SerializeField] int HP;
    [SerializeField] float shootrate;
    bool Isshooting;
    Color colorOrig;
    public NavMeshAgent Agent;
    public Transform playerChara;
    [SerializeField] Transform Anchor;  
    public LayerMask Ground, WherePlayer;
    //Patroling
    public Vector3 WalkingPoint;
    bool IsWalk;
    public float timebetween;
    [SerializeField] float Walkpointrange;

    //States
    [SerializeField] float SightRange;
    [SerializeField] float Shootrange;
    bool IsinSight; 
    // range
    public float Xrange;
    public float Yrange;
    public float Zrange;
    void Awake()
    {
        colorOrig = Model.material.color;
        playerChara = GameObject.Find("Player").transform;
        Anchor = GameObject.Find("SpawnPoint").transform;     
        Agent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        IsinSight = Physics.CheckSphere(transform.position, SightRange, WherePlayer);
        Isshooting = Physics.CheckSphere(transform.position, Shootrange, WherePlayer);    
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
         
            StartCoroutine(Shooting());
  

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

    }
    IEnumerator Shooting()
    {
        //enemy does not move
        Agent.SetDestination(transform.position);
        transform.LookAt(playerChara); 
        Isshooting = true;
        Instantiate(Bullet, Shotpostion.position, transform.rotation);
        yield return new WaitForSeconds(shootrate);
        Isshooting = false;

    } 
    void Teleport()
    {
        float X = Random.Range(-Xrange, Xrange);
        float Y = Random.Range(0, Yrange);
        float Z = Random.Range(-Zrange, Zrange);
        transform.position = new Vector3(X, Y, Z);
        transform.LookAt(GameManager.Instance.TeleportAnchor.transform); 
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
        Teleport();
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
