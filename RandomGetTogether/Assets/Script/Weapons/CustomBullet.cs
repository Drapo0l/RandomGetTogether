
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class CustomBullet : MonoBehaviour
{
    //assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    public float bulletDamage;

    //lifetime
    public int maxCollision;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //when to explode:
        if(collisions > maxCollision) Explode();

        //Count down lifetime

        maxLifetime -= Time.deltaTime;

        if (maxLifetime <= 0) Explode();
    }
    private void Explode() 
    { 
        //instantiate explosion
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position,explosionRange, whatIsEnemies);
        for(int i = 0; i < enemies.Length; i++) 
        {
            enemies[i].GetComponent<iDamage>().takeDamage(explosionDamage);  
            
            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position, explosionRange);
        }
        // add a little delay to make sure things work
        Invoke("Delay", 0.05f);
    }

    private IEnumerator NormalShot(GameObject target)
    {
        if (target.tag == "Player")
        {        
            
            
            PlayerMovement player = target.GetComponent<PlayerMovement>();
            GameManager.Instance.dmgflash();
            player.health -= bulletDamage;
            if (player.health <= 0)
            {
                Destroy(target);
            }           
            
        }
        else if (target.tag == "Enemy")
        {
            target.GetComponent<EnemyHealth>().health -= bulletDamage;
            target.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            target.GetComponent<Renderer>().material.color = Color.white;

            if (target.GetComponent<EnemyHealth>().health <= 0)
            {
                Destroy(target);               
            }
        }
        Destroy(gameObject);
        // add a little delay to make sure things work
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //explode if bullet hit an enemy directly and explodeOnTouch is true
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
        else if (collision.collider.CompareTag("Enemy") && !explodeOnTouch) StartCoroutine(NormalShot(collision.gameObject));
        else if (collision.collider.CompareTag("Player") && explodeOnTouch) Explode();
        else if (collision.collider.tag == "Player" && !explodeOnTouch) StartCoroutine(NormalShot(collision.gameObject));
        

        //don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet"))return;

        //count up collisions
        collisions++;

       
    }

    private void Setup()
    {
        //new physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //assign material
        GetComponent<SphereCollider>().material = physics_mat;

        //set gravity
        rb.useGravity = useGravity;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

   
}
