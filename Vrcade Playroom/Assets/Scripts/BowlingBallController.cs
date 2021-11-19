using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bowling ball controller - checks collisions, audio and reset
public class BowlingBallController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPosition;
    Rigidbody rb;
    [SerializeField] BowlingController left;
    [SerializeField] BowlingController right;
    AudioSource audio;
    [SerializeField] AudioClip strike;
    [SerializeField] AudioClip slide;

    void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    public void resetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BowlRail"))
        {
            audio.Stop();
            resetPosition();
            left.resetGame();
        }
        else if (other.gameObject.tag.Equals("BowlRail2"))
        {
            audio.Stop();
            resetPosition();
            right.resetGame();
        }
        else if (other.gameObject.tag.Equals("EndBowl"))
        {
            audio.Stop();
            audio.PlayOneShot(strike);
            ResetDelayed(left, 3);

        }
        else if (other.gameObject.tag.Equals("EndBowl2"))
        {

            audio.Stop();
            audio.PlayOneShot(strike);
            ResetDelayed(right, 3);

        }
        else if (other.gameObject.tag.Equals("bowlline"))
        {
            audio.PlayOneShot(slide);
        }
    }

    void ResetDelayed(BowlingController b, float delayTime)
    {
        StartCoroutine(Reset(b, delayTime));
    }

    IEnumerator Reset(BowlingController b, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        resetPosition();
        b.resetGame();
    }
}
