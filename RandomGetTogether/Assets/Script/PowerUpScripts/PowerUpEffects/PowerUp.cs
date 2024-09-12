using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffects poweupEffect;
    private void OnTriggerEnter(Collider collision)
    {
        //can check for player or enemy (I am lazy rn)
        //remember only for player
        poweupEffect.Apply(collision.gameObject);
        Destroy(gameObject);
        
    }
}
