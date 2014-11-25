using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private List<int> stats;
    private int health;
    private int meleeAttack;
    private int rangeAttack;
    private int meleeDefense;
    private int rangeDefense;
    private int speed;
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
    }

    public void generateEnemy()
    {
        this.stats = randomNumberGenerator(6);
        this.health = stats[0];
        this.meleeAttack = stats[1];
        this.rangeAttack = stats[2];
        this.meleeDefense = stats[3];
        this.rangeDefense = stats[4];
        this.speed = stats[5];
    }

    public Enemy()
    {
        generateEnemy();
    }

    public void fitness() {

    }

    public void mutate()
    {

    }


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

