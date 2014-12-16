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
            if (AllEnemiesDead())
            {
                currentWave++;
                if (currentWave <= maxWaves)
                {
                    // Verhoog de totale stat points
                    // currentTotalStatPoints += toenameTotalStatsPerWave;
                    // Spawn een extra enemy voor de volgende wave
                    // maxEnemies++;
                    // Versterk de huidige enemies
                    // BuffEnemies();
                    // nextGenRoulette();
                    // Respawn de enemies
                    Respawn();

                    if (currentGen.Count < maxEnemies) {
                        int difference = maxEnemies - currentGen.Count;
                        for (int i = 0; i < difference; i++) {
                            // Spawn meer enemies als er niet genoeg zijn in de huidige generatie
                            SpawnEnemy();
                        }
                    }
                }
                else
                {
                    Debug.Log("Congratulations! You've succesfully defeated all waves of enemies!");
                }
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
        currentGen.Add(Enemy);
    }

    public void selection(List<float> chances)
    {
        float randomFloat;

        for (int i = 0; i < currentGen.Count; i++)
        {
            randomFloat = Random.Range(0, 1) * chances[chances.Count - 1];
            int indexOfCurrentGen = 0;
            Debug.Log(randomFloat);
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
        for (int i = 0; i < currentGen.Count; i++)
        {
            float randX = Random.Range(-maxX / 2, maxX / 2);
            float randZ = Random.Range(-maxZ / 2, maxZ / 2);

            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().totalStatPoints = currentTotalStatPoints;
            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead = false;
            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().currentHealth = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().startingHealth;
            ((GameObject)currentGen[i]).transform.position = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().spawnPosition;
            //Debug.Log(((GameObject)currentGen[i]).GetComponent<EnemyHealth>().spawnPosition);
            //((GameObject)currentGen[i]).GetComponent<EnemyStats>().mutate(4);
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
        for (int i = 0; i < currentGen.Count; i++)
        {
            List<int> toenameStats = enemyStats.randomNumberGenerator(4, toenameTotalStatsPerWave);
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().health += toenameStats[0];
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().attack += toenameStats[1];
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().defense += toenameStats[2];
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().speed += toenameStats[3];
        }
    }


}