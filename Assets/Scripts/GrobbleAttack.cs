﻿using UnityEngine;
using System.Collections;

public class GrobbleAttack : MonoBehaviour
{
    private float timeBetweenAttacks = 2.5f / 3f;
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

    AudioClip hit;

    float volume;

    void Start()
    {
        totalDamage = 0;

        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        enemyStats = GetComponent<EnemyStats>();
        enemyResources = GetComponent<EnemyResources>();
        attackDamage = enemyStats.attack * damageMultiplier;

        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        hit = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().enemyHit;
    }

    void Attack()
    {
        GameObject barricade = enemyResources.targetBarricade;
		GameObject goal = GameObject.Find("Goal");
		if(goal!=null && (new Vector3(goal.transform.position.x,transform.position.y,goal.transform.position.z)-transform.position).magnitude<5f){
			goal.GetComponent<GoalScript>().removeLife(attackDamage);
			transform.LookAt(new Vector3(goal.transform.position.x,transform.position.y,goal.transform.position.z));
		}

		else if (barricade != null && (barricade.transform.position - transform.position).magnitude < 5f)
        {
            barricade.GetComponent<barricade>().TakeDamage(attackDamage);
			transform.LookAt(new Vector3(barricade.transform.position.x,transform.position.y,barricade.transform.position.z));
        }
        else if (playerHealth.currentHealth > 0 && (player.transform.position - transform.position).magnitude < 3f)
        {
            playerHealth.TakeDamage(attackDamage);
			transform.LookAt(new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z));
        }
        enemyResources.totalDamage += attackDamage;

        audio.PlayOneShot(hit, volume);

    }

    void Update()
    {


        if (enemyResources.attacking)
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
