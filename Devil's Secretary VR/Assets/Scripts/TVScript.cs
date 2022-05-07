using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TVScript : MonoBehaviour
{
    [Header("Canvas Attributes")]
    public GameObject baseScreen;
    public GameObject soulScreen;
    public GameObject timerScreen;
    public GameObject instructionsScreen;
    public TextMeshProUGUI timerCount;
    public Animator anim;
    public bool timerRunning = false;
    public float timeRemaining = 120;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public int points = 0;
    public bool isGrabbed = false;
    public Transform topDial;
    public Transform propellers;
    public float rotateSpeed;
    public bool gameOver = false;
    public Text scoreCount;
    public bool isVictory, isDefeat;

    public void Awake()
    {
        baseScreen.SetActive(true);
        soulScreen.SetActive(false);
        timerScreen.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        
    }
    public void StartGame()
    {
        baseScreen.SetActive(false);
        soulScreen.SetActive(true);
        timerScreen.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        anim.Play("TDR1");
    }
    public void Instructions()
    {
        baseScreen.SetActive(false);
        soulScreen.SetActive(false);
        instructionsScreen.SetActive(true);
        timerScreen.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        anim.Play("TDR2");
    }
    public void SoulGame()
    {
        baseScreen.SetActive(false);
        soulScreen.SetActive(false);
        timerScreen.SetActive(true);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        anim.Play("TDR3");
        timerRunning = true;
    }

    public void Update()
    {
        propellers.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
       
        if (timerRunning)
        {
            //Debug.Log(timeRemaining);
            DisplayTime(timeRemaining);
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;

                timerRunning = false;
                gameOver = true;
                isDefeat = true;
                isVictory = false;
                GameOver();
                
            }
            if (points >= 10)
            {
                timerRunning = false;
                gameOver = true;
                isDefeat = false;
                isVictory = true;
                Victory();
                
            }
        }
    }
    public void GameOver()
    {
        Debug.Log("GameOver, you lost");
        anim.Play("TDR4");
        defeatPanel.SetActive(true);

    }
    public void Victory()
    {
        Debug.Log("You won!");
        anim.Play("TDR4");
        victoryPanel.SetActive(true);
    }
    public void ScoreIncrease(int increase)
    {
        points += increase;
        scoreCount.text = "Souls Captured: " + points;
        Debug.Log("You got a point!");

    }
    /// <summary>
    /// Function to convert the time remaining into minutes and seconds
    /// </summary>
    /// <param name="timeToDisplay"></param>
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerCount.text =string.Format("{0:00}:{1:00}", minutes, seconds);


    }
}
