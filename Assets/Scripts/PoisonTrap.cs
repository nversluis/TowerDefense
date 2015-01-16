using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonTrap : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private int damagePerShot;// = 500;
	private string enemyTag = ("Enemy");
	private GameObject enemy;
	private List<GameObject> enemyOnTrap = new List<GameObject> ();
	private float particleStartSize;
	private GameObject partSys;

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == (enemyTag) && !enemyOnTrap.Contains (col.gameObject)) {
			enemy = col.gameObject;
			EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
			enemyOnTrap.Add (col.gameObject);
			if (enemyOnTrap.Count == 1) {
				InvokeRepeating ("DoDamage", 0.1f, 1/gameObject.GetComponent<TowerStats>().speed);
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemyOnTrap.Remove (col.gameObject);
		}
	}

	int testint = 0;

	void DoDamage ()
	{

		partSys.particleSystem.startSize = particleStartSize * 2;
		foreach (GameObject enemy in enemyOnTrap) {
			if (enemy != null) {
				EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
				enemyHealth.TakePoisonDamage (damagePerShot);

			}
		} 
		StartCoroutine (ResizeParticles ());
	}

	IEnumerator ResizeParticles ()
	{
		yield return new WaitForSeconds (1);
		//Debug.Log (45);
		partSys.particleSystem.startSize = particleStartSize / 10;
	}

	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		partSys = gameObject.transform.GetChild (2).gameObject;
		particleStartSize = partSys.particleSystem.startSize * resourceManager.planewidth / 5;
		TowerStats stats = gameObject.GetComponent < TowerStats> ();
		damagePerShot = stats.attack;
        stats.speedUpgrade = GameObject.Find("TowerStats").GetComponent<TowerResources>().poisonSpeedUpgrade;
        stats.attackUpgrade = (GameObject.Find("TowerStats").GetComponent<TowerResources>().poisonAttackUpgrade- 1) * stats.attack;
	}
	
	// Update is called once per frame
	void Update ()
	{
		enemyOnTrap.RemoveAll (item => item == null);
		for (int i = 0; i < enemyOnTrap.Count; i++) {
            EnemyResources enemyResources = enemyOnTrap[i].collider.GetComponent<EnemyResources>();
            if (enemyResources.isDead)
            {
				enemyOnTrap.Remove(enemyOnTrap[i]);
			}
		}
		if (enemyOnTrap.Count == 0) {
			partSys.gameObject.particleSystem.startSize = particleStartSize / 50;
		}
	}
}
