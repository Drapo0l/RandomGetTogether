using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDoorControler : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTriger = false;
    [SerializeField] private bool closeTriger = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (openTriger)
            {
                myDoor.Play("DoorOpenAnimation", 0, 0.0f);
               
            }
            else if (closeTriger)
            {
                myDoor.Play("DoorCloseAnimation", 0, 0.0f);
                
            }
        }
    }
}
