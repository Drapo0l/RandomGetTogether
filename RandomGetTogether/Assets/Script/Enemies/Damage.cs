using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] enum damageType { bullet, staionary, melee }
    [SerializeField] damageType DT;
    [SerializeField] Rigidbody rb;
    bool Isbump; 
    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destoryTime;
    void Start()
    {
        if (DT == damageType.bullet)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destoryTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageFE dmg = other.GetComponent<DamageFE>();

        if (dmg != null)
        { 
            dmg.takeDamge(damageAmount);  
        }
        if (DT == damageType.bullet)
        {
            Destroy(gameObject);
        }

        if(DT == damageType.melee)
        {
            Destroy(gameObject); 
        }

        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Isbump = true;

            Invoke("resetInvincibility", 2);
        }
         
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Isbump = true;

            Invoke("resetInvincibility", 2);
        }
    }

    void resetInvincibility()
    {
        Isbump = false;
    }
}
