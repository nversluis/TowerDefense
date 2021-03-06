﻿using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour
{
	//Initizialising
	private GameObject realTrap;
	private GameObject redTrap;
	float planeW;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private int cost;
	private PlayerData playerData = GUIScript.player;
	Color transparentgreen = new Color(0, 255, 0, 0.1f); //Color of the green prefab
	Color transparentred = new Color(255, 0, 0, 0.1f); //Color of the red prefab
	// Use this for initialization

	void Start ()
	{

		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
		cost = 9000;
		if (gameObject.name.Contains ("Fire")) {
			realTrap = resourceManager.fireTrap;
            cost = resourceManager.costFireTrap[ResourceManager.Difficulty];
		} else if (gameObject.name.Contains ("Poison")) {
			realTrap = resourceManager.poisonTrap;
            cost = resourceManager.costPoisonTrap[ResourceManager.Difficulty];
		} else if (gameObject.name.Contains ("Ice")) {
			realTrap = resourceManager.iceTrap;
            cost = resourceManager.costIceTrap[ResourceManager.Difficulty];
		} else if (gameObject.name.Contains ("Spear")) {
			realTrap = resourceManager.spearTrap;
			//cost = resourceManager.costFireTrap;
		} else if (gameObject.name.Contains ("arricade")) {
			realTrap = resourceManager.barricade;
            cost = resourceManager.costBarricade[ResourceManager.Difficulty];
			//cost = resourceManager.costFireTrap;
		} 
	}

	public void BuildTrap ()
	{
		if (cost <= playerData.getGold ()) {
			if (!transform.parent.gameObject.GetComponent<FloorScript> ().hasEnemy) {
				GameObject trap = (GameObject)Instantiate (realTrap, transform.position, Quaternion.identity);//Instantiantion of the tower
				trap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW / 20;
				trap.transform.parent = gameObject.transform.parent;
				float randHoek = 90 * Mathf.Floor (Random.value * 4);
				trap.transform.RotateAround (transform.position, Vector3.up, randHoek);
				trap.tag = "Tower";
				trap.SetActive (true); 
				playerData.addGold (-cost);
				TowerStats stats = trap.GetComponent<TowerStats> ();
				stats.upgradeCost = cost;
				stats.sellCost = cost / 2;

				if (gameObject.name.Contains ("Fire")) {
					Statistics.fireTrapsBuilt = Statistics.fireTrapsBuilt + 1;
				} else if (gameObject.name.Contains ("Poison")) {
					Statistics.poisonTrapsBuilt = Statistics.poisonTrapsBuilt + 1;
				} else if (gameObject.name.Contains ("Ice")) {
					Statistics.iceTrapsBuilt = Statistics.iceTrapsBuilt + 1;
				} else if (gameObject.name.Contains ("arricade")) {
					//Statistics.barricadesBuilt [0]++;
				} 

				WallScript.DestroyHotSpots ();
			}
		} else {
			GameObject.Find("GUIMain").GetComponent<GUIScript>().Notification ("NoGold");
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			BuildTrap ();
		}
		if (cost > playerData.getGold () || transform.parent.gameObject.GetComponent<FloorScript>().hasEnemy) {
			foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {
				foreach (Material mat in child.materials) {
					mat.shader = Shader.Find ("Transparent/Diffuse");
					mat.color = transparentred;
				}
			} 

		} else 
			foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {
				foreach (Material mat in child.materials) {
					mat.shader = Shader.Find ("Transparent/Diffuse");
					mat.color = transparentgreen;
				}
			}
	}



}
