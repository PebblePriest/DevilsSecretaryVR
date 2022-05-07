using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    public int health;
    private int maxHealth = 100;
    public GameObject endScreen;
    public GameObject victoryScreen;
    public Slider healthBar;
    public GameObject healthBarObject;
    public bool playerDied;
    private TVScript tv;
    private float timer;
    public GameObject damageScreen;
    public void Start()
    {
        health = maxHealth;
        tv = GameObject.Find("Tv").GetComponent<TVScript>();
    }
    public void Update()
    {
        healthBar.value = health;
        if (playerDied)
        {
            timer += Time.deltaTime;
            healthBarObject.SetActive(false);
            endScreen.SetActive(true);
            if(timer >= 5)
            {
                endScreen.SetActive(false);
            }
        }
       if(tv.timeRemaining <= 0)
        {
            timer += Time.deltaTime;
            healthBarObject.SetActive(false);
            endScreen.SetActive(true);
            if (timer >= 5)
            {
                endScreen.SetActive(false);
            }
        }
        if(tv.points >= 10)
        {
            timer += Time.deltaTime;
            healthBarObject.SetActive(false);
            victoryScreen.SetActive(true);
            if (timer >= 5)
            {
                victoryScreen.SetActive(false);
            }
        }
        if(damageScreen != null)
        {
            if(damageScreen.GetComponent<Image>().color.a > 0)
            {
                var color = damageScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                damageScreen.GetComponent<Image>().color = color;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;
        gotHit();
        if (health <= 0)
            playerDied = true;

    }
    public void gotHit()
    {
        var color = damageScreen.GetComponent<Image>().color;
        color.a = 0.8f;

        damageScreen.GetComponent<Image>().color = color;


    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);

    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
   
}
