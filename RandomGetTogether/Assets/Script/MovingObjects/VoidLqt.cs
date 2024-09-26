using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidLqt : MonoBehaviour,iDamage
{
    int health = 10000;
    [SerializeField] int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        iDamage damage = hit.transform.GetComponent<iDamage>();
        damage.takeDamage(attackDamage);
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
