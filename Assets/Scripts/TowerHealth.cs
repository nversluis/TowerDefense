using UnityEngine;
using System.Collections;

public class TowerHealth : MonoBehaviour {

    private int startingHealth = 2000;
    public int currentHealth;

    bool isDead = false;

    void Awake()
    {
        currentHealth = startingHealth;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        Destroy(this.gameObject);
    }
}
