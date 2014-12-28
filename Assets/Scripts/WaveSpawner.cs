using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    private GameObject EnemyGuyant;
    private GameObject EnemyGwarf;
    private GameObject EnemyGrobble;

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
        EnemyGuyant = resourceManager.enemyGuyant;
        EnemyGwarf = resourceManager.enemyGwarf;
        EnemyGrobble = resourceManager.enemyGrobble;
		maxWaves = resourceManager.maxWaves;
		currentWave = resourceManager.currentWave;
        enemies = new ArrayList();

        gui = GameObject.Find("GUIMain");
        waveText = GameObject.Find("WaveNumberText").GetComponent<Text>();
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
                        Spawnenemy();
                    }
                }
                else
                {
                    spawning = false;
                }
            }
            else
            {
                UpdateenemyCount();

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

    void Spawnenemy()
    {

        float enemyNumber = Mathf.Round(Random.Range(1f, 2f));
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        if (enemyNumber == 1)
        {

            GameObject enemyGuyant = (GameObject)Instantiate(EnemyGuyant, transform.position + new Vector3(randX, 7.34f / 4, randZ), Quaternion.identity);
            enemyGuyant.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyGuyant.name = "enemyGuyant";
            enemyStats = enemyGuyant.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            enemyStats.generateenemyStats();
            enemies.Add(enemyGuyant);
        }
        else if (enemyNumber == 2)
        {
            GameObject enemyGwarf = (GameObject)Instantiate(EnemyGwarf, transform.position + new Vector3(randX, 1.38f, randZ), Quaternion.identity);
            enemyGwarf.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyGwarf.name = "enemyGwarf";
            enemyStats = enemyGwarf.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            enemyStats.generateenemyStats();
            enemies.Add(enemyGwarf);
        }
    }

    void UpdateenemyCount()
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

