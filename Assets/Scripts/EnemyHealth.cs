using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;

    bool isDead;

    void Awake()
    {
<<<<<<< HEAD
=======
        enemyStats = GetComponent<EnemyStats>();
    }

    void Start()
    {
        startingHealth = enemyStats.health;
        defense = enemyStats.defense;
>>>>>>> c1dd732e0360c23fe7b90f070df2ed0988fd4266
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;

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
	
	// Update is called once per frame
	void Update () {
	
	}
}
