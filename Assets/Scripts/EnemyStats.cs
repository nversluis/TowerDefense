using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour {

    private List<int> stats;
    private int totalStatPoints = 10000;
    private string type;

    // Stats van een enemy
    private int health;
    private int attack;
    private int defense;
    private int speed;

    // Damage output van een enemy
    private float DamageToBase;
    private float DamageToPlayer;
    private float DamageToTower;
    
    /// <summary>
    /// Genereert een lijst van n random numbers die opsommen tot sum.
    /// </summary>
    /// <param name="n"></param> Het aantal random numbers
    /// <param name="sum"></param> De som van de random numbers
    /// <returns></returns>
    public List<int> randomNumberGenerator(int n, int sum)
    {
        List<int> randomNumbers = new List<int>();

        // Kies n - 1 random getallen en voeg deze toe in de lijst
        for (int i = 0; i < n - 1; i++)
        {
            int randomInt = Random.Range(0, sum);
            randomNumbers.Add(randomInt);
        }

        // Voeg 0 en sum toe aan de lijst
        randomNumbers.Add(0);
        randomNumbers.Add(sum);

        // Sorteer de lijst
        randomNumbers.Sort();

        // Maak een nieuwe lijst aan
        List<int> result = new List<int>();

        for (int i = 0; i < n; i++)
        {
            // Voeg het vershil van de elementen toe aan de nieuwe lijst
            result.Add(randomNumbers[i + 1] - randomNumbers[i]);
        }

        // De lijst bevat nu n willekeurige getallen die opsommen tot sum.
        return result;
    }
    
    public void generateEnemyStats()
    {
        this.stats = randomNumberGenerator(6, totalStatPoints);
        this.health = stats[0];
        this.attack = stats[1];
        this.defense = stats[2];
        this.speed = stats[3];
    }

    public float fitness() {
        if (type.Equals("BaseAttacker"))
        {
            return DamageToBase;
        }
        else if (type.Equals("PlayerAttacker"))
        {
            return DamageToPlayer;
        }
        else if (type.Equals("TowerAttacker"))
        {
            return DamageToTower;
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// Muteert een enemy door x willekeurige stats te verhogen en n - x te verlagen
    /// </summary>
    /// <param name="n"></param> Het aantal stats van een enemy
    /// <param name="delta"></param> De toename van een stat
    public void mutate(int n, int delta)
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
            stats[verhoogIndices[i]] += delta;
        }

        // Bepaald hoeveel elk stat wordt verlaagd, de som van de statverlagingen is gelijk aan die van de statverhogingen
        List<int> statverlaging = randomNumberGenerator(n - aantal, aantal * delta);

        for (int i = 0; i < verlaagIndices.Count; i++)
        {
            // Verlaag de stats
            stats[verlaagIndices[i]] -= statverlaging[i];
        }
    }

    public void crossover()
    {

    }

    public float getHealth() {
        return health;
    }

    public float getAttack()
    {
        return attack;
    }

    public float getDefense()
    {
        return defense;
    }

    public float getSpeed() 
    {
        return speed;
    }

     void Awake()
     {
         generateEnemyStats();
     }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}



