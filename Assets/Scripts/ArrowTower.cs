using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowTower : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private string enemyTag;
	private GameObject arrow;
	private List<GameObject> enemyOnTrap;
	public int amountOfArrows;
	TowerStats stats;
    AudioSource audiosource;
    AudioClip[] arrows;

	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		enemyTag = resourceManager.enemyTag;
		arrow = resourceManager.arrowTowerArrow;
        arrows = resourceManager.arrows;
		enemyOnTrap=new List<GameObject>();
		amountOfArrows = 20;
		stats = transform.parent.gameObject.GetComponent<TowerStats> ();
        stats.sellCost = resourceManager.costMagicTower[ResourceManager.Difficulty] / 2;
        stats.upgradeCost = resourceManager.costMagicTower[ResourceManager.Difficulty];
		stats.attackUpgrade = stats.attack / 2;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == enemyTag &&!enemyOnTrap.Contains(col.gameObject)) {
			enemyOnTrap.Add (col.gameObject);
			if (enemyOnTrap.Count == 1) {
				InvokeRepeating ("Shooting", 0, 1/stats.speed);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == enemyTag)
		{
			enemyOnTrap.Remove(col.gameObject);
		}
	}

	private void Shooting()
	{
		for (int i = 0; i < amountOfArrows; i++) {
			Invoke ("Shootbullet", i*0.02f);
		}
	}

	private void Shootbullet()
	{
		//Vector3 positionOffset = new Vector3(
		GameObject bullet = (GameObject)Instantiate (arrow, transform.parent.position, Quaternion.identity);
		bullet.transform.parent = gameObject.transform.parent;
		bullet.transform.localPosition = new Vector3 (0, 3.8f - Mathf.Floor (Random.value * 8) * 1.075f, 3.775f - Mathf.Floor (Random.value * 8) * 1.075f);
		bullet.transform.up = -transform.right;
		bullet.GetComponent<Arrow> ().damagePerShot = stats.attack;
		bullet.rigidbody.velocity = -transform.right*100*Random.Range(0.3f,1f);
        int i = Random.Range(0,4);
        audiosource = bullet.GetComponent<AudioSource>();
        audiosource.PlayOneShot(arrows[i]);
	}




	// Update is called once per frame
	void Update () {
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
		} 
	}
}
