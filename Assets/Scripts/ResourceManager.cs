﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//use this to declare all variables
public class ResourceManager : MonoBehaviour {
	private PlayerData playerData = GUIScript.player;

	//prefabs
	[Header("Costs")]
	public int startGold;
	public int costMagicTower;
	public int costArrowTower;
	public int costIceTrap;
	public int costFireTrap;
	public int costPoisonTrap;
	public int costBarricade;
	public int rewardenemy;
	public int rewardWave;


	[Header("Map Prefabs")]

	public GameObject planePrefab;
	public GameObject wallPrefab;
	public GameObject enemySpawner;
	public GameObject Minimapcamera;

	[Header("Decorations")]
	public GameObject Gate;
	public GameObject torch;
	public GameObject Goal;

	//float&ints map generate
	[Header("Map Generate")]
	public int length;	//Length of the map
	public int width;	//With of the map
	public float planewidth;//Size of the planes
	public float height;
	public float nodeSize;
	public int NumberOfPaths;
    public bool drawNavigationGrid;

	[Header("Enemies")]
	public string enemyTag;
	public GameObject enemyGuyant;
	public GameObject enemyGwarf;
	public GameObject enemyGrobble;
	public List<WayPoint> Nodes;
    public float walkSpeed;
    public float pathUpdateRate;
    public bool drawPath;
    public bool automaticPathUpdating;

	[Header("Waves")]
    public int currentWave;
	public int maxWaves;

	[Header("Towers")]
	public float maxTowerDistance;
	[Header("WallTowers")]

	public GameObject magicTower;
    public GameObject magicTowerBullet;
	public float coolDownTimeMagicTower;
	public GameObject arrowTower;
	public GameObject arrowTowerArrow;

	[Header("Floor")]
	public GameObject poisonTrap;
	public GameObject fireTrap;
	public GameObject iceTrap;
	public float speedReduceRate;
	public GameObject spearTrap;
	public GameObject barricade;
	[Header("Hotspots")]
	public GameObject magicTowerHotSpot;
	public GameObject arrowTowerHotSpot;
	public GameObject poisonTrapHotspot;
	public GameObject fireTrapHotspot;
	public GameObject iceTrapHotspot;
	public GameObject spearTrapHotspot;

	[Header("Spawners")]
	public float camOffset;
	public GameObject mainCamera;
	public GameObject player;
	public Vector2 startPos;
	public Vector2 endPos;

	[Header("Bullets")]
	public GameObject magicBullet;
	public int bulletSpeed;

	[Header("Audio")]
	public AudioClip magicBulletSound;
	public AudioClip backgroundMusic;

	[Header("HitSplash")]
	public GameObject damageText;

	[Header("UI")]
	public GameObject GUI;
	public GameObject eventListener;
	public float lengthMinimap;
	public float heightMinimap;

	[Header("Level Editor")]
	public GameObject editorPlane;
	public Color noPlane;
	public Color start;
    public Color end;
	public Color connected;
	public Color notConnected;
	public Color highlighted;


	[Header("MapGroottes")]
	public static int mostEast;
	public static int mostWest;
	public static int mostNorth;
	public static int mostSouth;

	void Start(){
		playerData.addGold (startGold);
	}


}
