using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidLqt : MonoBehaviour,iDamage
{
    int health = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
