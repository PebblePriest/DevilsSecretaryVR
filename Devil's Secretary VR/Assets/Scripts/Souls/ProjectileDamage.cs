using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public PlayerScript playerCharacter;
    private float timer;
    public void Awake()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Ranged HIT!");
            playerCharacter.TakeDamage(10);
            Destroy(this.gameObject);

        }
        if(collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 5)
        {
            Destroy(this.gameObject);
        }
    }
}
