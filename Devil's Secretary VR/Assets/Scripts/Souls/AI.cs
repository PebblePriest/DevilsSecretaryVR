using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;
    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;
    public Image healthBar;
    public float Health;
    private float maxHealth = 100f;
    public GameObject deadSkull;
    private float weaponDamage = 1f;

    public void Awake()
    {
       
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.SetDestination(RandomNavMeshLocation());
        }
        anim = this.GetComponent<Animator>();
        Health = maxHealth;
        healthBar.fillAmount = Health / maxHealth;

    }
    void Update()
    {
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(RandomNavMeshLocation());
        }
        healthBar.fillAmount = Health / maxHealth;

    }
    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    public void TakeDamage(float amnt)
    {
        Debug.Log(Health);
        Health -= amnt;
        healthBar.fillAmount = Health / maxHealth;
        if (Health <= 0)
        {
            Vector3 location = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Quaternion rotate = this.transform.rotation;
            GameManager.Instance.ScoreIncrease(1);
            Destroy(gameObject);
            Instantiate(deadSkull, location, rotate);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("lightning"))
        {
            TakeDamage(weaponDamage);

        }

    }



}
