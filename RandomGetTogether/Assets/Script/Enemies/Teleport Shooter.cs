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
    [SerializeField] Transform headPos;
    Color colorOrig;
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Transform Anchor;
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
    [SerializeField] float Shootrange;

    public float Xrange;
    public float Yrange;
    public float Zrange;
    bool IsinSight;
    bool isinRange;

    //roam
    bool Isroaming;
    [SerializeField] int RoamTimer;
    [SerializeField] int RoamDist;
  //  [SerializeField] int Viewangle;
    [SerializeField] int faceTargetSpeed;
    bool PlayerRange; 
    Vector3 StartPos;
    Vector3 PlayerDir;
    float AngleToPlayer;
    float stoppingDistOrig;

    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip roboDeath;
    [SerializeField] float AudrobotDeathVol;
    [SerializeField] AudioClip RobotHit;
    [SerializeField] float AudrobotHitVol;
    [SerializeField] AudioClip RobotLaser; 
    [SerializeField] float AudRobotLaser;
    [SerializeField] AudioClip[] Footsteps;
    [SerializeField] float AudFootSteps;

    bool isPlayingStop;
    void Start()
    {
        isPlayingStop = false;
        colorOrig = Model.material.color;
        Anchor = GameObject.Find("SpawnPoint").transform;
        //Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        IsinSight = Physics.CheckSphere(transform.position, SightRange, WherePlayer); // checks how far it can see the player and what you want to put im for it
        isinRange = Physics.CheckSphere(transform.position, Shootrange, WherePlayer); //Checks how far it can shoot the player and what you want to put im for it
        if (!IsinSight) // if not in sight, it patrols around its area
        {
            Patroling();
        }
        if (IsinSight && !isinRange) // If it sees you,it will chase you but not attack you until your in range
        {
            CHASE();
        }
        if (IsinSight && isinRange) // if in sight and range to attack, it would start shooting you
        {
            
            Shooting();
        }
        //if(PlayerRange && !CanseePlayer())
        //{
        //    //if (!Isroaming && Agent.remainingDistance < 0.05f)
        //    //{
        //    //    StartCoroutine(roam());
        //    //}
        //}
    }
    public void Patroling()
    {
        if (!IsWalk)   // if its not walking, it will begin to walk in its range
        {
            SearchWalkpath();
        }
        if (IsWalk) // if it is walking, it would search what its walk range is and move around in that range
        {
            if (!isPlayingStop) playSteps();
            Agent.SetDestination(WalkingPoint);
        }

        Vector3 DistanceWalking = transform.position - WalkingPoint; // calucating its walking distance 

        if (DistanceWalking.magnitude < 1f) // if its lower than one, you reached the walkpoint and stopped walking and will search for a new one
        {
            IsWalk = false;
        }
    }
    private void SearchWalkpath()
    {
        float RandomZ = Random.Range(-Walkpointrange, Walkpointrange);  // randomizing the range where it would walk on Z plane
        float RandomX = Random.Range(-Walkpointrange, Walkpointrange); // randomizing the range where it would walk on x plane
        WalkingPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ); // adds the random range to the enemy amd keep Y the same

        if (Physics.Raycast(WalkingPoint, -transform.up, 2f, Ground)) // to check if its on the groud of the map and will walk if it is
        {
            IsWalk = true;
        }

    }
    public void CHASE() // Chases the player
    {
        Agent.SetDestination(GameManager.Instance.Player.transform.position);   

    }
    private void Shooting()
    {
        // Check for a clear line of sight before shooting
        RaycastHit hit;
        Vector3 directionToPlayer = GameManager.Instance.Player.transform.position - transform.position;
        // Perform the raycast to check for obstacles between enemy and player
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, Shootrange))
        {
            Transform Parent = hit.transform.parent;
      
            // Check if the raycast hit the player
            if (hit.transform.CompareTag("Player") || Parent != null)
            {
                if (!hit.transform.CompareTag("Player") && Parent != null)
                {
                    if (Parent.CompareTag("Player"))
                    {
                        InstantiateBullet(directionToPlayer);
                    }
                }
                else
                {
                    InstantiateBullet(directionToPlayer);
                }
            }
        }
    }

    private void InstantiateBullet(Vector3 directionToPlayer)
    {

        // Make the enemy face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation

        Agent.SetDestination(GameManager.Instance.Player.transform.position);
        if (!Isshooting)
        {
            Aud.PlayOneShot(RobotLaser, AudRobotLaser);
            // Calculate the direction towards the player
            Vector3 shootDirection = (GameManager.Instance.Player.transform.position - Shotpostion.position).normalized;

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

    void Teleport()
    {
        float X = Random.Range(-Xrange, Xrange); // teleports it through the random X range you gave it
        float Y = Random.Range(0, Yrange); // alwways keep this 0 unless you want it to go up, becasuse it would go down if not 0
        float Z = Random.Range(-Zrange, Zrange); // teleports it through the random X range you gave it
        transform.position = new Vector3(X, Y, Z); //Sets it as the new postiton of the enemy 
        transform.LookAt(GameManager.Instance.TeleportAnchor.transform); // Teleports around the Anchor and won't teleport out of its range
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
        Teleport();
        Aud.PlayOneShot(RobotHit, AudrobotHitVol);
        if (HP <= 0)
        {
            Aud.PlayOneShot(roboDeath, AudrobotDeathVol);

            int GoldDropped = Random.Range(1, 20);
            GameManager.Instance.PlayerScript.Gold += GoldDropped;
            Destroy(gameObject);
        }

    }

    IEnumerator roam()
    {
        Isroaming = true;
        yield return new WaitForSeconds(RoamTimer);

        Agent.stoppingDistance = 0;
        Vector3 randomPos = Random.insideUnitSphere * RoamDist;
        randomPos += StartPos;
        NavMeshHit Hit;
        NavMesh.SamplePosition(randomPos, out Hit, RoamDist, 1);
        Agent.SetDestination(Hit.position);
        Isroaming = false;
    }
    private void ResetShooting()
    {
        Isshooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRange = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRange = false;
        }
    }

    bool CanseePlayer()
    {
        PlayerDir = GameManager.Instance.Player.transform.position - headPos.position;
        AngleToPlayer = Vector3.Angle(PlayerDir, transform.forward);
        Debug.DrawRay(headPos.position, PlayerDir);
        RaycastHit hit; 
        if(Physics.Raycast(headPos.position,PlayerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && AngleToPlayer <= SightRange) 
            {
                Agent.SetDestination(GameManager.Instance.Player.transform.position);
                if(Agent.remainingDistance <= Agent.stoppingDistance)
                {
                    FaceTarget();
                }

                if (!Isshooting)
                {
                    Shooting();
                }
            }
            Agent.stoppingDistance = stoppingDistOrig;
            return true;
        }
        Agent.stoppingDistance = 0; 
        return false;
    }

    IEnumerator playSteps()
    {
        isPlayingStop = true;

        //play walk sound
        Aud.PlayOneShot(Footsteps[Random.Range(0, Footsteps.Length)], AudFootSteps);
        yield return new WaitForSeconds(.8f);
        isPlayingStop = false;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(PlayerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation,rot,Time.deltaTime * faceTargetSpeed);
    }
}

