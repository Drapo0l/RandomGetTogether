using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lazer : MonoBehaviour, iDamage
{
    [SerializeField] float health;
    [SerializeField] int attackDamage;
    [SerializeField] int lazerDistance;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform startPoint;
    [SerializeField] ParticleSystem hitEffect;

    [SerializeField] bool SeeRayCast;



    private void Update()
    {
        RaycastHit rayHit;
        lineRenderer.SetPosition(0, startPoint.position);


        if (Physics.Raycast(transform.position, transform.forward, out rayHit))
        {
            Instantiate(hitEffect, rayHit.point, Quaternion.identity);

            if (rayHit.collider)
            {
                lineRenderer.SetPosition(1, rayHit.point);

            }
            if (rayHit.transform.CompareTag("Player"))
            {
                
                iDamage damage = rayHit.transform.GetComponent<iDamage>();
                damage.takeDamage(attackDamage);
            }
            
        }
        else
        {
            lineRenderer.SetPosition(1, startPoint.position + transform.forward * lazerDistance);
        }
    }
    private void OnDrawGizmos()
    {
        if (SeeRayCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * lazerDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.23f);
        }
    }

    public void takeDamage(int amount)
    {

        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}


