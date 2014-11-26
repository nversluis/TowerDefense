using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BidarraSpawner : MonoBehaviour {

    public GameObject bidarro;
    public float SpawnRate = 1f;
    private float spawnRange = 10;
    public float maxX;
    public float maxZ;

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
            float randX = Random.Range(-maxX / 2, maxX / 2);
            float randZ = Random.Range(-maxZ / 2, maxZ / 2);
            
            GameObject Bidarro = (GameObject)Instantiate(bidarro, transform.position + new Vector3(randX, 0f, randZ), Quaternion.identity);
            Bidarro.name = "bidarro";
        }
    }

    void Spawning()
    {
        if(SpawnRate>1)
        SpawnRate = SpawnRate -1;
    }
}
