using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonTrap : MonoBehaviour {
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private int damagePerShot=500;
	private string enemyTag = ("Enemy");
	private GameObject enemy;
	private List<GameObject> enemyOnTrap = new List<GameObject> ();
	private float particleStartSize;
	private GameObject partSys;
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == (enemyTag)&&!enemyOnTrap.Contains(col.gameObject)) {
			enemy = col.gameObject;
			EnemyHealth enemyHealth = enemy.collider.GetComponent<EnemyHealth> ();
			enemyHealth.TakePoisonDamage (damagePerShot);
			enemyOnTrap.Add (col.gameObject);
			}
		}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == (enemyTag)) {
			enemyOnTrap.Remove (col.gameObject);
		}
	}
	
	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
		partSys = gameObject.transform.GetChild (2).gameObject;
		particleStartSize=partSys.particleSystem.startSize*resourceManager.planewidth/5;

	}
	
	// Update is called once per frame
	void Update () {
		enemyOnTrap.RemoveAll (item => item == null);
		if (enemyOnTrap.Count == 0) {
			partSys.gameObject.particleSystem.startSize = particleStartSize/10;
		} else
			partSys.gameObject.particleSystem.startSize = particleStartSize*3;
	}
}
