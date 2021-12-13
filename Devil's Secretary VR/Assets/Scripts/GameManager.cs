using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    
    public Text timerCount;
    private bool timerRunning = false;
    private float timeRemaining = 120;
    private int points = 0;
    public Text soulsCaptured;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject[] spawns;
    public GameObject enemyPrefab;
    public GameObject[] cubicles;
    private int cubicleNumber;
    public List<int> takenNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
    public static GameManager Instance { get; private set;}

    public void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        soulsCaptured.text = " Collect as may Souls as you can before the time runs out!\n Once you capture the soul, return them to their cubicle.\n The amount of souls returned will automatically be recorded on this page...\n Souls needed to complete challenge...10\n Souls Collected: " + points;
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        Time.timeScale = 1;
        spawns = GameObject.FindGameObjectsWithTag("Spawn");

    }
    public void Start()
    {
        foreach(GameObject c in cubicles)
        {
            int i = 1;
            Text cubicleText = c.GetComponent<Text>();
            cubicleNumber = 1;
            cubicleText.text = cubicleNumber.ToString();
            i++;

        }
        foreach (GameObject e in spawns)
        {
            
            Instantiate(enemyPrefab, e.transform.position , transform.rotation);
        }
    }
    public void Update()
    {
        if(timerRunning)
        {
            Debug.Log(timeRemaining);
            DisplayTime(timeRemaining);
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;

                timerRunning = false;

                GameOver();
            }
            if(points >= 10)
            {
                Victory();
            }
        }
    }
    /// <summary>
    /// When the player hits the location that the gamemanager is located, the game will officially start
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            timerRunning = true;
        }
    }
    /// <summary>
    /// Starts the countdown when called
    /// </summary>
   
    /// <summary>
    /// Function to convert the time remaining into minutes and seconds
    /// </summary>
    /// <param name="timeToDisplay"></param>
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerCount.text = "Time Remaining: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        

    }
    public void GameOver()
    {
        Debug.Log("GameOver, you lost");
        Time.timeScale = 0;
        defeatPanel.SetActive(true);

    }
    public void Victory()
    {
        Debug.Log("You won!");
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
    }
    public void ScoreIncrease(int increase)
    {
        points += increase;
        soulsCaptured.text = "Collect as may Souls as you can before the time runs out!Once you capture the soul, return them to their desk. The amount of souls returned will automatically be recorded on this page...\n\nSouls needed to complete challenge...10\n\nSouls Collected: " + points;




    }
   
}
