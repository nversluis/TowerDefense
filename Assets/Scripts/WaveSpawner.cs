using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies = 1;
    public float maxX;
    public float maxZ;
    float orcHeigthSpawn = 3.27f;
    public bool spawning = true;

    public int maxWaves = 5;
    public int currentWave = 1;

    public ArrayList enemies;


    // Use this for initialization
    void Start()
    {
        enemies = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentWave <= maxWaves)
        {
            if (spawning)
            {
                if (enemies.Count < maxEnemies)
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
                for (int i = 0; i < enemies.Count; i++)
                {
                    if ((GameObject)(enemies[i]) == null)
                    {
                        // Verwijder een enemy uit de lijst van enemies als die dood is
                        enemies.Remove(enemies[i]);
                    }
                }

                if (enemies.Count == 0)
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
        enemies.Add(Enemy);
        //Debug.Log(enemies.Count);
    }

}

