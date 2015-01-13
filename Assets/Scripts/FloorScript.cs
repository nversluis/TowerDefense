using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour
{

	Color transparentgreen = new Color (0, 255, 0, 0.1f);
	Color transparentred = new Color (255, 0, 0, 0.1f);
	private int i;
	private GameObject ResourceManagerObj;
	private GameObject player;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;
	private int cost;
	private KeyInputManager inputManager;

	void Start ()
	{
		i = 0;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = 0;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
		transform.GetChild (0).renderer.material.color = Color.white;
	}

	void Update ()
	{
		//Debug.Log (CameraController.hitObject.name);
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the floor
			if (gameObject.transform.childCount == 1) {
				WallScript.DestroyHotSpots ();//Destroy all hotspots
			}
			
			GameObject TowerPrefab = WeaponController.curFloorTower;
			if (TowerPrefab != null && gameObject.transform.childCount == 1) { 
				float planeW = resourceManager.planewidth;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position + new Vector3 (0, planeW / 1000, 0), transform.rotation);
				//tower.gameObject.transform.Rotate(270, 0, 0);

				tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW * 5;
				if (TowerPrefab.name.Contains ("arricade")) {
					tower.gameObject.transform.localScale /= 100 / 1.7f;
					tower.tag = "TowerHotSpot";
				}

				float randHoek = 90 * Mathf.Floor (Random.value * 4);
				tower.transform.RotateAround (transform.position, Vector3.up, randHoek);

				//tower.renderer.material.color =new Color(0,255,0,0.1f);
				//tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) {
					foreach (Material mat in child.materials) {
						mat.shader = Shader.Find ("Transparent/Diffuse");
						if (cost >= playerData.getGold ()) {
							mat.color = transparentred;
						} else {
							mat.color = transparentgreen;
						}
					}
				}

				if (TowerPrefab.name.Contains ("Fire")) {
					cost = resourceManager.costFireTrap;
				} else if (TowerPrefab.name.Contains ("Poison")) {
					cost = resourceManager.costPoisonTrap;
				} else if (TowerPrefab.name.Contains ("Ice")) {
					cost = resourceManager.costIceTrap;
				} else if (TowerPrefab.name.Contains ("Spear")) {
					//cost = resourceManager.costSpearTrap;
				} else if (TowerPrefab.name.Contains ("arricade")) {
					cost = resourceManager.costBarricade;
				}

				//tower.transform.GetChild (0).gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				//tower.transform.GetChild (0).gameObject.renderer.
				tower.transform.parent = gameObject.transform;		
			}
			//Sell the trap
			if (WeaponController.weapon == 50) {
				if (gameObject.transform.childCount == 2) { 
					GameObject baseOfTrap;
					try {
						baseOfTrap = gameObject.transform.GetChild (1).Find ("Base").gameObject;
					
						GameObject resTower = (GameObject)Instantiate (baseOfTrap, baseOfTrap.transform.position, baseOfTrap.transform.rotation);
						resTower.gameObject.transform.localScale *= 1.001f / 2;
						resTower.name = "blueTower";
						resTower.gameObject.transform.parent = gameObject.transform;
						resTower.tag = "TowerHotSpot";
						Color col;
						if(cost*2<=playerData.getGold()){
							col = new Color(0,0,255,0.1f);
						} else
							col = new Color(255,0,0,0.1f);
						foreach (Renderer child in resTower.GetComponentsInChildren<Renderer>()) {
							child.material.color = col;
							child.material.shader = Shader.Find ("Transparent/Diffuse");
						}
					} catch (System.Exception e) {
					}
				}
				if (gameObject.transform.childCount == 3) { 
					if (Input.GetKeyDown (inputManager.upgradeInput)) {
						upgradeTower ();
					}
					//upgrade the tower - todo
					if (Input.GetKeyDown (inputManager.sellInput)) {
						sellTower ();
					}
				}

			}
		}	
		//change the layer of the minimapplane of this floor if the player is too far, so it wont show on the minimap
		if (player == null) {
			player = GameObject.Find ("Player");
		}
		else if ((player.transform.position - transform.position).magnitude >= 70) {
			transform.GetChild(0).gameObject.layer = 0;
		} else {
			transform.GetChild(0).gameObject.layer = 9;
		}
	}

	private void sellTower ()
	{
		Destroy (gameObject.transform.GetChild (1).gameObject);
		GUIScript.player.addGold (cost / 2);
		WallScript.DestroyHotSpots();
	}

	private void upgradeTower ()
	{
		GameObject tower = gameObject.transform.GetChild (1).gameObject;
		TowerStats stats = tower.GetComponent<TowerStats> ();
		if (cost * 2 <= GUIScript.player.getGold ()) {
			stats.level++;
			cost *= 2;
			if (tower.name.Contains ("Fire")) {
				stats.attack = (int)Mathf.Round (stats.attack * resourceManager.fireAttack);
				stats.speed = (int)Mathf.Round (stats.speed * resourceManager.fireSpeed);
				stats.specialDamage *= resourceManager.fireSpecial;
				GUIScript.player.addGold (-cost);
			} else if (tower.name.Contains ("Poison")) {
				stats.attack = (int)Mathf.Round (stats.attack * resourceManager.poisonAttack);
				stats.speed = (int)Mathf.Round (stats.speed * resourceManager.poisonSpeed);
				stats.specialDamage *= resourceManager.poisonSpecial;
				GUIScript.player.addGold (-cost);
			} else if (tower.name.Contains ("Ice")) {
				stats.attack = (int)Mathf.Round (stats.attack * resourceManager.iceAttack);
				stats.speed = (int)Mathf.Round (stats.speed * resourceManager.iceSpeed);
				stats.specialDamage *= resourceManager.iceSpecial;
				GUIScript.player.addGold (-cost);
			} else if (tower.name.Contains ("Spear")) {
				//cost = resourceManager.costSpearTrap;
			} else if (tower.name.Contains ("arricade")) {
				cost = (int)resourceManager.costBarricade;
			}
		} else {
			Debug.Log ("no moneyzz");
		}

		WallScript.DestroyHotSpots();
	}

}

