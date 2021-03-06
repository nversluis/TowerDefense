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

    public int maxEnemies;
    public int startMaxEnemies;
    public int startTotalStatPoints;

    public float mutationProbability;

    private int indexOfCurrentGen;
    private bool spawning = true;
    private bool eersteWaveDoorlopen = false;
    public bool keepDistribution;
    public bool keepType;
    private bool counting;

    GameObject gui;
    GUIScript guiScript;

    public int toenameAantalEnemiesPerWave;

    private int maxWaves;
    private int currentWave;

    public int startRewardWave;

    public float maxX;
    public float maxZ;
    //float orcHeigthSpawn = 3.27f;
    private PlayerData playerData = GUIScript.player;

    bool Won = false;
    private bool gameHasStarted = false;
    public bool allEnemiesSpawned = false;
    public bool allEnemiesDead = false;

    private float timer;
    public int timeBetweenWaves;
    private int waitTime;

    public int currentTotalStatPoints;
    public int delta;

    public float spawnTime; // in seconden
    public float RewardWaveFactor;

    public ArrayList enemies;
    public ArrayList enemiesInWave;
    List<List<float>> currentGenDistributions;
    List<List<float>> nextGenDistributions;
    List<float> currentGenFitness;
    List<float> nextGenFitness;
    List<float> currentGenType;
    List<float> nextGenType;
    EnemyStats enemyStats;

    Statistics statistics;

    Text waveText;
    // Use this for initialization
    void Start()
    {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        EnemyGuyant = resourceManager.enemyGuyant;
        EnemyGwarf = resourceManager.enemyGwarf;
        EnemyGrobble = resourceManager.enemyGrobble;
        maxWaves = resourceManager.maxWaves[ResourceManager.Difficulty];
        currentWave = resourceManager.currentWave;
		toenameAantalEnemiesPerWave = (resourceManager.toenameAantalEnemiesPerWave)[ResourceManager.Difficulty];
        timeBetweenWaves = resourceManager.timeBetweenWaves[ResourceManager.Difficulty];
		currentTotalStatPoints = resourceManager.totalStatPoints[ResourceManager.Difficulty];
		maxEnemies = resourceManager.maxEnemies[ResourceManager.Difficulty];
		delta = resourceManager.toenameTotalStatPointsPerWave[ResourceManager.Difficulty];
        keepType = resourceManager.keepType;
        keepDistribution = resourceManager.keepDistribution;
        currentTotalStatPoints = (currentWave - 1) * delta + currentTotalStatPoints;
		resourceManager.totalStatPoints[ResourceManager.Difficulty] = currentTotalStatPoints;
        maxEnemies = (currentWave - 1) * toenameAantalEnemiesPerWave + maxEnemies;
		resourceManager.maxEnemies[ResourceManager.Difficulty] = maxEnemies;
		startMaxEnemies = resourceManager.maxEnemies[ResourceManager.Difficulty];
		startTotalStatPoints = resourceManager.totalStatPoints[ResourceManager.Difficulty];
        resourceManager.currentSpawnTime = resourceManager.startSpawnTime[ResourceManager.Difficulty] * Mathf.Pow(resourceManager.AfnameSpawnTimeFactor[ResourceManager.Difficulty], (currentWave - 1));
        spawnTime = resourceManager.currentSpawnTime;
        startRewardWave = resourceManager.rewardWave[ResourceManager.Difficulty];
        RewardWaveFactor = resourceManager.rewardWaveFactor[ResourceManager.Difficulty];
        enemies = new ArrayList();
        enemiesInWave = new ArrayList();
        currentGenDistributions = new List<List<float>>();
        nextGenDistributions = new List<List<float>>();
        currentGenFitness = new List<float>();
        nextGenFitness = new List<float>();
        currentGenType = new List<float>();
        nextGenType = new List<float>();

        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            gameHasStarted = true;
        }
    }

    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (!gameHasStarted)
            {
                //    Debug.Log("Press enter to start the waves");
            }

            UpdateCount();

            if (spawning)
            {
                allEnemiesSpawned = false;

                if (currentWave == 1)
                {
                    if (gameHasStarted)
                    {
                        //waitTime = 0;
                        waitTime = timeBetweenWaves;
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

                if (gameHasStarted)
                {
                    timer += Time.deltaTime;
                }

                if (waitTime < int.MaxValue && waitTime - timer > 0)
                {
                    if (counting) {
                        guiScript.WaveCountdown(resourceManager.timeBetweenWaves[ResourceManager.Difficulty]);
                        counting = false;
                    }
                }

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
                        allEnemiesSpawned = true;
                        spawning = false;

                        eersteWaveDoorlopen = true;

                        currentGenType.Clear();
                        currentGenType = new List<float>(nextGenType);
                        nextGenType.Clear();

                        currentGenDistributions.Clear();
                        currentGenDistributions = new List<List<float>>(nextGenDistributions);
                        nextGenDistributions.Clear();

                        currentGenFitness.Clear();
                        currentGenFitness = new List<float>(nextGenFitness);
                        nextGenFitness.Clear();

                        timer = 0;
                    }
                }
            }
            else
            {
                if (enemies.Count == 0)
                {
                    resourceManager.rewardWave[ResourceManager.Difficulty] = (int)Mathf.Round((startRewardWave * Mathf.Pow(RewardWaveFactor, (currentWave - 1))));
                    // Voeg gold toe voor de speler na elke wave
                    playerData.addGold(resourceManager.rewardWave[ResourceManager.Difficulty]);
                    // Ga naar de volgende wave
                    currentWave++;
                    resourceManager.currentWave++;
                    // Verhoog het aantal enemies in de wave
					maxEnemies = (currentWave - 1) * resourceManager.toenameAantalEnemiesPerWave[ResourceManager.Difficulty] + startMaxEnemies;
					resourceManager.maxEnemies[ResourceManager.Difficulty] = maxEnemies;
                    // Verhoog de totale stat points
                    currentTotalStatPoints = (currentWave - 1) * delta + startTotalStatPoints;
					resourceManager.totalStatPoints[ResourceManager.Difficulty] = currentTotalStatPoints;
                    //currentTotalStatPoints += delta;
                    spawnTime = resourceManager.startSpawnTime[ResourceManager.Difficulty] * Mathf.Pow(resourceManager.AfnameSpawnTimeFactor[ResourceManager.Difficulty], (currentWave - 1));
                    resourceManager.currentSpawnTime = spawnTime;
                    // Wave countdown mag weer plaatsvinden
                    counting = true;
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
            PlayerController.waveEnded = true;
            Invoke("Winning", 4f);

        }
    }

    void Winning()
    {
        currentWave = maxWaves;
        resourceManager.currentWave = maxWaves;
        guiScript.resultScoreText.text = Statistics.Score().ToString();
        guiScript.EndGame("You Won!");
        Won = false; 
    }

    void Spawnenemy()
    {
        int enemyNumber = Random.Range(1, 4);
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        if (eersteWaveDoorlopen)
        {
            selectEnemy();
            if (keepType)
            {
                enemyNumber = (int)currentGenType[indexOfCurrentGen];
            }
        }

        if (enemyNumber == 1)
        {
            GameObject enemyGuyant = (GameObject)Instantiate(EnemyGuyant, transform.position + new Vector3(randX, 7.34f / 4, randZ), Quaternion.identity);
            enemyGuyant.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyGuyant.name = "Guyant";
            enemyStats = enemyGuyant.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            if (eersteWaveDoorlopen)
            {
                if (keepDistribution)
                {
                    enemyStats.statDistribution = getDistribution(indexOfCurrentGen);
                    enemyStats.mutate(mutationProbability);
                    enemyStats.generateDistribution();
                }
            }
            enemyStats.generateenemyStats();
            nextGenType.Add(enemyNumber);
            nextGenDistributions.Add(enemyStats.statDistribution);
            nextGenFitness.Add(enemyStats.fitness);
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
            if (eersteWaveDoorlopen)
            {
                if (keepDistribution)
                {
                    enemyStats.statDistribution = getDistribution(indexOfCurrentGen);
                    enemyStats.mutate(mutationProbability);
                    enemyStats.generateDistribution();
                }
            }
            enemyStats.generateenemyStats();
            nextGenType.Add(enemyNumber);
            nextGenDistributions.Add(enemyStats.statDistribution);
            nextGenFitness.Add(enemyStats.fitness);
            enemies.Add(enemyGwarf);
        }
        else if (enemyNumber == 3)
        {
            GameObject enemyGrobble = (GameObject)Instantiate(EnemyGrobble, transform.position + new Vector3(randX, 1f, randZ), Quaternion.identity);
            enemyGrobble.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            enemyGrobble.name = "Grobble";
            enemyStats = enemyGrobble.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            if (eersteWaveDoorlopen)
            {
                if (keepDistribution)
                {
                    enemyStats.statDistribution = getDistribution(indexOfCurrentGen);
                    enemyStats.mutate(mutationProbability);
                    enemyStats.generateDistribution();
                }
            }
            enemyStats.generateenemyStats();
            nextGenType.Add(enemyNumber);
            nextGenDistributions.Add(enemyStats.statDistribution);
            nextGenFitness.Add(enemyStats.fitness);
            enemies.Add(enemyGrobble);
        }
    }

    void UpdateCount()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if ((GameObject)(enemies[i]) != null)
                {
                    if (!allEnemiesSpawned)
                    {
                        nextGenFitness[i] = ((GameObject)(enemies[i])).GetComponent<EnemyStats>().fitness;
                        //Debug.Log("nextGenFitness[" + i + "] = " + nextGenFitness[i]);
                    }
                    else
                    {
                        currentGenFitness[i] = ((GameObject)(enemies[i])).GetComponent<EnemyStats>().fitness;
                        //Debug.Log("currentGenFitness[" + i + "] = " + currentGenFitness[i]);
                    }
                }
            }
            if (AllEnemiesDead())
            {
                enemies.Clear();
            }
        }
    }

    bool AllEnemiesDead()
    {
        int aantal = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if ((GameObject)(enemies[i]) == null)
            {
                aantal++;
            }
        }
        return (aantal == maxEnemies);
    }

    public List<float> getRandomDistribution()
    {
        int randomElement = Random.Range(0, currentGenDistributions.Count);
        List<float> distribution = currentGenDistributions[randomElement];

        return distribution;
    }

    /*public List<float> getRouletteWheelDistribution()
    {
        float total = 0;
        float randomFloat;
        List<float> chances = new List<float>();

        for (int i = 0; i < currentGenFitness.Count; i++)
        {
            // upper limit on the roulette wheel of current enemy
            total += currentGenFitness[i];
            chances.Add(total);
        }

        randomFloat = Random.Range(0.0f, 1.0f) * chances[chances.Count - 1];
        indexOfCurrentGen = 0;
        while (randomFloat > chances[indexOfCurrentGen])
        {
            indexOfCurrentGen++;
        }

        List<float> distribution = currentGenDistributions[indexOfCurrentGen];
        return distribution;
    }*/

    public List<float> getDistribution(int index)
    {
        return currentGenDistributions[index];
    }

    public void selectEnemy()
    {
        float total = 0;
        float randomFloat;
        List<float> chances = new List<float>();

        for (int i = 0; i < currentGenFitness.Count; i++)
        {
            // upper limit on the roulette wheel of current enemy
            total += currentGenFitness[i];
            chances.Add(total);
        }

        randomFloat = Random.Range(0.0f, 1.0f) * chances[chances.Count - 1];
        indexOfCurrentGen = 0;
        while (randomFloat > chances[indexOfCurrentGen])
        {
            indexOfCurrentGen++;
        }
        //return indexOfCurrentGen;
    }

    public List<float> getBestDistribution()
    {
        int index;
        float maxValue = float.MinValue;

        for (int i = 0; i < currentGenFitness.Count; i++)
        {
            if (currentGenFitness[i] > maxValue)
            {
                maxValue = currentGenFitness[i];
            }
        }
        index = currentGenFitness.IndexOf(maxValue);
        List<float> distribution = currentGenDistributions[index];

        return distribution;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetMaxWave()
    {
        return maxWaves;
    }


}

