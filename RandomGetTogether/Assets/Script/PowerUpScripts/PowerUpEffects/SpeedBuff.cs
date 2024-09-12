using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PowerUpEffects
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.transform.parent.GetComponent<PlayerMovement>().walkSpeed += amount;
        target.transform.parent.GetComponent<PlayerMovement>().sprintSpeed += amount;
    }
}
