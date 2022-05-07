using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    
    public Text soulsCaptured;
    public GameObject[] spawns;
    public GameObject enemyPrefab;
    private TVScript tv;
    private bool spawnedEnemies;
    public static GameManager Instance { get; private set;}

    public void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        Time.timeScale = 1;
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        tv = GameObject.Find("Tv").GetComponent<TVScript>();

    }
    
    public void Update()
    {
        if(tv.timerRunning)
        {
            if (!spawnedEnemies)
            {
                foreach (GameObject e in spawns)
                {

                    Instantiate(enemyPrefab, e.transform.position, transform.rotation);
                }
                spawnedEnemies = true;
            }
            else if(spawnedEnemies)
            {
                Debug.Log("All enemies spawned");
            }
            
            
        }
    }
 
 
   
   
}
