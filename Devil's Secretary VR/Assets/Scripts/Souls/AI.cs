using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class AI : MonoBehaviour
{
    [Header("Skull Controls")]
    [Tooltip("Variables to find certain values of the player position")]
    public LayerMask thePlayer;
    public Transform player;

    [Tooltip("Variables used to determine the skull and its animations")]
    public NavMeshAgent agent;
    public Animator anim;
    private Rigidbody rb;
    //Prefab used to spawn the dead object after the skull has been slain
    public GameObject deadSkull;

    [Tooltip("Booleans used to determine the location of the player, and if the skull can change states in order to pursue or attack.")]
    public float sightRange = 10, attackRange = 3;
    public Collider[] playerInSight, playerInAttack; 
    public bool canWalk, playerInSRange = false, playerInARange = false;

    [Tooltip("Variables used to determine how often the skull can attack and how much damage is done")]
    //Timer variable to be set to determine how quickly the skull can attack with its fireball
    private float timeBetweenAttacks = 1f;
    bool hasAttacked;

    [Tooltip("Variables used to determine the attack power and health of the skull")]
    public Image healthBar;
    public float Health;
    private float maxHealth = 100f;
    private float weaponDamage = 1f;
    public GameObject projectile;

    [Tooltip("Variables used to determine the speed and height of the Skull")]
    public float height;
    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;

    [Tooltip("Variables to determine the distance of the Wander State")]
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    private TVScript tv;
    private PlayerScript playerScript;
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        timer = wanderTimer;
        tv = GameObject.Find("Tv").GetComponent<TVScript>();
        playerScript = player.GetComponent<PlayerScript>();
    }
    void Start()
    {
       
        if (agent != null)
        {
            agent.speed = speed;
        }
        anim = this.GetComponent<Animator>();
        Health = maxHealth;
        healthBar.fillAmount = Health / maxHealth;

    }
    void Update()
    {
        CheckCircles();
        if (!playerInSRange && !playerInARange)
        {
            Debug.Log("is Idling");
            canWalk = true;
            timer += Time.deltaTime;
            if (timer >= wanderTimer)
            {
                //Finds a new point on the nav mesh, sets the location, then restarts the timer to find another point
                agent.speed = .5f;
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
            
        
        if (playerInSRange && !playerInARange)
        {
            Debug.Log("Is running");
            canWalk = true;
            timer += Time.deltaTime;
            if (timer >= wanderTimer)
            {
                //Finds a new point on the nav mesh, sets the location, then restarts the timer to find another point
                agent.speed = 2f;
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;

                playerInSight = null;
                playerInSRange = false;
            }
        }
        if(playerInSRange && playerInARange)
        {
            Debug.Log("Is Attacking");
            canWalk = false;
            agent.speed = 0f;
            Attack();
        }
        if(tv.points >= 10)
        {
            Destroy(this.gameObject);
        }
        if (tv.gameOver)
        {
            Destroy(this.gameObject);

        }
        if(playerScript.playerDied)
        {
            Destroy(this.gameObject);
        }

        //Change the y location of the skull to above the ground by this much.
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        healthBar.fillAmount = Health / maxHealth;

    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    
    public void TakeDamage(float amnt)
    {
        Health -= amnt;
        healthBar.fillAmount = Health / maxHealth;
        if (Health <= 0)
        {
            Vector3 location = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Quaternion rotate = this.transform.rotation;
            Destroy(gameObject);
            Instantiate(deadSkull, location, rotate);
        }
    }
    public void Attack()
    {
        if (!canWalk)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            if (!hasAttacked)
            {
               Rigidbody rigb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

                rigb.AddForce(transform.forward * 10f, ForceMode.Impulse);
                rigb.AddForce(transform.up * 5f, ForceMode.Impulse);

                hasAttacked = true;
                Invoke(nameof(ResetAttacks), timeBetweenAttacks);
            }
        }
    }
    public void ResetAttacks()
    {
        Debug.Log("ResetAttack");
        hasAttacked = true;
        canWalk = true;
        playerInSight = null;
        playerInAttack = null;
        playerInSRange = false;
        playerInARange = false;
        timer = 0;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("lightning"))
        {
            TakeDamage(weaponDamage);

        }

    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    public void CheckCircles()
    {
        playerInSight = Physics.OverlapSphere(transform.position, sightRange, thePlayer);

        if (playerInSight.Length > 0)
        {
            playerInSRange = true;
        }

        playerInAttack = Physics.OverlapSphere(transform.position, attackRange, thePlayer);

        if(playerInAttack.Length > 0)
        {
            playerInARange = true;
        }
    }


}
