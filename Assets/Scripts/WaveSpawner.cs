using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    private int maxEnemies = 2;
    private int aantalEnemies;
    public float SpawnRate = 1f;
    //private float spawnRange = 10;
    public float maxX;
    public float maxZ;
    float orcHeigthSpawn = 3.27f;
    public bool spawning = true;
    public bool spawningWave = false;

    public int maxWaves = 5;
    public int currentWave = 0;

    public ArrayList enemies;


    // Use this for initialization
    void Start()
    {
        //aantalEnemies = 0;
        //InvokeRepeating("Spawning", 0, 3);
        //GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(0f, orcHeigthSpawn, 0f), Quaternion.identity);
        //Enemy.name = "enemy";
        enemies = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float rand = Random.value;

        if (spawning)
        {
            /*while (enemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if ((GameObject)(enemies[i]) == null)
                {
                    enemies.Remove(enemies[i]);
                }
            }*/

            if (currentWave <= maxWaves)
            {
                if (spawningWave)
                {
                    if (enemies.Count < maxEnemies)
                    {
                        SpawnEnemy();
                    }
                }
                if (enemies.Count == 0)
                {
                    spawningWave = true;
                    currentWave++;
                }
                if (aantalEnemies == maxEnemies)
                {
                    spawningWave = false;
                }
            }

            /*if (spawning)
            {
                if (rand < 1 / SpawnRate && enemies.Count < maxEnemies)
                {
                    SpawnEnemy();
                }
                else
                {
                    spawning = false;
                }


                if (currentWave < maxWaves)
                {
                    if (spawningWave)
                    {
                        SpawnEnemy();
                    }
                    if (aantalEnemies == 0)
                    {
                        spawningWave = true;
                        currentWave++;
                    }
                    if (aantalEnemies == maxEnemies)
                    {
                        spawningWave = false;
                    }
                }
            }*/
        }
    }

    void SpawnEnemy()
    {
        float randX = Random.Range(-maxX / 2, maxX / 2);
        float randZ = Random.Range(-maxZ / 2, maxZ / 2);

        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position + new Vector3(randX, 0f, randZ), Quaternion.identity);
        Enemy.name = "enemy";
        aantalEnemies++;
        enemies.Add(Enemy);
        //Debug.Log(enemies.Count);
    }

}

