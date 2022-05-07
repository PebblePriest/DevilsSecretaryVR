using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeadSoulScript : MonoBehaviour
{
    public Text number;
    public float skullNumber;
    public float timer = 25f;
    public GameObject enemyPrefab;
    private TVScript tv;

    private PlayerScript playerScript;
    public void Start()
    {
        skullNumber = Random.Range(1, 32);
        number.text = skullNumber.ToString();
        tv = GameObject.Find("Tv").GetComponent<TVScript>();

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    public void Update()
    {
        if(tv.points <= 10)
        {
            if (timer <= 0)
            {
                SpawnEnemy();
                timer = 25f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        if(tv.points >= 10)
        {
            Debug.Log("Player has won!");
            Destroy(this.gameObject);
        }
        if(tv.gameOver)
        {
            Debug.Log("Player has lost!");
            Destroy(this.gameObject);
        }
       if(playerScript.playerDied)
        {
            Destroy(this.gameObject);
        }

    }
    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
