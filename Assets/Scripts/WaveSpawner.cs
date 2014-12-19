using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    private GameObject enemy;
    public int maxEnemies = 15;
    public bool spawning = true;

    GameObject gui;
    GUIScript guiScript;
    
    public int toenameAantalEnemiesPerWave = 5;

	private int maxWaves;
	private int currentWave;

    public float maxX;
    public float maxZ;
    //float orcHeigthSpawn = 3.27f;
	private PlayerData playerData = GUIScript.player;

    bool Won;

    public int currentTotalStatPoints = 250;
    public int delta = 20;

    public float spawnTime = 0.5f; // in seconden

    public ArrayList enemies;
    EnemyStats enemyStats;

    Text waveText;
    // Use this for initialization
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		enemy = resourceManager.enemyGuyant;
		maxWaves = resourceManager.maxWaves;
		currentWave = resourceManager.currentWave;
        enemies = new ArrayList();

        gui = GameObject.Find("GUIMain");
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        guiScript = gui.GetComponent<GUIScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        waveText.text = (currentWave + " / " + maxWaves);

        if (currentWave <= maxWaves)
        {
            if (spawning)
            {
                if (enemies.Count < maxEnemies)
                {
                    float randomFloat = Random.Range(0.0f, 1.0f);
                    if (randomFloat < (float) (1 / (spawnTime * 60)))
                    {
                        // Spawn enemies tot het maximale aantal enemies wordt bereikt
                        SpawnEnemy();
                    }
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
					// Voeg gold toe voor de speler na elke wave
					playerData.addGold(resourceManager.rewardWave);
                    // Als alle enemies dood zijn, ga naar de volgende wave
                    currentWave++;
                    // Verhoog het aantal enemies in de wave
                    maxEnemies += toenameAantalEnemiesPerWave;
                    // Verhoog de totale stat points
                    currentTotalStatPoints += delta;
                    // Enemies mogen weer gespawnd worden
                    spawning = true;
                }
            }
        }
        else
        {
            Won = true;
        }

        if (Won)
        {
            guiScript.EndGame("You Won!");
        }
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 7.34f / 4, randZ), Quaternion.identity);
        Enemy.gameObject.transform.localScale= new Vector3(0.5f, 0.5f, 0.5f);
        Enemy.name = "enemy";
        enemyStats = Enemy.GetComponent<EnemyStats>();
        // Genereer enemies met toenemende stats per wave
        enemyStats.totalStatPoints = currentTotalStatPoints;
        enemyStats.generateEnemyStats();
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

