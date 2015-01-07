using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAWaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject EnemyGuyant;
    public GameObject EnemyGwarf;
    public GameObject nextGenenemy;
    public int maxEnemies;
    public bool spawning = true;

    public int maxWaves = 10;
    public int currentWave = 1;

    GameObject gui;
    GUIScript guiScript;

    public int minAantalEnemiesPerWave = 2;
    public int maxAantalEnemiesPerWave = 10;

    public float maxX = 1;
    public float maxZ = 1;
    float orcHeigthSpawn = 3.27f;

    public int totalStatPointsPerWave = 600;
    public int toenameStatPointsPerWave = 300;

    public int currentTotalStatPoints;
    public int toenameTotalStatsPerWave = 50;

    public bool won;

    public Vector3 spawnPosition;

    public ArrayList allEnemies;
    public ArrayList currentGen;
    public ArrayList nextGen;
    EnemyStats enemyStats;
    EnemyHealth enemyHealth;

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;

    // Use this for initialization
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        EnemyGuyant = resourceManager.enemyGuyant;
        EnemyGwarf = resourceManager.enemyGwarf;
        allEnemies = new ArrayList();
        currentGen = new ArrayList();
        nextGen = new ArrayList();
        generateWave();
        currentTotalStatPoints = totalStatPointsPerWave / maxEnemies;
        //Debug.Log("Wave " + currentWave + " / " + maxWaves);
		minAantalEnemiesPerWave = resourceManager.minEnemies;
		maxAantalEnemiesPerWave = resourceManager.maxEnemies;

        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawning)
        {
            if (allEnemies.Count < maxEnemies)
            {
                Spawnenemy();
            }
            else
            {
                spawning = false;
                setCurrentGen();
                //Debug.Log(currentGen.Count);
            }
        }
        else
        {
            if (AllEnemiesDead())
            {
                Debug.Log("Alle enemies zijn dood");
				playerData.addGold(resourceManager.rewardWave);

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
                    //Debug.Log("Wave " + currentWave + " / " + maxWaves);
                }
                else
                {
                    won = true;
                }
            }
        }

        if (won)
        {
            guiScript.EndGame("You Won!");
        }
    }

    public void generateWave()
    {
        // kiest een random integer tussen minAantalEnemiesPerWave en maxAantalEnemiesPerWave
        int randomInt = Random.Range(minAantalEnemiesPerWave, maxAantalEnemiesPerWave + 1);

        maxEnemies = randomInt;
    }

    bool AllEnemiesDead()
    {
        int aantal = 0;

        for (int i = 0; i < currentGen.Count; i++)
        {
            //Debug.Log(((GameObject)currentGen[i]).GetComponent<EnemyResources>().isDead);
            if (((GameObject)currentGen[i]).GetComponent<EnemyResources>().isDead)
            {                
                aantal++;
            }
        }
        Debug.Log(aantal + " / " + maxEnemies);
        return (aantal == maxEnemies);
    }

    void setAllEnemies()
    {
        if (allEnemies.Count < maxEnemies)
        {
            int difference = maxEnemies - allEnemies.Count;
            for (int i = 0; i < difference; i++)
            {
                Spawnenemy();
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
            enemyStats.generateenemyStats();
            allEnemies.Add(enemyGuyant);
        }
        else if (enemyNumber == 2)
        {
            GameObject enemyGwarf = (GameObject)Instantiate(EnemyGwarf, transform.position + new Vector3(randX, 1.38f, randZ), Quaternion.identity);
            enemyGwarf.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyGwarf.name = "Gwarf";
            enemyStats = enemyGwarf.GetComponent<EnemyStats>();
            // Genereer enemies met toenemende stats per wave
            enemyStats.totalStatPoints = currentTotalStatPoints;
            enemyStats.generateenemyStats();
            allEnemies.Add(enemyGwarf);
        }
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
            enemy.GetComponent<EnemyResources>().isDead = false;
            enemy.GetComponent<EnemyStats>().respawn = true;
            enemy.GetComponent<EnemyStats>().totalStatPoints = totalStatPointsPerWave / maxEnemies;
            enemy.transform.position = enemy.GetComponent<EnemyHealth>().spawnPosition;
        }
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