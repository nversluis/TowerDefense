using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAWaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject nextGenEnemy;
    public int maxEnemies = 5;
    public bool spawning = true;

    public int maxWaves = 10;
    public int currentWave = 1;

    public float maxX;
    public float maxZ;
    float orcHeigthSpawn = 3.27f;

    public int currentTotalStatPoints = 100;
    public int toenameTotalStatsPerWave = 50;

    public Vector3 spawnPosition;

    public ArrayList currentGen;
    public ArrayList nextGen;
    EnemyStats enemyStats;
    EnemyHealth enemyHealth;


    // Use this for initialization
    void Start()
    {
        currentGen = new ArrayList();
        nextGen = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
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
            UpdateEnemyCount();

            if (AllEnemiesDead())
            {
                currentWave++;
                maxEnemies++;

                if (currentWave <= maxWaves)
                {
                    Respawn();
                    if (currentGen.Count < maxEnemies) {
                        int difference = maxEnemies - currentGen.Count;
                        for (int i = 0; i < difference; i++) {
                            SpawnEnemy();
                        }
                    }
                }
                else
                {
                    Debug.Log("Congratulations! You've succesfully defeated all waves of enemies!");
                }
                // Als alle enemies dood zijn, ga naar de volgende wave
                // Verhoog de totale stat points
                // currentTotalStatPoints += toenameTotalStatsPerWave;
                // Spawn een extra enemy voor de volgende wave
                // SpawnNextGen();
                // maxEnemies++;
                // Enemies mogen weer gespawnd worden
            }
        }
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 7.34f / 2, randZ), Quaternion.identity);

        Enemy.name = "enemy";
        enemyStats = enemy.GetComponent<EnemyStats>();
        enemyHealth = enemy.GetComponent<EnemyHealth>();
        // Genereer enemies met toenemende stats per wave
        enemyStats.totalStatPoints = currentTotalStatPoints;
        enemyHealth.spawnPosition = Enemy.transform.position;
        currentGen.Add(Enemy);
    }


    void UpdateEnemyCount()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            if (((GameObject)currentGen[i]) == null)
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
                nextGenEnemy = (GameObject)Instantiate((GameObject)nextGen[i], transform.position + new Vector3(randX, 7.34f / 2, randZ), Quaternion.identity);
                currentGen.Add(nextGenEnemy);
            }
        }

        for (int i = 0; i < nextGen.Count; i++)
        {
            nextGen.Remove(nextGen[i]);
        }
    }

    void Respawn()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            float randX = Random.Range(-maxX / 2, maxX / 2);
            float randZ = Random.Range(-maxZ / 2, maxZ / 2);

            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead = false;
            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().currentHealth = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().startingHealth;
            ((GameObject)currentGen[i]).transform.position = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().spawnPosition;
            Debug.Log(((GameObject)currentGen[i]).GetComponent<EnemyHealth>().spawnPosition);
            //((GameObject)currentGen[i]).transform.position = new Vector3(0, 7.34f / 2, randZ);
        }
    }

    bool AllEnemiesDead()
    {
        int aantal = 0;

        for (int i = 0; i < currentGen.Count; i++)
        {
            if (((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead)
            {
                aantal++;
            }
        }
        return (aantal == maxEnemies);
    }


}