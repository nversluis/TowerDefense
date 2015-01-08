﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{

    private List<int> stats;
    public List<float> statDistribution;
    public int totalStatPoints = 100;

    // Stats van een enemy
    public int health;
    public int attack;
    public int defense;
    public float speedMultiplier;
    public float dfactor;
    public float goalImportance;
    public float playerImportance;

    // statdistribution of enemy
    public float healthDistributionFactor;
    public float attackDistributionFactor;
    public float defenseDistributionFactor;

    public bool respawn = false;
    public int statsMutation = 1;

    public int totalDamage;
    EnemyResources enemyResources;
    EnemyHealth enemyHealth;


    void Awake()
    {
        generateDistribution();
        enemyResources = GetComponent<EnemyResources>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            enemyHealth.startingHealth = health * 10;
            enemyHealth.defense = defense;
            enemyHealth.currentHealth = enemyHealth.startingHealth;
            enemyResources.attackDamage = attack;
            respawn = false;
        }
    }

    public void generateenemyStats()
    {

        this.health = (int)(healthDistributionFactor * totalStatPoints);
        this.attack = (int)(attackDistributionFactor * totalStatPoints);
        this.defense = (int)(defenseDistributionFactor * totalStatPoints);

        this.speedMultiplier = Random.Range(0.90f, 1.10f);
        this.dfactor = Random.Range(0.05f, 0.80f);
        this.goalImportance = Random.Range(0.4f, 1f);
        this.playerImportance = Random.Range(0, 0.6f);
    }

    public void generateDistribution()
    {
        // Als enemy nog geen distributie heeft
        if (statDistribution.Count == 0)
        {
            List<int> temp = randomNumberGenerator(3, 100);

            this.healthDistributionFactor = (float)temp[0] / 100;
            this.attackDistributionFactor = (float)temp[1] / 100;
            this.defenseDistributionFactor = (float)temp[2] / 100;

            statDistribution.Add(healthDistributionFactor);
            statDistribution.Add(attackDistributionFactor);
            statDistribution.Add(defenseDistributionFactor);
        }
        else
        {
            this.healthDistributionFactor = statDistribution[0];
            this.attackDistributionFactor = statDistribution[1];
            this.defenseDistributionFactor = statDistribution[2];
        }
    }

    public int fitness()
    {
        return totalDamage + 1;
    }

    /// <summary>
    /// Genereert een lijst van n random numbers die opsommen tot sum.
    /// </summary>
    /// <param name="n"></param> Het aantal random numbers
    /// <param name="sum"></param> De som van de random numbers
    /// <returns></returns>
    public List<int> randomNumberGenerator(int n, int sum)
    {
        List<int> randomNumbers = new List<int>();

        // Voeg 0 en sum toe aan de lijst
        randomNumbers.Add(0);
        randomNumbers.Add(sum);

        int i = 0;
        // Kies n - 1 random getallen tussen 0 en sum en voeg deze toe in de lijst
        while (i < n - 1)
        {
            int randomInt = Random.Range(0, sum);
            // Voeg alleen toe als de random waarde nog niet voorkomt in de lijst
            if (!randomNumbers.Contains(randomInt))
            {
                randomNumbers.Add(randomInt);
                i++;
            }
        }

        // Sorteer de lijst
        randomNumbers.Sort();

        // Maak een nieuwe lijst aan
        List<int> result = new List<int>();

        for (int j = 0; j < n; j++)
        {
            // Voeg het vershil van de elementen toe aan de nieuwe lijst, het verschil is nooit 0 omdat elk getal in de lijst verschillend is.
            result.Add(randomNumbers[j + 1] - randomNumbers[j]);
        }

        // De lijst bevat nu n willekeurige getallen die opsommen tot sum.
        return result;
    }

    /// <summary>
    /// Muteert een enemy door x willekeurige stats te verhogen en n - x te verlagen
    /// </summary>
    /// <param name="n"></param> Het aantal stats van een enemy
    /// <param name="delta"></param> De toename van een stat
    public void mutate(int n)
    {
        // Het aantal stats dat verhoogd moet worden
        int aantal = Random.Range(1, n - 1);
        List<int> verhoogIndices = new List<int>();

        for (int i = 0; i < aantal; i++)
        {
            // Bepaald welk index van stats verhoogd wordt
            int index = Random.Range(0, n - 1);
            verhoogIndices.Add(index);
        }

        List<int> verlaagIndices = new List<int>();

        // Voegt de indices die niet voorkomen in verhoogIndices aan verlaagIndices toe
        for (int i = 0; i < n - aantal; i++)
        {
            int index = Random.Range(0, n - 1);
            while (!verhoogIndices.Contains(index) && verlaagIndices.Count < n - aantal)
            {
                verlaagIndices.Add(index);
            }
        }

        for (int i = 0; i < verhoogIndices.Count; i++)
        {
            // Verhoog de stats 
            stats[verhoogIndices[i]] += statsMutation;
        }

        // Bepaald hoeveel elk stat wordt verlaagd, de som van de statverlagingen is gelijk aan die van de statverhogingen
        List<int> statverlaging = randomNumberGenerator(n - aantal, aantal * statsMutation);

        for (int i = 0; i < verlaagIndices.Count; i++)
        {
            // Verlaag de stats
            stats[verlaagIndices[i]] -= statverlaging[i];
        }
    }

}



