using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

//Arcade basket controller - manages game start, score and timer
public class BasketController : MonoBehaviour
{
    int score = 0;
    float time = 60f;
    [SerializeField] TextMeshPro timeText;
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] List<BasketBallController> balls;

    AudioSource audio;
    [SerializeField] AudioClip scored;

    float startTime;

    bool gamePlaying = false;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        newGame();
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
    }

    public void newGame()   //Create new game, on button pressed
    {
        score = 0;
        time = 60;
        scoreText.text = "00";
        timeText.text = "60";
        startTime = Time.time;
        foreach (var item in balls)
        {
            item.resetPosition();
        }
        gamePlaying = true;

    }

    void updateTime()   //Game timer
    {
        if (gamePlaying)
        {
            float currentTime = Time.time;
            float timeLeft = time - (currentTime - startTime);
            if (timeLeft < 10)
            {
                timeText.text = "0" + (int)timeLeft;
                if (timeLeft <= 0)
                {
                    gamePlaying = false;
                }
            }
            else {
                timeText.text = ((int)timeLeft).ToString();
            }
        }
        else
        {
            timeText.text = "00";
        }
    }

    public void addScore()  //Adds new point to score
    {
        score++;
        if (score < 10)
        {
            scoreText.text = "0" + score;
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }

    private void OnTriggerEnter(Collider other) //If balls enter collider, add score
    {
        if (other.gameObject.tag.Equals("basketball"))
        {
            audio.PlayOneShot(scored);
            addScore();
        }
    }

    public bool getGamePlaying()
    {
        return gamePlaying;
    }
}
