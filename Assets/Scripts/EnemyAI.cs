using UnityEngine;
using System.Collections;

public class enemyAI : MonoBehaviour {

    Transform target;
    Transform treasure;
    public float moveSpeed = 20f;
    public float targetDistance;
    public float treasureDistance;
    public float chaseThreshold = 50f;
    public float decisionThreshold = 4.5f;
    private int randomInt;

    public int attackDamage = 10;

    NavMeshAgent agent;
    public float attackThreshold = 10f;
    public static bool isPlayerAlive = true;
    public static bool attackingPlayer = false;

    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = target.GetComponent<PlayerHealth>();
    }


	// Use this for initialization
	void Start () {

        Vector3 startPosition = new Vector3(50, 2.5f, 40);

        // Sets position of enemy to startPosition
        transform.position = startPosition;

        // Sets color of enemy to green
        renderer.material.color = new Color(0, 1, 0);

        // Sets randomInt to a random Integer between 0 (inclusive) and 9 (inclusive) 
        randomInt = Random.Range(0, 9);
	}

	// Update is called once per frame
    void Update()
    {
        if (isPlayerAlive)
        {
            targetDistance = Vector3.Distance(target.position, this.transform.position);
            treasureDistance = Vector3.Distance(treasure.position, this.transform.position);

            // Decides whether the enemy is an attacking variant or an exploring variant
            if (randomInt >= decisionThreshold)
            {
                // Sets the color of an attacking variant to yellow
                renderer.material.color = new Color(1.0f, 1.0f, 0);
                
                // Chase the target if its close enough to
                if (targetDistance < chaseThreshold)
                {
                    // Chase the target
                    agent.SetDestination(target.position);

                    // Sets the color of a chasing enemy to white
                    renderer.material.color = new Color(1, 1, 1);

                    if (targetDistance < attackThreshold)
                    {
                        // Sets color of enemy to red
                        renderer.material.color = new Color(1, 0.0f, 0.0f);
                        attackPlayer();
                    }
                    else
                    {
                        agent.SetDestination(target.position);
                    }
                }
                else
                {
                    agent.SetDestination(treasure.position);
                }
            }
            else
            {
                agent.SetDestination(treasure.position);
            }
        }

    }

    void chasePlayer()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void attackPlayer()
    {
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }   
    }

}
