using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Airhockey controller - manages score text
public class AirHockeyController : MonoBehaviour
{

    int PointsP1;
    int PointsP2;
    bool gamePlaying = false;

    [SerializeField] TextMeshPro scoreText;
    [SerializeField] TextMeshPro scoreText2;

    [SerializeField] DiskController disk;

    void Start()
    {
        newGame();
    }


    void Update()
    {
        if (gamePlaying)
        {
            scoreText.text = PointsP1 + " - " + PointsP2;
            scoreText2.text = PointsP2 + " - " + PointsP1;
        }
    }

    public void newGame()
    {
        PointsP1 = 0;
        PointsP2 = 0;

        scoreText.text = "0 - 0";
        scoreText2.text = "0 - 0";
        disk.resetPosition();
        gamePlaying = true;
    }

    public void addPoints(int player)
    {
        if (player == 1)
        {
            if (PointsP1 < 7)
            {
                PointsP1++;
                if (PointsP1 == 7)
                {
                    gamePlaying = false;
                    scoreText.text = "P1 WON";
                    scoreText2.text = "P1 WON";
                }
            }
        }
        else
        {
            if (PointsP2 < 7)
            {
                PointsP2++;
                if (PointsP2 == 7)
                {
                    gamePlaying = false;
                    scoreText.text = "P2 WON";
                    scoreText2.text = "P2 WON";
                }
            }
            
        }
    }


}
