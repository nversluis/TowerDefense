using UnityEngine;
using System.Collections;

public class ArrowTower : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private string enemyTag;
	private GameObject arrow;
	private bool isShooting;

	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		enemyTag = resourceManager.enemyTag;
		arrow = resourceManager.arrowTowerArrow;
		isShooting = false;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == enemyTag && !isShooting) {
			InvokeRepeating ("Shootbullet", 0, 0.5f);
			isShooting = true;

		}
	}

	private void Shootbullet()
	{
		GameObject bullet = (GameObject)Instantiate (arrow, transform.parent.position, Quaternion.identity);
		bullet.transform.up = -transform.right;
		bullet.rigidbody.velocity = -transform.right*20;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
