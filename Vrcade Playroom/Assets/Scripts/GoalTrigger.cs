using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Goal collider - Air hockey
public class GoalTrigger : MonoBehaviour
{
    [SerializeField] int playerNum;
    [SerializeField] AirHockeyController ahc;
    AudioSource audio;
    [SerializeField] AudioClip goal;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("disk"))
        {
            ahc.addPoints(playerNum);
            audio.PlayOneShot(goal);
            other.gameObject.GetComponent<DiskController>().resetPosition();
        }
    }
}
