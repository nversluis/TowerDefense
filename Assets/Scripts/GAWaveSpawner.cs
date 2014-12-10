using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAWaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies = 5;
    public float maxX;
    public float maxZ;
    //float orcHeigthSpawn = 3.27f;
    public bool spawning = true;

    public int maxWaves = 5;
    public int currentWave = 1;

    public ArrayList currentGen;
    public ArrayList nextGen;

    EnemyAttack enemyAttack;

    void Awake()
    {
        enemyAttack = GetComponent<EnemyAttack>();
    }

    // Use this for initialization
    void Start()
    {
        currentGen = new ArrayList();
        nextGen = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (spawning)
            {
                if (currentGen.Count < maxEnemies)
                {
                    // Spawn enemies tot het maximale aantal enemies wordt bereikt
                    SpawnEnemy();
                }
                else
                {
                    spawning = false;
                }
            }
            else
            {
                UpdateEnemyCount();

                if (currentGen.Count == 0)
                {
                    // Als alle enemies dood zijn, ga naar de volgende wave
                    currentWave++;
                    // Spawn een extra enemy voor de volgende wave
                    maxEnemies++;
                    // Enemies mogen weer gespawnd worden
                    spawning = true;
                }
            }
        }
        else
        {
            Debug.Log("Congratulations! You've succesfully defeated all waves of enemies!");
        }
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 0f, randZ), Quaternion.identity);
        Enemy.name = "enemy";
        currentGen.Add(Enemy);
        //Debug.Log(enemies.Count);
    }

    void UpdateEnemyCount()
    {
        for (int i = 0; i < currentGen.Count; i++)
        {
            if ((GameObject)(currentGen[i]) == null)
            {
                // Verwijder een enemy uit de lijst van enemies als die dood is
                currentGen.Remove(currentGen[i]);
            }
        }
    }

}