using UnityEngine;
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
    public int magicDefense;
    public float speedMultiplier;
    public float dfactor;
    public float goalImportance;
    public float playerImportance;

    // statdistribution of enemy
    public float healthDistributionFactor;
    public float attackDistributionFactor;
    public float defenseDistributionFactor;
    public float magicDefenseDistributionFactor;

    public bool respawn = false;

    // geeft aan hoeveel % de som van de stats maximaal mag veranderen
    public int statMutation = 24;

    public int totalDamage;
    public float leeftijd;
    public float goalDistance;
    public float fitness;
    EnemyResources enemyResources;
    EnemyHealth enemyHealth;
    GameObject goal;
    GameObject guiscript;

    // fitness variabelen
    public float factorTime;
    public float factorDamage;
    public float factorGateDamage;
    public float factorGateDistance;

    void Awake()
    {
        guiscript = GameObject.Find("GUIMain");
        goal = GameObject.Find("Goal");
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
        leeftijd += Time.deltaTime;
        fitness = getFitness();
        goalDistance = Vector3.Distance(goal.transform.position, this.transform.position);
    }

    public void generateenemyStats()
    {

        this.health = (int)Mathf.Round(healthDistributionFactor * totalStatPoints);
        if (this.health <= 0)
        {
            Debug.Log("health <= 0 is true");
            this.health = 1;
        }
        this.attack = (int)Mathf.Round(attackDistributionFactor * totalStatPoints);
        if (this.attack <= 0)
        {
            Debug.Log("attack <= 0 is true");
            this.attack = 1;
        }
        this.defense = (int)Mathf.Round(defenseDistributionFactor * totalStatPoints);
        if (this.defense == 0)
        {
            Debug.Log("'defense <= 0 is true");
            this.defense = 1;
        }
        this.magicDefense = (int) Mathf.Round(magicDefenseDistributionFactor * totalStatPoints);
        if (this.magicDefense == 0)
        {
            Debug.Log("magicDefense <= 0 is true");
            magicDefense = 1;
        }
        this.speedMultiplier = Random.Range(0.90f, 1.10f);
        this.dfactor = Random.Range(0.05f, 0.80f);
        this.goalImportance = Random.Range(0.6f, 1f);
        this.playerImportance = Random.Range(0, 0.3f);
    }

    public void generateDistribution()
    {
        // Als enemy nog geen distributie heeft
        if (statDistribution.Count == 0)
        {
            List<int> temp = randomNumberGenerator(4, 100);

            this.healthDistributionFactor = (float)temp[0] / 100;
            this.attackDistributionFactor = (float)temp[1] / 100;
            this.defenseDistributionFactor = (float)temp[2] / 100;
            this.magicDefenseDistributionFactor = (float)temp[3] / 100;

            statDistribution.Add(healthDistributionFactor);
            statDistribution.Add(attackDistributionFactor);
            statDistribution.Add(defenseDistributionFactor);
            statDistribution.Add(magicDefenseDistributionFactor);
        }
        else
        {
            this.healthDistributionFactor = statDistribution[0];
            this.attackDistributionFactor = statDistribution[1];
            this.defenseDistributionFactor = statDistribution[2];
            this.magicDefenseDistributionFactor = statDistribution[3];
        }
    }

    public float getFitness()
    {
        return factorTime * leeftijd + factorDamage * enemyResources.totalDamage + factorGateDamage * enemyResources.totalGateDamage  + factorGateDistance / goalDistance;
    }

    /// <summary>
    /// Genereert een lijst van n random numbers die opsommen tot sum.
    /// </summary>
    /// <param name="n"></param> Het aantal random numbers1
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

    public void mutate(float mutationProbability)
    {
        float randomFloat = Random.Range(0.0f, 1.0f);
        if (randomFloat <= mutationProbability)
        {
            mutateDistribution(4);
        }
    }

    /// <summary>
    /// Muteert distributiefactoren van enemy
    /// </summary>
    /// <param name="n"></param> Het aantal stats van een enemy
    public void mutateDistribution(int n)
    {
        List<int> statMutations = randomNumberGenerator(n, statMutation);

        // Tijdelijke oplossing...
        while ( statDistribution[0] * 100 + statMutations[0] - statMutation / n > 99.1 || statDistribution[0] * 100 + statMutations[0] - statMutation / n < 0.9 ||
                statDistribution[1] * 100 + statMutations[1] - statMutation / n > 99.1 || statDistribution[1] * 100 + statMutations[1] - statMutation / n < 0.9 ||
                statDistribution[2] * 100 + statMutations[2] - statMutation / n > 99.1 || statDistribution[2] * 100 + statMutations[2] - statMutation / n < 0.9 ||
                statDistribution[3] * 100 + statMutations[3] - statMutation / n > 99.1 || statDistribution[3] * 100 + statMutations[3] - statMutation / n < 0.9)
        {
            statMutations = randomNumberGenerator(n, statMutation);
        }

        for (int i = 0; i < statMutations.Count; i++)
        {
            statMutations[i] -= statMutation / n;
        }

        List<float> DistributionMutation = new List<float>();

        for (int i = 0; i < statMutations.Count; i++)
        {
            DistributionMutation.Add((float)statMutations[i] / 100);
            //Debug.Log(DistributionMutation[i]);
        }

        for (int i = 0; i < statDistribution.Count; i++)
        {
            statDistribution[i] += DistributionMutation[i];
            // Afronden op twee decimalen
            statDistribution[i] = Mathf.Round(statDistribution[i] * 100f) / 100f;
        }

    }

    public int calculateMax(List<int> seq)
    {
        int maxValue = int.MinValue;

        for (int i = 0; i < seq.Count; i++)
        {
            if (maxValue < seq[i])
            {
                maxValue = seq[i];
            }
        }
        return maxValue;
    }


    public float calculateMax(List<float> seq)
    {
        float maxValue = int.MinValue;

        for (int i = 0; i < seq.Count; i++)
        {
            if (maxValue < seq[i])
            {
                maxValue = seq[i];
            }
        }
        return maxValue;
    }

    public int calculateMin(List<int> seq)
    {
        int minValue = int.MaxValue;

        for (int i = 0; i < seq.Count; i++)
        {
            if (minValue > seq[i])
            {
                minValue = seq[i];
            }
        }
        return minValue;
    }

    public float calculateMin(List<float> seq)
    {
        float minValue = int.MaxValue;

        for (int i = 0; i < seq.Count; i++)
        {
            if (minValue > seq[i])
            {
                minValue = seq[i];
            }
        }
        return minValue;
    }

    public float getLowestFactor()
    {
        return calculateMin(statDistribution);
    }

    public float getHighestFactor()
    {
        return calculateMax(statDistribution);
    }

}



