using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AttractedObject : MonoBehaviour
{

    [SerializeField] Transform objectOfAttraction;
    [SerializeField] AttractionType attractionType;
    [SerializeField] float attractionStrength;
    [SerializeField] bool useSqrtOfDistance;

    Transform myTransform;
    Rigidbody myRigidbody;

    void Awake()
    {
        // cache these
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        // get the positions of this object and the target
        Vector3 targetPosition = objectOfAttraction.position;
        Vector3 myPosition = myTransform.position;

        // work out direction and distance
        Vector3 direction = (targetPosition - myPosition).normalized;
        float distance = Vector3.Magnitude(targetPosition - myPosition);       // you could move this inside the switch to avoid processing it for the Constant case where it's not used

        // apply square root to distance if specified to do so in the inspector
        if (useSqrtOfDistance) distance = Mathf.Sqrt(distance);

        Vector3 resultingForceAmount = Vector3.zero;
        // depending on which type of attraction, work out the appropriate
        // amount and direction of force to apply to cause movement
        switch (attractionType)
        {
            case AttractionType.Constant:
                resultingForceAmount = attractionStrength * direction;
                break;
            case AttractionType.DecreaseWithDistance:
                resultingForceAmount = attractionStrength * direction / distance;
                break;
            case AttractionType.IncreaseWithDistance:
                resultingForceAmount = attractionStrength * direction * distance;
                break;
        }

        // then finally add the force to the rigidbody
        myRigidbody.AddForce(resultingForceAmount);

    }

    enum AttractionType
    {
        DecreaseWithDistance, // (like gravity)
        Constant,             // constant force magnitude
        IncreaseWithDistance  // (opposite of gravity)
    }

}