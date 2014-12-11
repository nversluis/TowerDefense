using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireTrapScript : MonoBehaviour
{

	private int damagePerShot = 50;
	private string enemyTag = ("Enemy");
	//private GameObject enemy;
	private List<GameObject> enemyOnTrap = new List<GameObject> ();

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemyOnTrap.Add (col.gameObject);
			if (enemyOnTrap.Count ==1)
				InvokeRepeating ("DoDamage", 0.1f, 0.33f);
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemyOnTrap.Remove (col.gameObject);
		}
	}

	void DoDamage ()
	{
		foreach (GameObject enemy in enemyOnTrap) {
			if (enemy != null) {
				EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
				enemyHealth.TakeDamage (damagePerShot);

			}
		} 
		
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (enemyOnTrap.Count == 0)
			CancelInvoke ();
	}
}
