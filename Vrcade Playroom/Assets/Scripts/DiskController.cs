using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Disk controller - Air hockey
public class DiskController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPosition;
    Rigidbody rb;
    void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    
    public void resetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
        Debug.Log("Reset");
    }
}
