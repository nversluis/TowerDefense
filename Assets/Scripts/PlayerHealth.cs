using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private int startingHealth = 10000;
    public int currentHealth;
    public int defence = 10;

    bool isDead = false;
    bool damaged = false;

    void Awake()
    {
        currentHealth = startingHealth;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }

    internal void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount/defence;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        Destroy(this.gameObject);
    }

}
