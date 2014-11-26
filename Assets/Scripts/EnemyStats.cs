using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private List<int> stats;
<<<<<<< HEAD
=======
    private int totalStatPoints = 600;
    private string type;

    // Stats van een enemy
>>>>>>> Tiamur
    private int health;
    private int meleeAttack;
    private int rangeAttack;
    private int meleeDefense;
    private int rangeDefense;
    private int speed;
<<<<<<< HEAD
    private int totalPoints = 600;
    
    public List<int> randomNumberGenerator(int aantalStats)
    {
        List<int> randomNumbers = new List<int>();

        for (int i = 0; i < aantalStats - 1; i++)
        {
            int randomInt = Random.Range(0, totalPoints);
            randomNumbers.Add(randomInt);
        }

        randomNumbers.Add(0);
        randomNumbers.Add(totalPoints);

        randomNumbers.Sort();

        List<int> result = new List<int>();

        for (int i = 0; i < randomNumbers.Count - 1; i++)
        {
            result.Add(randomNumbers[i + 1] - randomNumbers[i]);
        }

        return randomNumbers;
=======

    // Damage output van een enemy
    private float DamageToBase;
    private float DamageToPlayer;
    private float DamageToTower;


    public Enemy()
    {
        generateEnemy();
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
>>>>>>> Tiamur
    }

    public void generateEnemy()
    {
<<<<<<< HEAD
        this.stats = randomNumberGenerator(6);
=======
        this.stats = randomNumberGenerator(6, totalStatPoints);
>>>>>>> Tiamur
        this.health = stats[0];
        this.meleeAttack = stats[1];
        this.rangeAttack = stats[2];
        this.meleeDefense = stats[3];
        this.rangeDefense = stats[4];
        this.speed = stats[5];
    }

<<<<<<< HEAD
    public Enemy()
    {
        generateEnemy();
    }

    public void fitness() {

    }

    public void mutate()
=======
    public float fitness() {
        if (type.Equals("BaseAttacker"))
        {
            calculateDamageOnTower();
            return DamageToBase;
        }
        else if (type.Equals("PlayerAttacker"))
        {
            calculateDamageToPlayer();
            return DamageToPlayer;
        }
        else if (type.Equals("TowerAttacker"))
        {
            calculateDamageOnTower();
            return DamageToTower;
        }
        else
        {
            return 0;
        }
    }

    void calculateDamageToBase()
    {

    }

    void calculateDamageToPlayer()
    {

    }

    void calculateDamageOnTower()
>>>>>>> Tiamur
    {

    }

<<<<<<< HEAD
=======
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
>>>>>>> Tiamur

    public float getHealth() {
        return health;
    }

     public float getMeleeAttack() {
        return meleeAttack;
    }

     public float getRangeAttack() {
        return rangeAttack;
    }

     public float getMeleeDefense() {
        return meleeDefense;
    }

     public float getRangeSpeed() {
        return rangeDefense;
    }

     public float getSpeed() {
        return speed;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}

public class baseAttacker : Enemy
{

    public void genereateBaseAttacker()
    {

    }

}

