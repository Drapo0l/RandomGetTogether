using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningObject : MonoBehaviour
{
    public Vector3 spinRotation;
    public Vector3 spinPosition;
    [SerializeField] float spiningSpeed;

    // Update is called once per frame
    void Update()
    {
        //spinPosition = this.transform.position;
        //this.transform.position = spinPosition ;
        this.transform.Rotate(new Vector3(spinRotation.x, spinRotation.y, spinRotation.z), spiningSpeed * Time.deltaTime);
    }
}
