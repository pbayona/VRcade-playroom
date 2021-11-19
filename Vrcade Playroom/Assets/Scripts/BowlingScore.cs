using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Check if bowling ball has reached bowling pins
public class BowlingScore : MonoBehaviour
{
    [SerializeField] BowlingController b;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EndBowl") || other.gameObject.tag.Equals("EndBowl2"))
        {
            b.addScore();
        }
    }
}
