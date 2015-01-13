using UnityEngine;
using System.Collections;

public class GuyantAttack : MonoBehaviour
{
	private float timeBetweenAttacks = 2.5f / 3f;
	public int attackDamage;
	public float playerDistance;
	public int damageMultiplier = 20;

	public int totalDamage;

	public GameObject player;
	PlayerHealth playerHealth;
	EnemyResources enemyResources;
	EnemyStats enemyStats;
	bool playerInRange;
	public bool invoked = false;

	void Start ()
	{
		totalDamage = 0;

		player = GameObject.Find ("Player");
		playerHealth = player.GetComponent<PlayerHealth> ();

		enemyStats = GetComponent<EnemyStats> ();
		enemyResources = GetComponent<EnemyResources> ();
		attackDamage = enemyStats.attack * damageMultiplier;

	}

	void Attack ()
	{
		GameObject barricade = enemyResources.targetBarricade;
		if (barricade != null && (barricade.transform.position - transform.position).magnitude < 5f) {
			barricade.GetComponent<barricade> ().TakeDamage (attackDamage);
		}
		else if (playerHealth.currentHealth > 0 && (player.transform.position - transform.position).magnitude < 3f) {
			playerHealth.TakeDamage (attackDamage);
		} 
		enemyResources.totalDamage += attackDamage;
	}

	void Update ()
	{


		if (enemyResources.attacking) {
			if (!invoked) {
				InvokeRepeating ("Attack", timeBetweenAttacks / 1.8f, timeBetweenAttacks);
				invoked = true;
			}
		} else {
			CancelInvoke ("Attack");
			invoked = false;
            
		}
	}
}
