using UnityEngine;
using System.Collections;

public class PoisonTrap : MonoBehaviour {

	private int damagePerShot=100;
	private string enemyTag = ("Enemy");
	private GameObject enemy;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == (enemyTag)) {
			enemy = col.gameObject;
			EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
			enemyHealth.TakePoisonDamage (damagePerShot);
			}
		}


	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
