using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceTrap : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private int damagePerShot;
	private string enemyTag = ("Enemy");
	private float particleStartSize;
	private GameObject partSys;
	//private GameObject enemy;

	private List<GameObject> enemyOnTrap = new List<GameObject> ();

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == (enemyTag) && !enemyOnTrap.Contains (col.gameObject)) {
            EnemyResources enemyResources = col.gameObject.collider.GetComponent<EnemyResources>();
			enemyResources.isSlowed = gameObject.GetComponent<TowerStats> ().specialDamage;
			if (gameObject.transform.GetChild (2).gameObject.activeSelf == false)
				gameObject.transform.GetChild (2).gameObject.SetActive (true);
			enemyOnTrap.Add (col.gameObject);
			if (enemyOnTrap.Count == 1)
				InvokeRepeating ("DoDamage", 0.1f, 1/gameObject.GetComponent<TowerStats>().speed);
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemyOnTrap.Remove (col.gameObject);
            EnemyResources enemyResources = col.gameObject.collider.GetComponent<EnemyResources>();
            enemyResources.isSlowed = 1;
		}
	}



	void DoDamage ()
	{
		partSys.particleSystem.startSize = particleStartSize * 2;
		foreach (GameObject enemy in enemyOnTrap) {
			if (enemy != null) {
                EnemyResources enemyResources = enemy.collider.GetComponent<EnemyResources>();
				EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
				enemyHealth.TakeDamage (damagePerShot, "magic", false);
				enemyResources.isSlowed = gameObject.GetComponent<TowerStats> ().specialDamage;
			}
		}

		StartCoroutine (ResizeParticles ());

	}

	IEnumerator ResizeParticles ()
	{
		yield return new WaitForSeconds (1);
		partSys.particleSystem.startSize = particleStartSize / 10;
	}


	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		ResourceManager resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		partSys = gameObject.transform.GetChild (2).gameObject;
		particleStartSize = partSys.particleSystem.startSize * resourceManager.planewidth / 10;
		damagePerShot = gameObject.GetComponent<TowerStats> ().attack;
        gameObject.GetComponent<TowerStats>().speedUpgrade = GameObject.Find("TowerStats").GetComponent<TowerResources>().iceSpeedUpgrade;
        gameObject.GetComponent<TowerStats>().attackUpgrade = GameObject.Find("TowerStats").GetComponent<TowerResources>().iceAttackUpgrade;
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
			CancelInvoke ();
			partSys.particleSystem.startSize = particleStartSize / 10;
		} else {
		}
		//partSys.particleSystem.startSize = particleStartSize * 2;
	}
}
