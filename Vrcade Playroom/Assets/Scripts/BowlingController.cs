using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

//Bowling game controller - shows score
public class BowlingController : MonoBehaviour
{
    int puntuacionConseguida = 0;
    [SerializeField] List<GameObject> bolos;
    [SerializeField] TextMeshPro score;

    public void resetGame()
    {
        //Set text with score
        //Delayed call with reset score
        score.text = puntuacionConseguida.ToString();
        ResetDelayed(3);
        puntuacionConseguida = 0;


        foreach (var item in bolos)
        {
            item.GetComponent<BowlingPinController>().resetPosition();
        }
    }

    public void addScore()
    {
        puntuacionConseguida++;
    }

    void ResetDelayed(float delayTime)
    {
        StartCoroutine(Reset(delayTime));
    }

    IEnumerator Reset(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        score.text = "";
    }
}
