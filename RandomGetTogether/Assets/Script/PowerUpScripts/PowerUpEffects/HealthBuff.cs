using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthBuff : PowerUpEffects
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.transform.parent.GetComponent<PlayerMovement>().health += amount;
        if (target.transform.parent.GetComponent<PlayerMovement>().health > target.transform.parent.GetComponent<PlayerMovement>().maxHealth)
            target.transform.parent.GetComponent<PlayerMovement>().health = target.transform.parent.GetComponent<PlayerMovement>().maxHealth;
    }


}
