using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bowling pins reset and respawn
public class BowlingPinController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPosition;
    Quaternion initialRotation;
    Rigidbody rb;
    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void resetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
