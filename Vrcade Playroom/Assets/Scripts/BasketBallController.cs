using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basketball ball controller - manages its position and respawn
public class BasketBallController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPosition;
    Rigidbody rb;
    [SerializeField] BasketController machine;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("barrera"))
        {
            if (machine.getGamePlaying())
            {
                resetPosition();
            }
        }
    }
}
