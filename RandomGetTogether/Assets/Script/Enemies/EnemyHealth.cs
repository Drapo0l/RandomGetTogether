using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    [SerializeField] Renderer Model;
    public Color colorOrig;
    


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;

        colorOrig = Model.material.color;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator flashColor()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        Model.material.color = colorOrig;
    }

}
