using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceTrap : MonoBehaviour {
	private GameObject ResourceManagerObj;
	private int damagePerShot = 100;
	private string enemyTag = ("Enemy");
	private float particleStartSize;
	private GameObject partSys;
	//private GameObject enemy;
	private List<GameObject> enemyOnTrap = new List<GameObject> ();

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)&&!enemyOnTrap.Contains(col.gameObject)) {
			if(gameObject.transform.GetChild (2).gameObject.activeSelf==false)
				gameObject.transform.GetChild (2).gameObject.SetActive (true);
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
				enemyHealth.TakeDamage (damagePerShot,"magic");

			}
		} 

	}

	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		ResourceManager resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
		partSys = gameObject.transform.GetChild (2).gameObject;
		particleStartSize=partSys.particleSystem.startSize*resourceManager.planewidth/10;
	}

	// Update is called once per frame
	void Update ()
	{
		enemyOnTrap.RemoveAll (item => item == null);
		if (enemyOnTrap.Count == 0) {
			CancelInvoke ();
			partSys.particleSystem.startSize = particleStartSize/10;
		} else
			partSys.particleSystem.startSize = particleStartSize*2;
	}
}
