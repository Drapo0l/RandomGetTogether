using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header("----Pickup Setting----")]
    [SerializeField] Transform holdArea;
    private GameObject holdObj;
    private Rigidbody holdObjRB;

    [Header("----Physics Parameter----")]
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupForce;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (holdObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    //GrabObject
                    PickUPObject(hit.transform.gameObject);
                }
                else
                {
                    //DropObject
                    DropObject();
                }
            }
        }
        if (holdObjRB != null)
        {
            //MoveObject
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(holdObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirc = (holdArea.position - holdObj.transform.position);
            holdObjRB.AddForce(moveDirc * pickupForce);
        }
    }

    void PickUPObject(GameObject grabObj)
    {
        if (grabObj.GetComponent<Rigidbody>() && CompareTag("GrabObject"))
        {
            holdObjRB = grabObj.GetComponent<Rigidbody>();
            holdObjRB.drag = 10;
            holdObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            holdObjRB.transform.parent = holdArea;
            holdObj = grabObj;
        }
    }
    

    void DropObject()
    {
        holdObjRB.drag = 1;
        holdObjRB.constraints = RigidbodyConstraints.None;

        holdObj.transform.parent = null;
        holdObj = null;
        
    }
}
