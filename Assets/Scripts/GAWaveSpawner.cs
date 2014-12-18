﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAWaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject nextGenEnemy;
    public int maxEnemies;
    public bool spawning = true;

    public int maxWaves = 10;
    public int currentWave = 1;

    public int minAantalEnemiesPerWave = 2;
    public int maxAantalEnemiesPerWave = 10;

    public float maxX = 1;
    public float maxZ = 1;
    float orcHeigthSpawn = 3.27f;

    public int totalStatPointsPerWave = 600;
    public int toenameStatPointsPerWave = 300;

    public int currentTotalStatPoints;
    public int toenameTotalStatsPerWave = 50;

    public Vector3 spawnPosition;

    public ArrayList allEnemies;
    public ArrayList currentGen;
    public ArrayList nextGen;
    EnemyStats enemyStats;
    EnemyHealth enemyHealth;


    // Use this for initialization
    void Start()
    {
        allEnemies = new ArrayList();
        currentGen = new ArrayList();
        nextGen = new ArrayList();
        generateWave();
        currentTotalStatPoints = totalStatPointsPerWave / maxEnemies;
        Debug.Log("Wave " + currentWave + " / " + maxWaves);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawning)
        {
            if (allEnemies.Count < maxEnemies)
            {
                // Spawn enemies tot het maximale aantal enemies wordt bereikt
                SpawnEnemy();
            }
            else
            {
                spawning = false;
                setCurrentGen();
            }
        }
        else
        {
            if (AllEnemiesDead())
            {
                if (currentWave < maxWaves)
                {
                    // Genereer de volgende wave
                    generateWave();
                    // Verhoog de totale stat points
                    totalStatPointsPerWave += toenameStatPointsPerWave;
                    // Spawnt nieuwe enemies als er een te kort is aan enemies voor de volgende generatie.
                    setAllEnemies();
                    // Selecteert de enemies voor de volgende generatie
                    setCurrentGen(); 
                    // Versterk de enemies in de volgende generatie
                    BuffEnemies();
                    // Spawnt de volgende generatie
                    Respawn();
                    // Verhoog de wave count
                    currentWave++;
                    Debug.Log("Wave " + currentWave + " / " + maxWaves);
                }
                else
                {
                    Debug.Log("Congratulations! You've succesfully defeated all waves of enemies!");
                }
            }
        }
    }

    void setAllEnemies()
    {
        if (allEnemies.Count < maxEnemies)
        {
            int difference = maxEnemies - allEnemies.Count;
            for (int i = 0; i < difference; i++)
            {
                SpawnEnemy();
            }
        }
    }

    void setCurrentGen()
    {
        currentGen.Clear();

        for (int i = 0; i < maxEnemies; i++)
        {
            currentGen.Add((GameObject)allEnemies[i]);
        }
    }

    void updateCurrentGen()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            if (((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead)
            {
                currentGen.Remove(currentGen[i]);
            }
        }
    }

    
    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 7.34f / 2, randZ), Quaternion.identity);

        Enemy.name = "enemy";
        enemyStats = Enemy.GetComponent<EnemyStats>();
        enemyHealth = Enemy.GetComponent<EnemyHealth>();
        // Genereer enemies met toenemende stats per wave
        enemyStats.totalStatPoints = currentTotalStatPoints;
        // Genereer stats van de enemy
        enemyStats.generateEnemyStats();
        enemyHealth.spawnPosition = Enemy.transform.position;
        allEnemies.Add(Enemy);
    }

    public void generateWave()
    {
        // kiest een random integer tussen minAantalEnemiesPerWave en maxAantalEnemiesPerWave
        int randomInt = Random.Range(minAantalEnemiesPerWave, maxAantalEnemiesPerWave + 1);

        maxEnemies = randomInt;
    }

    public void selection(List<float> chances)
    {
        float randomFloat;

        for (int i = 0; i < currentGen.Count; i++)
        {
            randomFloat = Random.Range(0, 1) * chances[chances.Count - 1];
            int indexOfCurrentGen = 0;
            /*while (randomFloat > chances[indexOfCurrentGen])
            {
                indexOfCurrentGen++;
            }*/
            nextGen.Add(currentGen[indexOfCurrentGen]);
        }
        currentGen = nextGen;
    }

    public void nextGenRoulette()
    {
        float total = 0;
        List<float> chances = new List<float>();

        for (int i = 0; i < currentGen.Count; i++)
        {
            total += ((GameObject)currentGen[i]).GetComponent<EnemyStats>().fitness();
            // upper limit on the roulette wheel of current enemy
            chances.Add(total);
        }
        selection(chances);
    }

    void Respawn()
    {
        foreach (GameObject enemy in currentGen)
        {
            enemy.GetComponent<EnemyHealth>().isDead = false;
            enemy.GetComponent<EnemyStats>().respawn = true;
            enemy.GetComponent<EnemyStats>().totalStatPoints = totalStatPointsPerWave / maxEnemies;
            enemy.transform.position = enemy.GetComponent<EnemyHealth>().spawnPosition;
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

    void BuffEnemies()
    {
        foreach (GameObject enemy in currentGen)
        {
            List<int> toenameStats = enemyStats.randomNumberGenerator(3, totalStatPointsPerWave / maxEnemies);
            enemy.GetComponent<EnemyStats>().health = toenameStats[0];
            enemy.GetComponent<EnemyStats>().attack = toenameStats[1];
            enemy.GetComponent<EnemyStats>().defense = toenameStats[2];
            enemy.GetComponent<EnemyStats>().speedMultiplier = Random.RandomRange(0.80f, 1.20f);
        }
    }

}