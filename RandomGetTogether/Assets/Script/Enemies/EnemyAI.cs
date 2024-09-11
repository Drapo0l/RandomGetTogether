using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;

   [SerializeField] int HP;

    //Color colorOrig;
    // Start is called before the first frame update
    void Start()
    {
        //colorOrig = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //IEnumerator flashColor()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(1f);
    //    model.material.color = colorOrig;
    //}
    //public void takeDamge(int amount) 
    //{  
    //    HP -= amount; 
    //    flashColor();
    //    if (HP <= 0)
    //    {
    //        Destroy(gameObject);
    //    }

    //}
}
