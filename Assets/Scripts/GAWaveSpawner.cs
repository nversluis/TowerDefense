using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAWaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies = 5;
    public bool spawning = true;

    public int maxWaves = 10;
    public int currentWave = 1;

    public float maxX;
    public float maxZ;
    float orcHeigthSpawn = 3.27f;

    public int currentTotalStatPoints = 100;
    public int delta = 50;

    public ArrayList currentGen;
    public ArrayList nextGen;
    EnemyStats enemyStats;


    // Use this for initialization
    void Start()
    {
        currentGen = new ArrayList();
        nextGen = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (spawning)
            {
                if (currentGen.Count < maxEnemies)
                {
                    // Spawn enemies tot het maximale aantal enemies wordt bereikt
                    SpawnEnemy();
                }
                else
                {
                    spawning = false;
                }
            }
            else
            {
                if (AllEnemiesDead())
                {
                    // Als alle enemies dood zijn, ga naar de volgende wave
                    currentWave++;
                    // Enemies mogen weer gespawnd worden
                    spawning = true;
                }
            }
        }
        else
        {
            Debug.Log("Congratulations! You've succesfully defeated all waves of enemies!");
        }
        
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, orcHeigthSpawn, randZ), Quaternion.identity);

        Enemy.name = "enemy";
        Enemy.transform.FindChild("Floor").transform.position = transform.position;
        enemyStats = enemy.GetComponent<EnemyStats>();
        // Genereer enemies met toenemende stats per wave
        enemyStats.totalStatPoints = currentTotalStatPoints;
        currentGen.Add(Enemy);
    }


    void UpdateEnemyCount()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            // Debug.Log(currentGen.Count);
            //Debug.Log(((GameObject)currentGen[i]).GetComponent<EnemyHealth>().currentHealth);
            if (((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead)
            {
                // Voeg de enemy toe aan de volgende generatie
                nextGen.Add(currentGen[i]);
                // Verwijder de enemy uit de huidige genertatie als die dood is
                currentGen.Remove(currentGen[i]);
            }
        }
    }

    void SpawnNextGen()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);


        if (nextGen.Count == maxEnemies)
        {
            for (int i = 0; i < nextGen.Count; i++)
            {
                Instantiate((GameObject)nextGen[i], transform.position + new Vector3(randX, 0f, randZ), Quaternion.identity);
            }
        }
    }

    void Respawn()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().currentHealth = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().startingHealth;
        }
    }

    bool AllEnemiesDead()
    {
        int aantal = 0;

        for (int i = 0; i < currentGen.Count; i++)
        {
            if (((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead) {
                aantal++;
            }
        }
        return (aantal == maxEnemies);
    }
}