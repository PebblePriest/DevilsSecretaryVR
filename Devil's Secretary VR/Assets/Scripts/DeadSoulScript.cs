using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeadSoulScript : MonoBehaviour
{
    public Text number;
    public float skullNumber;
    public float timer = 60f;
    public GameObject enemyPrefab;
    public void Start()
    {
        skullNumber = Random.Range(1, 32);
        number.text = skullNumber.ToString();
       for(int i = 0; i < GameManager.Instance.takenNumbers.Count; i++)
        {
            if(GameManager.Instance.takenNumbers[i] == skullNumber)
            {

                GameManager.Instance.takenNumbers.Remove(GameManager.Instance.takenNumbers[i]);
            }
            i++;
        }
    }
    public void Update()
    {
        if(timer <= 0)
        {
            SpawnEnemy();
            timer = 60;
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }
    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
