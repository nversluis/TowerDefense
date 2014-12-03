using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 1;
    public int attackThreshold = 5;
    public float playerDistance;

    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = player.GetComponent<EnemyHealth>();
    }

    void SetPlayerInRange(float playerDistance)
    {
        if (playerDistance < attackThreshold)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }*/

    void Attack()
    {
        if (playerHealth.currentHealth > 0 && playerInRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;

        playerDistance = Vector3.Distance(player.transform.position, this.transform.position);

        SetPlayerInRange(playerDistance);
        Debug.Log("Is player in range? " + playerInRange);


        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack();
        }
	}
}
