using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public GameObject objectToRotate;
    Quaternion Protation;
   


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
    }

    void CheckRotation()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Protation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x + 90, objectToRotate.transform.eulerAngles.y, objectToRotate.transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Protation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x - 90, objectToRotate.transform.eulerAngles.y, objectToRotate.transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Protation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x, objectToRotate.transform.eulerAngles.y + 90, objectToRotate.transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Protation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x, objectToRotate.transform.eulerAngles.y + 90, objectToRotate.transform.eulerAngles.z);
        }

        objectToRotate.transform.rotation = Protation;
    }
}
