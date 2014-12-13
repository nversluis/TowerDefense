using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{

	private float planewidth;
	//Size of the planes
	private float height;
	private float nodeSize;
	//Distance between nodepoints

	private GameObject editorPlane;
	private GameObject planePrefab;
	//Floor prefab
	private GameObject wallPrefab;
	//Wall prefab
	private GameObject EnemySpawner;
	private GameObject Minimapcamera;
	private GameObject Gate;
	private GameObject torch;
	public Camera cam;

	public static ArrayList positions = new ArrayList ();
	//Positions of the floors
	private List<Vector3> NodesPos = new List<Vector3> ();
	//Positions of the waypoints/nodes
	private List<WayPoint> Nodes = new List<WayPoint> ();
	//List with all Nodes
	private int length;
	private int width;
	private List<GameObject> floors = new List<GameObject> ();
	private Vector2 startPos;
	private Vector2 endPos;
	private bool playing;


	public InputField lengthInput;
	public InputField widthInput;
	public Button SubmitButton;
	public Button GenerateLevelButton;
	public Button selectStart;
	public Button selectEnd;
	public Button selectNormal;
	public GameObject canvas;



	public static int type;
	public static int amountOfEnds;
	public static int amountOfStarts;
	public static Vector3 endPos3;
	public static Vector3 startPos3;
	public static List<Vector3> posConnected;
	public static List<GameObject> allPos;
	public static GameObject startPlane;
	public static GameObject endPlane;


	// Use this for initialization
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;

	void Start ()
	{
		playing = false;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		torch = resourceManager.torch;
		planewidth = resourceManager.planewidth;
		height = resourceManager.height;
		nodeSize = resourceManager.nodeSize;
		editorPlane = resourceManager.editorPlane;
		planePrefab = resourceManager.planePrefab;
		wallPrefab = resourceManager.wallPrefab;
		EnemySpawner = resourceManager.EnemySpawner;
		Minimapcamera = resourceManager.Minimapcamera;
		Gate = resourceManager.Gate;
		torch = resourceManager.torch;
		posConnected = new List<Vector3> ();
		allPos = new List<GameObject> ();

		SubmitButton.onClick.AddListener (delegate {
			submitSize ();
		});
		GenerateLevelButton.onClick.AddListener (delegate {
			GenerateLevel ();
		});
		selectStart.onClick.AddListener (delegate {
			SwitchType (0);
		});
		selectEnd.onClick.AddListener (delegate {
			SwitchType (1);
		});
		selectNormal.onClick.AddListener (delegate {
			SwitchType (2);
		});
		amountOfEnds = 0;
		amountOfStarts = 0;
	}

	// Update is called once per frame
	void Update ()
	{

		if (playing) {
			int length1;
			int width1;
			bool resultLength = int.TryParse (lengthInput.text, out length1);
			bool resultWidth = int.TryParse (widthInput.text, out width1);
			if (resultWidth && resultLength && int.Parse (lengthInput.text) > 0 && int.Parse (widthInput.text) > 0) {
				SubmitButton.GetComponentInChildren<Text> ().text = "Confirm/Reset";
				SubmitButton.enabled = true;
			} else {
				SubmitButton.enabled = false;
				SubmitButton.GetComponentInChildren<Text> ().text = "Positive Integers Only";
			}
		}

	}

	private void submitSize ()
	{

		length = int.Parse (lengthInput.text);
		width = int.Parse (widthInput.text);
		resourceManager.length = length;
		resourceManager.width = width;
		if (length < 1 || width < 1) {
			SubmitButton.gameObject.transform.renderer.material.color = Color.red;
		} else {

			SubmitButton.GetComponentInChildren<Text> ().text = "Reset"; //reset the floor
			foreach (GameObject floor1 in floors) {
				Destroy (floor1);
			}
			posConnected = new List<Vector3> ();
			allPos = new List<GameObject> ();
			floors = new List<GameObject> ();
			positions = new ArrayList ();
			amountOfEnds = 0;
			amountOfStarts = 0;

			for (int w = 0; w < width; w++) {
				for (int l = 0; l < length; l++) {
					GameObject floor = (GameObject)Instantiate (editorPlane, new Vector3 (l * planewidth, 0, w * planewidth), Quaternion.identity);
					floor.transform.localScale *= planewidth / 10;
					floors.Add (floor);
					allPos.Add (floor);
				}
			}
			float tempL = length - 1;
			float tempW = width - 1;
			cam.transform.position = new Vector3 (tempL / 2, 1, tempW / 2) * planewidth;
			cam.orthographicSize = Mathf.Max (length, width) * planewidth / 3 * 2;
		}
	}





	public static void addPos (Vector2 pos)
	{
		positions.Add (pos);
	}

	public static void removePos (Vector2 pos)
	{
		positions.Remove (pos);
	}

	public static bool checkConnected (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length)
	{
		Vector3 posNormalized = plane.transform.position / planewidth;
		bool res = (posConnected.Contains (posNormalized + new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized - new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized + new Vector3 (0, 0, 1)) || posConnected.Contains (posNormalized - new Vector3 (0, 0, 1)));
		if (res) {
			convertAround (plane, posConnected, allPos, positions, planewidth, length);
		}
		return res;
	}

	public static void Recalculate (List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length)
	{
		LevelEditor.posConnected = new List<Vector3> ();

		foreach (GameObject plane in LevelEditor.allPos) {
			if (plane.renderer.material.color == Color.yellow && plane != LevelEditor.startPlane && plane != LevelEditor.endPlane)
				plane.renderer.material.color = Color.black;
		}
	
		if (LevelEditor.startPlane != null) {
			LevelEditor.posConnected.Add (LevelEditor.startPlane.transform.position / planewidth);

			LevelEditor.convertAround (LevelEditor.startPlane, LevelEditor.posConnected, LevelEditor.allPos, positions, planewidth, length);
		}


	}

	//method that checks all positions around a new connected plane
	public static void convertAround (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length)
	{
		Vector2 planePos = new Vector2 (plane.transform.position.x, plane.transform.position.z) / planewidth;
		int index = allPos.IndexOf (plane);
		GameObject checkPlane;
		//north
		for (int i = 0; i < 4; i++) {
			if (i == 0 && (index - length) >= 0)
				checkPlane = allPos [index - length];
			else if (i == 1 && (index + length) < allPos.Count)
				checkPlane = allPos [index + length];
			else if (i == 2 && (index + 1) < allPos.Count)
				checkPlane = allPos [index + 1];
			else if (i == 3 && (index - 1) >= 0)
				checkPlane = allPos [index - 1];
			else
				checkPlane = null;


			if (checkPlane != null) {
				if (checkPlane.renderer.material.color == Color.black || checkPlane.renderer.material.color == Color.blue) {
					if (checkPlane.renderer.material.color == Color.black)
						checkPlane.renderer.material.color = Color.yellow;
					posConnected.Add (checkPlane.transform.position / planewidth);
					convertAround (checkPlane, posConnected, allPos, positions, planewidth, length);
				}
			}

		}
	}



	private void GenerateLevel ()
	{
		if (endPlane != null) {
			if (LevelEditor.posConnected.Contains (LevelEditor.endPlane.transform.position / planewidth)) {

				for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array
					positions [i] = (Vector2)positions [i] / planewidth;

				}
				startPos = new Vector2 (startPos3.x, startPos3.z);
				endPos = new Vector2 (endPos3.x, endPos3.z);
				resourceManager.startPos = startPos;
				resourceManager.endPos = endPos;

				RandomMaze.GenerateWall (positions, planewidth, wallPrefab, torch, height, length, width, gameObject);

				GenerateFloor ();
				GameObject player = resourceManager.player;
				GameObject camera = resourceManager.mainCamera;
				GameObject Gui = resourceManager.gui;
				RandomMaze.spawnPlayer (player, camera, Gui, startPos * planewidth);
				disableLevelEditor ();
				RandomMaze.createSingleObjects (Minimapcamera, width, length, planewidth, EnemySpawner, endPos);
				RandomMaze.SpawnNodes (positions, nodeSize, planewidth, NodesPos, Nodes, length, width);

				resourceManager.Nodes = Nodes;
			}
			} else
				GenerateLevelButton.GetComponentInChildren<Text> ().text = "Connect end to start!";
		
	}

	private void SwitchType (int type1)
	{
		type = type1;
	}

	private void GenerateFloor ()
	{
		foreach (Vector2 posi in positions) {
			GameObject floor = (GameObject)Instantiate (resourceManager.planePrefab, new Vector3 (posi.x, 0, posi.y) * planewidth, Quaternion.identity);
			floor.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
			floor.transform.parent = gameObject.transform; //Set the floor to the gameObject.
			floor.name = "Floor"; //name the floor Floor
			GameObject ceil = (GameObject)Instantiate (resourceManager.planePrefab, new Vector3 (posi.x, height, posi.y) * planewidth, Quaternion.identity);
			ceil.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
			ceil.transform.parent = gameObject.transform; //Set the floor to the gameObject.
			ceil.transform.Rotate (new Vector3 (180, 0, 0));
			ceil.name = "Floor"; //name the floor Floor
		}
	}

	private void disableLevelEditor ()
	{
		foreach (GameObject floor1 in floors) {
			Destroy (floor1);
		}
		cam.gameObject.SetActive (false);
		Destroy (canvas);
		GameObject.Find ("EditorLight").SetActive (false);
	}





}
