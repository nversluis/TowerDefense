﻿using UnityEngine;
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
    public bool keepDistribution = false;

    GameObject gui;
    GUIScript guiScript;

    public int toenameAantalEnemiesPerWave;

    private int maxWaves;
    private int currentWave;

    public float maxX;
    public float maxZ;
    //float orcHeigthSpawn = 3.27f;
    private PlayerData playerData = GUIScript.player;

    private bool gameHasStarted = false;
    bool Won;

    private float timer;
    public int timeBeforeFirstWave;
    public int timeBetweenWaves;
    private int waitTime;

    public int currentTotalStatPoints ;
    public int delta;

    public float spawnTime; // in seconden

    public ArrayList enemies;
    List<List<float>> statDistributions;
    EnemyStats enemyStats;

    Text waveText;
    // Use this for initialization
    void Start()
    {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        EnemyGuyant = resourceManager.enemyGuyant;
        EnemyGwarf = resourceManager.enemyGwarf;
        EnemyGrobble = resourceManager.enemyGrobble;
        maxWaves = resourceManager.maxWaves;
        currentWave = resourceManager.currentWave;
        enemies = new ArrayList();
        statDistributions = new List<List<float>>();

        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (!gameHasStarted)
            {
            //    Debug.Log("Press enter to start the waves");
            }

            if (spawning)
            {
                if (currentWave == 1)
                {
                    if (Input.GetKeyDown("return"))
                    {
                        gameHasStarted = true;
                    }
                    if (gameHasStarted)
                    {
                        waitTime = 0;
                    }
                    else
                    {
                        waitTime = int.MaxValue;
                    }
                }
                else
                {
                    waitTime = timeBetweenWaves;
                }

                timer += Time.deltaTime;
                //Debug.Log("timer: " + timer + " " + "waitTime: " + waitTime);

                if (timer > waitTime)
                {
                    if (enemies.Count < maxEnemies)
                    {
                        float randomFloat = Random.Range(0.0f, 1.0f);
                        if (randomFloat < (float)(1 / (spawnTime * 60)))
                        {
                            // Spawn enemies tot het maximale aantal enemies wordt bereikt
                            Spawnenemy();
                        }
                    }
                    else
                    {
                        spawning = false;
                        keepDistribution = true;
                        timer = 0;
                        /*
                        Debug.Log("statDistribution[0][0]: " + statDistributions[0][0]);
                        Debug.Log("statDistribution[0][1]: " + statDistributions[0][1]);
                        Debug.Log("statDistribution[0][2]: " + statDistributions[0][2]);
                        Debug.Log("statDistribution[1][0]: " + statDistributions[1][0]);
                        Debug.Log("statDistribution[1][1]: " + statDistributions[1][1]);
                        Debug.Log("statDistribution[1][2]: " + statDistributions[1][2]);*/
                    }
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
            enemyGuyant.name = "Guyant";
            enemyStats = enemyGuyant.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            if (keepDistribution)
            {
                enemyStats.statDistribution = getRandomDistribution();
                enemyStats.generateDistribution();
                //Debug.Log(enemyStats.statDistribution[0]);
                //Debug.Log(enemyStats.statDistribution[1]);
                //Debug.Log(enemyStats.statDistribution[2]);
            }
            //Debug.Log(enemyStats.statDistribution[0]);
            //Debug.Log(enemyStats.statDistribution[1]);
            //Debug.Log(enemyStats.statDistribution[2]);
            enemyStats.generateenemyStats();
            statDistributions.Add(enemyStats.statDistribution);
            enemies.Add(enemyGuyant);
        }
        else if (enemyNumber == 2)
        {
            GameObject enemyGwarf = (GameObject)Instantiate(EnemyGwarf, transform.position + new Vector3(randX, 1.38f, randZ), Quaternion.identity);
            enemyGwarf.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyGwarf.name = "Gwarf";
            enemyStats = enemyGwarf.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            if (keepDistribution)
            {
                enemyStats.statDistribution = getRandomDistribution();
                enemyStats.generateDistribution();
                //Debug.Log(enemyStats.statDistribution[0]);
                //Debug.Log(enemyStats.statDistribution[1]);
                //Debug.Log(enemyStats.statDistribution[2]);
            }
            //Debug.Log(enemyStats.statDistribution[0]);
            //Debug.Log(enemyStats.statDistribution[1]);
            //Debug.Log(enemyStats.statDistribution[2]);
            enemyStats.generateenemyStats();
            statDistributions.Add(enemyStats.statDistribution);
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

    public List<float> getRandomDistribution()
    {
        int randomElement = Random.Range(0, statDistributions.Count);
        Debug.Log(randomElement);
        List<float> distribution = statDistributions[randomElement];

        return distribution;
    }

    public void clearDistributions()
    {
        statDistributions.Clear();
    }

}

