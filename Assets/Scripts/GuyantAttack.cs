using UnityEngine;
using System.Collections;

public class GuyantAttack : MonoBehaviour
{
    private float timeBetweenAttacks = 2.5f/3f;
    public int attackDamage;
    public float playerDistance;
    public int damageMultiplier = 20;

    public int totalDamage;

    public GameObject player;
    PlayerHealth playerHealth;
    EnemyResources enemyResources;
    EnemyStats enemyStats;
    bool playerInRange;
    public bool invoked = false;

    void Start()
    {
        totalDamage = 0;

        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        enemyStats = GetComponent<EnemyStats>();
        enemyResources = GetComponent<EnemyResources>();
        attackDamage = enemyStats.attack * damageMultiplier;

    }

    void Attack()
    {
        playerHealth.TakeDamage(attackDamage);
        totalDamage += attackDamage;
    }

    void Update()
    {
        if (enemyResources.attacking && playerHealth.currentHealth > 0 )
        {
            if (!invoked)
            {
                InvokeRepeating("Attack", timeBetweenAttacks / 1.8f, timeBetweenAttacks);
                invoked = true;
            }

        }
        else
        {
            CancelInvoke("Attack");
            invoked = false;
            
        }
    }
}
