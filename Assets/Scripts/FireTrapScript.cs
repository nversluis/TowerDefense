using UnityEngine;
using System.Collections;

public class FireTrapScript : MonoBehaviour
{

	private int damagePerShot = 10;
	private string enemyTag = ("Enemy");
	private GameObject enemy;

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemy = col.gameObject;
			InvokeRepeating ("DoDamage", 0.1f, 1);
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject==enemy) {
			CancelInvoke();
		}
	}

	void DoDamage ()
	{
		EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
		enemyHealth.TakeDamage (damagePerShot);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
