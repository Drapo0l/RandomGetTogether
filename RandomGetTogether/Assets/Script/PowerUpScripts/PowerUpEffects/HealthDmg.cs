using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthDmg : PowerUpEffects
{
    public int damageAmount;
    public override void Apply(GameObject target)
    {
        target.transform.parent.GetComponent<PlayerMovement>().takeDamage(damageAmount);
       
    }


}
