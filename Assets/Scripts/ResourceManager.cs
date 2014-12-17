using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//use this to declare all variables
public class ResourceManager : MonoBehaviour {

	//prefabs
	[Header("Map Prefabs")]

	public GameObject planePrefab;
	public GameObject wallPrefab;
	public GameObject EnemySpawner;
	public GameObject Minimapcamera;
	public GameObject Gate;
	public GameObject torch;

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
    public bool drawPath;

	[Header("Waves")]
	public int maxWaves;
	public int currentWave;

	[Header("Towers")]
	public float maxTowerDistance;
	[Header("WallTowers")]

	public GameObject tower1;
	public float coolDownTimeTower1;
	public GameObject tower2;
	[Header("Floor")]
	public GameObject poisonTrap;
	public GameObject fireTrap;
	[Header("Hotspots")]
	public GameObject poisonTrapHotspot;
	public GameObject fireTrapHotspot;

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
	public GameObject gui;
	public float lengthMinimap;
	public float heightMinimap;

	[Header("Level Editor")]
	public GameObject editorPlane;
	public Color noPlane;
	public Color startOrEnd;
	public Color connected;
	public Color notConnected;
	public Color highlighted;


	[Header("MapGroottes")]
	public static int mostEast;
	public static int mostWest;
	public static int mostNorth;
	public static int mostSouth;



}
