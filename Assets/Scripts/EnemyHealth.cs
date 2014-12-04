using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth;
    public int currentHealth;
    public int defense;
    public EnemyStats enemyStats;

    bool isDead;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        startingHealth = enemyStats.health;
        currentHealth = startingHealth;
        defense = enemyStats.defense;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;
        currentHealth -= amount/defense;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
