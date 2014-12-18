using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    private GameObject enemy;
    public int maxEnemies = 5;
    public bool spawning = true;

	private int maxWaves;
	private int currentWave;

    public float maxX;
    public float maxZ;
    //float orcHeigthSpawn = 3.27f;


    public int currentTotalStatPoints = 100;
    public int delta = 50;

    public ArrayList enemies;
    EnemyStats enemyStats;

    // Use this for initialization
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		enemy = resourceManager.enemyGuyant;
		maxWaves = resourceManager.maxWaves;
		currentWave = resourceManager.currentWave;
        enemies = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (spawning)
            {
                if (enemies.Count < maxEnemies)
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

                if (enemies.Count == 0)
                {
                    // Als alle enemies dood zijn, ga naar de volgende wave
                    currentWave++;
                    // Spawn een extra enemy voor de volgende wave
                    maxEnemies++;
                    // Verhoog de totale stat points
                    currentTotalStatPoints += delta;
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

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 7.34f/2, randZ), Quaternion.identity);
       
        Enemy.name = "enemy";
        enemyStats = Enemy.GetComponent<EnemyStats>();
        // Genereer enemies met toenemende stats per wave
        enemyStats.totalStatPoints = currentTotalStatPoints;
        // enemyStats.generateEnemyStats();
        enemies.Add(Enemy);
    }

    void UpdateEnemyCount()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if ((GameObject)(enemies[i]) == null)
            {
                // Verwijder een enemy uit de lijst van enemies als die dood is
                enemies.Remove(enemies[i]);
            }
        }
    }

}

