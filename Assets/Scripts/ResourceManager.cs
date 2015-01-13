using UnityEngine;
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
	public float poisonAttack;
	public float poisonSpeed;
	public float poisonSpecial;
	public GameObject fireTrap;
	public float fireAttack;
	public float fireSpeed;
	public float fireSpecial;
	public GameObject iceTrap;
	public float iceAttack;
	public float iceSpeed;
	public float iceSpecial;
	public float speedReduceRate;
	public GameObject spearTrap;
	public GameObject barricade;
	public int barricadeHealth;

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
    public AudioClip headShot;
    public AudioClip bulletHit;

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
    
    [Header("Scores")]
    public int killsScore;
    public int guyantKillsScore;
    public int gwarfKillsScore;
    public int grobbleKillsScore;
    public int headshotsScore;
    public int killStreakScore;
    public int headShotStreakScore;
    public int playerKillsScore;
    public int towerKillsScore;


	public List<GameObject> allBarricades;

	void Start(){
		playerData.addGold (startGold);
	}


}
