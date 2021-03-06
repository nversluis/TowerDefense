﻿using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour
{

	Color transparentgreen = new Color (0, 255, 0, 0.1f);
	Color transparentred = new Color (255, 0, 0, 0.1f);
	private GameObject player;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;
	private int cost;
	private KeyInputManager inputManager;
	public bool hasEnemy;

	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = 0;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
		transform.GetChild (0).renderer.material.color = Color.white;
		hasEnemy = false;
		gameObject.layer = 10;
	}

	void Update ()
	{
		//Debug.Log (CameraController.hitObject.name);
		if (gameObject == CameraController.hitObject && gameObject.transform.parent.name.Contains("loor")) { //if the object you are looking at is the floor
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
				tower.transform.parent = gameObject.transform;	
				//tower.renderer.material.color =new Color(0,255,0,0.1f);
				//tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) {
					foreach (Material mat in child.materials) {
						mat.shader = Shader.Find ("Transparent/Diffuse");
						if (cost >= playerData.getGold () || hasEnemy) {
							mat.color = transparentred;
						} else {
							mat.color = transparentgreen;
						}
					}
				}

				if (TowerPrefab.name.Contains ("Fire")) {
                    cost = resourceManager.costFireTrap[ResourceManager.Difficulty];
				} else if (TowerPrefab.name.Contains ("Poison")) {
                    cost = resourceManager.costPoisonTrap[ResourceManager.Difficulty];
				} else if (TowerPrefab.name.Contains ("Ice")) {
                    cost = resourceManager.costIceTrap[ResourceManager.Difficulty];
				} else if (TowerPrefab.name.Contains ("Spear")) {
					//cost = resourceManager.costSpearTrap;
				} else if (TowerPrefab.name.Contains ("arricade")) {
                    cost = resourceManager.costBarricade[ResourceManager.Difficulty];
				}

				//tower.transform.GetChild (0).gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				//tower.transform.GetChild (0).gameObject.renderer.

			}
			//Sell the trap
			if (WeaponController.weapon == 50) {
				if (gameObject.transform.childCount == 2) { 
					GameObject baseOfTrap;
					try {
						baseOfTrap = gameObject.transform.GetChild (1).Find ("Base").gameObject;
						WallScript.DestroyHotSpots();
						GameObject resTower = (GameObject)Instantiate (baseOfTrap, baseOfTrap.transform.position, baseOfTrap.transform.rotation);
						resTower.gameObject.transform.localScale *= 1.001f / 2;
						resTower.name = "blueTower";
						resTower.gameObject.transform.parent = gameObject.transform;
						resTower.tag = "TowerHotSpot";
						Color col;
						if(cost*2<=playerData.getGold() && transform.GetChild (1).GetComponent<TowerStats> ().level<5){
							col = new Color(0,0,255,0.1f);
						} else
							col = new Color(255,0,0,0.1f);
						foreach (Renderer child in resTower.GetComponentsInChildren<Renderer>()) {
							child.material.color = col;
							child.material.shader = Shader.Find ("Transparent/Diffuse");
						}
					} catch {
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
			transform.GetChild(0).gameObject.layer = 16;
		} else {
			transform.GetChild(0).gameObject.layer = 9;
		}
	}

	private void sellTower ()
	{
		GameObject tower = gameObject.transform.GetChild (1).gameObject;
		TowerStats stats = tower.GetComponent<TowerStats> ();
		GUIScript.player.addGold (stats.sellCost);
		Destroy (gameObject.transform.GetChild (1).gameObject);
		WallScript.DestroyHotSpots();
	}

	private void upgradeTower ()
	{
		GameObject tower = gameObject.transform.GetChild (1).gameObject;
		TowerStats stats = tower.GetComponent<TowerStats> ();
		if (stats.level < 5) {
			if (stats.upgradeCost <= GUIScript.player.getGold ()) {
				stats.level++;
				if (tower.name.Contains ("Fire")) {
					stats.attack = (int)Mathf.Round (stats.attack * GameObject.Find ("TowerStats").GetComponent<TowerResources> ().fireAttackUpgrade);
					stats.speed += GameObject.Find ("TowerStats").GetComponent<TowerResources> ().fireSpeed;
					stats.attackUpgrade = (int)(stats.attack * (GameObject.Find ("TowerStats").GetComponent<TowerResources> ().fireAttackUpgrade - 1));
					stats.speedUpgrade = GameObject.Find ("TowerStats").GetComponent<TowerResources> ().fireSpeed;
					GUIScript.player.addGold (-stats.upgradeCost);
				} else if (tower.name.Contains ("Ice")) {
					stats.attack = (int)Mathf.Round (stats.attack * GameObject.Find ("TowerStats").GetComponent<TowerResources> ().iceAttackUpgrade);
					stats.speed += GameObject.Find ("TowerStats").GetComponent<TowerResources> ().iceSpeed;
					stats.specialDamage += GameObject.Find ("TowerStats").GetComponent<TowerResources> ().iceSpecialDamage;
					stats.attackUpgrade = (int)(stats.attack * (GameObject.Find ("TowerStats").GetComponent<TowerResources> ().iceAttackUpgrade - 1));
					stats.speedUpgrade = GameObject.Find ("TowerStats").GetComponent<TowerResources> ().iceSpeed;
					GUIScript.player.addGold (-stats.upgradeCost);
				} else if (tower.name.Contains ("Poison")) {
					stats.attack = (int)Mathf.Round (stats.attack * GameObject.Find ("TowerStats").GetComponent<TowerResources> ().poisonAttackUpgrade);
					stats.speed += GameObject.Find ("TowerStats").GetComponent<TowerResources> ().poisonSpeed;
					stats.attackUpgrade = (int)(stats.attack * (GameObject.Find ("TowerStats").GetComponent<TowerResources> ().poisonAttackUpgrade - 1));
					stats.speedUpgrade = GameObject.Find ("TowerStats").GetComponent<TowerResources> ().poisonSpeed;
					GUIScript.player.addGold (-stats.upgradeCost);
				} else if (tower.name.Contains ("Spear")) {
					//cost = resourceManager.costSpearTrap;
				} else if (tower.name.Contains ("arricade")) {
                    cost = (int)resourceManager.costBarricade[ResourceManager.Difficulty];
				}
				stats.sellCost += stats.upgradeCost / 2;
				stats.upgradeCost *= 2;

			} else {
				GameObject.Find ("GUIMain").GetComponent<GUIScript> ().Notification ("NoGold");
			}
		}

		WallScript.DestroyHotSpots();
	}

}

