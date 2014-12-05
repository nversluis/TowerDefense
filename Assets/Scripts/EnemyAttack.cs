﻿using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 5f;
    public int attackDamage = 1;
    private int attackThreshold = 10;
    public float playerDistance;

    public GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
	EnemyStats enemyStats;
    bool playerInRange;
    float timer;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player);
        //playerHealth = player.GetComponent <PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //enemyStats = GetComponent<EnemyStats>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
        Debug.Log(player);
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
	void Update () 
    {
        timer += Time.deltaTime;
        playerDistance = Vector3.Distance(player.transform.position, this.transform.position);
        SetPlayerInRange(playerDistance);
        Debug.Log(playerInRange);


        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack();
        }
	}
}
