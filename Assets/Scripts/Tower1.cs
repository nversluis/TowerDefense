using UnityEngine;
using System.Collections;

//Deze moeten toegevoegd worden aan de triggers van de towers
//Attach TowerScript op Prefab van de towers
//Geef deze triggers ook een IgnoreRaycast layer
public class Tower1 : MonoBehaviour {
	public string targetName;
	public GameObject Tower1Bullet;
	public int spawnTime; //Frames tussen twee bullets. Todo, veranderen met tower upgrads

	public static GameObject targetObject;

	private int timer;

	//Instantiate bullet at center of prefab if the target is in the trigger.
	void OnTriggerStay(Collider col){
		if (timer%spawnTime==0&&col.gameObject.name.Equals (targetName)) {
			targetObject=col.gameObject;
			Instantiate (Tower1Bullet,transform.position, transform.rotation);
				}
		}

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer++;
		timer=timer%spawnTime;
	}
}
