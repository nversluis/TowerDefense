using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public float SpawnRate = 1f;
    private float spawnRange = 10;
    public float maxX;
    public float maxZ;
    float orcHeigthSpawn = 3.27f;

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("Spawning", 0, 3);
        GameObject Enemy = (GameObject)Instantiate(enemy, transform.position+new Vector3(0f,orcHeigthSpawn,0f), Quaternion.identity);
        Enemy.name = "enemy";
        Enemy.transform.FindChild("Floor").transform.position = transform.position;
    }

    // Update is called once per frame
//    void FixedUpdate()
//    {
//        float rand=Random.value;
//        if (rand < 1/SpawnRate)
//        {
//            float randX = Random.Range(-maxX / 2, maxX / 2);
//            float randZ = Random.Range(-maxZ / 2, maxZ / 2);
//
//			GameObject Enemy = (GameObject)Instantiate(enemy, transform.position+new Vector3(0f,orcHeigthSpawn,0f), Quaternion.identity);
//            Enemy.name = "enemy";
//        }
//    }
//
    //void Spawning()
    //{
    //    if(SpawnRate>5)
    //    SpawnRate = SpawnRate -1;
    //}
    
}