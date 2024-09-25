using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class FodderEnemy : MonoBehaviour, iDamage
{
    public int damage;
    [SerializeField] Renderer Model;
    [SerializeField] int HP;
    [SerializeField] NavMeshAgent agent;
    public LayerMask Ground, WherePlayer; 
    //Patroling
    public Vector3 WalkPoint;
    bool IsWalking;
    [SerializeField] float walkpointRange;  
    Color colorOrig;        
    //States
    [SerializeField] float Sightrange;  
     bool isinSight;

    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip roboDeath;
    [SerializeField] float AudrobotDeathVol;
    [SerializeField] AudioClip RobotHit;
    [SerializeField] float AudrobotHitVol;
    [SerializeField] AudioClip[] Footsteps;
    [SerializeField] float AudFootSteps;


    void Start()
    {
        colorOrig = Model.material.color; 
    }


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


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashColor());
        Aud.PlayOneShot(RobotHit, AudrobotHitVol);
        if (HP <= 0)
        {
            Aud.PlayOneShot(roboDeath, AudrobotDeathVol);
            int GoldDropped =  Random.Range(1, 20);
            GameManager.Instance.PlayerScript.Gold += GoldDropped; 
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
            Aud.PlayOneShot(Footsteps[Random.Range(0, Footsteps.Length)], AudFootSteps);
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
        agent.SetDestination(GameManager.Instance.Player.transform.position); 
    }

    IEnumerator flashColor()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        Model.material.color = colorOrig;
    }

}
