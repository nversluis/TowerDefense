using UnityEngine;
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

    public int minAantalEnemiesPerWave;
    public int maxAantalEnemiesPerWave;
    public int toenameAantalEnemiesPerWave = 5;

    public float maxX = 1;
    public float maxZ = 1;
    float orcHeigthSpawn = 3.27f;

    public int totalStatPointsPerWave = 1400;
    public int toenameStatPointsPerWave = 200;

    public int currentTotalStatPoints;


    public Vector3 spawnPosition;

    public ArrayList allEnemies;
    public ArrayList currentGen;
    public ArrayList nextGen;
    EnemyStats enemyStats;
    EnemyHealth enemyHealth;

    private GameObject ResourceManagerObj;
    private ResourceManager resourceManager;

    public float counter;
    public float spawnTime = 2; // in seconden
    public float timeTillNextWave = 10;

    // Use this for initialization
    void Start()
    {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        allEnemies = new ArrayList();
        currentGen = new ArrayList();
        nextGen = new ArrayList();
        generateWave();
        currentTotalStatPoints = totalStatPointsPerWave / maxEnemies;
        //Debug.Log("Wave " + currentWave + " / " + maxWaves);
        minAantalEnemiesPerWave = resourceManager.minEnemies;
        maxAantalEnemiesPerWave = resourceManager.maxEnemies;
        Debug.Log(maxEnemies);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawning)
        {
            if (allEnemies.Count < maxEnemies)
            {
                float randomFloat = Random.Range(0.0f, 1.0f);
                if (randomFloat < (float)1 / (spawnTime * 60))
                {
                    SpawnEnemy();
                }
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
                    minAantalEnemiesPerWave += toenameAantalEnemiesPerWave;
                    maxAantalEnemiesPerWave += toenameAantalEnemiesPerWave;
                    // Genereer de volgende wave
                    generateWave();
                    Debug.Log(maxEnemies);
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
            Debug.Log("difference" + difference);
            for (int i = 0; i < difference; i++)
            {
                SpawnEnemy();
            }
        }
        Debug.Log("AllEnemies.COunt" + allEnemies.Count);
    }

    void setCurrentGen()
    {
        currentGen.Clear();

        for (int i = 0; i < maxEnemies; i++)
        {
            currentGen.Add((GameObject)allEnemies[i]);
        }
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 7.34f / 4, randZ), Quaternion.identity);
        Enemy.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
        float randomFloat = Random.Range(0.0f, 1.0f);
        int i = 0;

        if (randomFloat < (float) 1 / (spawnTime * 60) && i < currentGen.Count)
        {
            ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().isDead = false;
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().respawn = true;
            ((GameObject)currentGen[i]).GetComponent<EnemyStats>().totalStatPoints = totalStatPointsPerWave / maxEnemies;
            ((GameObject)currentGen[i]).transform.position = ((GameObject)currentGen[i]).GetComponent<EnemyHealth>().spawnPosition;
            i++;
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
        Debug.Log(aantal + " / " + maxEnemies);
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