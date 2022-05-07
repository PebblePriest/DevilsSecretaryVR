using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWorkingSkull : MonoBehaviour
{
    public GameObject workingSoul;
    public GameObject arrow;
    public GameObject cylinder;
    private TVScript tv;

    private PlayerScript playerScript;
    private void Start()
    {
        tv = GameObject.Find("Tv").GetComponent<TVScript>();

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    public void Update()
    {
        if (tv.gameOver)
        {
            if (tv.isVictory)
            {
                arrow.SetActive(false);
                workingSoul.SetActive(true);
                cylinder.SetActive(false);
            }
            if (tv.isDefeat)
            {
                arrow.SetActive(false);
                workingSoul.SetActive(false);
                cylinder.SetActive(false);
            }
            if (playerScript.playerDied)
            {
                this.arrow.SetActive(false);
                this.workingSoul.SetActive(false);
                this.cylinder.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Soul"))
        {
            Destroy(other.gameObject);
            this.arrow.SetActive(false);
            this.workingSoul.SetActive(true);
            this.cylinder.SetActive(false);
            tv.ScoreIncrease(1);
            Debug.Log("Score Increased by 1");
        }
    }

}
