using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{

    public float timeBetweenAttacks = 5f;
    public int attackDamage = 1;
    private int attackThreshold = 5;
    public float playerDistance;

    public GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    EnemyStats enemyStats;
    bool playerInRange;
    float timer;

    void Start()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyStats = GetComponent<EnemyStats>();
        attackDamage = enemyStats.attack;
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

    void Attack()
    {
        if (playerHealth.currentHealth > 0 && playerInRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        playerDistance = Vector3.Distance(player.transform.position, this.transform.position);
        SetPlayerInRange(playerDistance);

        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack();
        }
    }
}
