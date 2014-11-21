using UnityEngine;
using System.Collections;

public class BidarraSpawner : MonoBehaviour {

    public GameObject biddaro;
    public float SpawnRate = 1f;
    private float spawnRange;

	// Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawning", 0, 3);

    }
	
	// Update is called once per frame

    void FixedUpdate()
    {
        float rand=Random.value;
        if (rand < 1/SpawnRate)
        {
            //GameObject Biddaro = (GameObject)Instantiate(biddaro, transform.position + new Vector3(Random.Range(-spawnRange / 2, spawnRange / 2) / (spawnRange + 0.1f * spawnRange), 0f, Random.Range(-spawnRange / 2, spawnRange / 2) / (spawnRange + 0.1f * spawnRange)), Quaternion.identity);
            //Biddaro.name = "biddaro";
        }

    }
    void Spawning()
    {
        if(SpawnRate>1)
        SpawnRate = SpawnRate -1;
    }
}
