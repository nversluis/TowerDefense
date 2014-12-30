﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
	private GameObject enemySpawner;
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
	private int currentPage;
	private int maxPages;
	private Button currentButSelected;
	private string currentFileSelected;
	//colors
	private Color CnoPlane;
	private Color CstartEnd;
	private Color CConnected;
	private Color CNotConnected;
	private Color CHighlighted;
	private Button prevBut;
	private Button nextBut;
	private bool drawNavGrid;


	public GameObject newMapScreen;
	public InputField lengthInput;
	public InputField widthInput;
	public Button SubmitButton;
	public Button cancelSizeBut;
	public Button newSizeBut;

	public Button GenerateLevelButton;
	public InputField fileNameInput;
	public Button saveLayout;
	public Button loadLayout;
	public Canvas canvas;
	public Text ErrorText;
	public GameObject loadMapsPanel;
	public Button loadButton;
	public Button cancelButton;
	public Button submitLoadButton;
	public Button deleteButton;

	public GameObject LoadingScreen;

	private GameObject player;
	private GameObject camera;
	private bool drawNavigationGrid;
	private string AppPath;



	public static int type;
	public static int amountOfEnds;
	public static int amountOfStarts;
	public static Vector3 endPos3;
	public static Vector3 startPos3;
	public static List<Vector3> posConnected;
	public static List<GameObject> allPos;
	public static GameObject startPlane;
	public static GameObject endPlane;
	public static bool loadScreenOpen;


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
		enemySpawner = resourceManager.enemySpawner;
		Minimapcamera = resourceManager.Minimapcamera;
		Gate = resourceManager.Gate;
		torch = resourceManager.torch;
		drawNavGrid = resourceManager.drawNavigationGrid;
		posConnected = new List<Vector3> ();
		allPos = new List<GameObject> ();
		currentPage = 1;
		CnoPlane = resourceManager.noPlane;
		CstartEnd = resourceManager.startOrEnd;
		CConnected = resourceManager.connected;
		CNotConnected = resourceManager.notConnected;
		CHighlighted = resourceManager.highlighted;
		loadScreenOpen = true;
		AppPath = Application.persistentDataPath + "/MapLayouts/";
		type = 2;
		drawNavigationGrid = resourceManager.drawNavigationGrid;
		player = resourceManager.player;
		camera = resourceManager.mainCamera;


		SubmitButton.onClick.AddListener (delegate {

			submitSize ();
			newMapScreen.SetActive (false);
		
		});
		GenerateLevelButton.onClick.AddListener (delegate {
			if (!loadScreenOpen)
				GenerateLevel ();
			else
				setErrorTekst ("Can't do that while load screen is open");
		});

		saveLayout.onClick.AddListener (delegate {
			if (!loadScreenOpen)
				SavePositionsToFile ();
			else
				setErrorTekst ("Can't do that while load screen is open");
		});

		loadLayout.onClick.AddListener (delegate {
			ShowSavedMaps ();
		});
		submitLoadButton.onClick.AddListener (delegate {
			generateEditorMap ();
		});

		deleteButton.onClick.AddListener (delegate {
			deleteFile (currentFileSelected);
		});

		cancelButton.onClick.AddListener (delegate {
			cancelLoadScreen ();
		});

		newSizeBut.onClick.AddListener (delegate { 
			newMapScreen.SetActive (true);
			loadMapsPanel.SetActive(false);
			loadScreenOpen = true;
		});

		cancelSizeBut.onClick.AddListener (delegate {
			newMapScreen.SetActive (false);
			loadScreenOpen = false;
		});

		amountOfEnds = 0;
		amountOfStarts = 0;
		loadMapsPanel.SetActive (true);

		nextBut = loadMapsPanel.GetComponentsInChildren<Button> () [0];
		prevBut = loadMapsPanel.GetComponentsInChildren<Button> () [1];
		nextBut.onClick.AddListener (delegate {	
			if (currentPage < maxPages) {
				currentPage++;
				ShowSavedMaps ();
			}
		});
		prevBut.onClick.AddListener (delegate {	
			if (currentPage > 1) {
				currentPage--;
				ShowSavedMaps ();
			}
		});
		loadMapsPanel.SetActive (false);

		if (!File.Exists (AppPath)) {
			Directory.CreateDirectory (AppPath);
		}


		GameObject nothingCol = GameObject.Find ("NothingCol");
		nothingCol.transform.GetChild(1).GetComponent<Image>().color = CnoPlane;
		GameObject startCol = GameObject.Find ("StartCol");
		startCol.transform.GetChild (1).GetComponent<Image> ().color = CstartEnd;	
		GameObject endCol = GameObject.Find ("EndCol");
		endCol.transform.GetChild (1).GetComponent<Image> ().color = CstartEnd;
		GameObject conCol = GameObject.Find ("ConCol");
		conCol.transform.GetChild (1).GetComponent<Image> ().color = CConnected;
		GameObject notConCol = GameObject.Find ("NotConCol");
		notConCol.transform.GetChild (1).GetComponent<Image> ().color = CNotConnected;

		startCol.GetComponent<Button>().onClick.AddListener (delegate {
			type=0;
			nothingCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			startCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
			endCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			conCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			notConCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
		});
		endCol.GetComponent<Button>().onClick.AddListener (delegate {
			type=1;
			nothingCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			startCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			endCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
			conCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			notConCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
		});

		conCol.GetComponent<Button>().onClick.AddListener (delegate {
			type=2;
			nothingCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			startCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			endCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			conCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
			notConCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
		});
		notConCol.GetComponent<Button>().onClick.AddListener (delegate {
			type=2;
			nothingCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			startCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			endCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			conCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
			notConCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
		});

		nothingCol.GetComponent<Button>().onClick.AddListener (delegate {
			type=3;
			nothingCol.transform.GetChild (0).GetComponent<Text>().color = Color.green;
			startCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			endCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			conCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
			notConCol.transform.GetChild (0).GetComponent<Text>().color = Color.white;
		});


	}



	// Update is called once per frame
	void Update ()
	{

		if (newMapScreen.activeSelf) {
			int length1;
			int width1;
			bool resultLength = int.TryParse (lengthInput.text, out length1);
			bool resultWidth = int.TryParse (widthInput.text, out width1);
			if (resultWidth && resultLength && int.Parse (lengthInput.text) > 0 && int.Parse (widthInput.text) > 0) {
				SubmitButton.GetComponentInChildren<Text> ().text = "Confirm";
				SubmitButton.enabled = true;
			} else {
				SubmitButton.GetComponentInChildren<Text> ().text = "Not Valid";
				SubmitButton.enabled = false;			
			}
		}

	}

	private void submitSize ()
	{
		loadScreenOpen = false;
		length = int.Parse (lengthInput.text);
		width = int.Parse (widthInput.text);
		resourceManager.length = length;
		resourceManager.width = width;
		if (length < 1 || width < 1) {
			SubmitButton.gameObject.transform.renderer.material.color = Color.red;
		} else {

			SubmitButton.GetComponentInChildren<Text> ().text = "Reset"; //reset the floor
			Reset ();

		}

	}





	public static void addPos (Vector2 pos)
	{
		if (!loadScreenOpen) {
			if (!positions.Contains (pos)) {
				positions.Add (pos);
			}
		}
	}

	public static void removePos (Vector2 pos)
	{
		if (!loadScreenOpen) {
			positions.Remove (pos);
		}

	}

	public static bool checkConnected (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width, Color CConnected, Color CNotConnected, Color CstartEnd)
	{
		Vector3 posNormalized = plane.transform.position / planewidth;
		bool res = (posConnected.Contains (posNormalized + new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized - new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized + new Vector3 (0, 0, 1)) || posConnected.Contains (posNormalized - new Vector3 (0, 0, 1)));
		if (res) {
			convertAround (plane, posConnected, allPos, positions, planewidth, length, width, CstartEnd, CNotConnected, CConnected);
		}
		return res;
	}

	//clear and recheck if everything is connected
	public static void Recalculate (List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width, Color CConnected, Color CNotConnected, Color CstartEnd)
	{
		LevelEditor.posConnected = new List<Vector3> ();
		foreach (GameObject plane in LevelEditor.allPos) {
			if (plane.renderer.material.color == CConnected && plane != LevelEditor.startPlane && plane != LevelEditor.endPlane)
				plane.renderer.material.color = CNotConnected;
		}	
		if (LevelEditor.startPlane != null) {
			LevelEditor.posConnected.Add (LevelEditor.startPlane.transform.position / planewidth);
			LevelEditor.convertAround (startPlane, posConnected, allPos, positions, planewidth, length, width, CstartEnd, CNotConnected, CConnected);		
		}
	}

	//method that checks all positions around a new connected plane
	public static void convertAround (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width, Color CstartEnd, Color CNotConnected, Color CConnected)
	{
		Vector2 planePos = new Vector2 (plane.transform.position.x, plane.transform.position.z) / planewidth;
		int index = allPos.IndexOf (plane);
		GameObject checkPlane;


		for (int i = 0; i < 4; i++) { //check in all directions
			if (i == 0 && plane.transform.position.z / planewidth > 0 && (index - length) >= 0)
				checkPlane = allPos [index - length];
			else if (i == 1 && (plane.transform.position.z / planewidth) < (width - 1) && (index + length) < allPos.Count)
				checkPlane = allPos [index + length];
			else if (i == 2 && (plane.transform.position.x / planewidth) < (length - 1) && (index + 1) < allPos.Count)
				checkPlane = allPos [index + 1];
			else if (i == 3 && plane.transform.position.x > 0 && (index - 1) >= 0)
				checkPlane = allPos [index - 1];
			else {
				checkPlane = null;
			}

			if (checkPlane != null) {
				if (checkPlane.renderer.material.color == CNotConnected || (checkPlane.renderer.material.color == CstartEnd && !posConnected.Contains (checkPlane.transform.position / planewidth))) {

					if (checkPlane.renderer.material.color == CNotConnected) {
						checkPlane.renderer.material.color = CConnected;
					}
					LevelEditor.posConnected.Add (checkPlane.transform.position / planewidth);
					convertAround (checkPlane, posConnected, allPos, positions, planewidth, length, width, CstartEnd, CNotConnected, CConnected);
				}
			}
		}


	}



	IEnumerator spawnLevel ()
	{

		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Destroying what you just built";
		yield return new WaitForSeconds (0.1f);
		disableLevelEditor ();
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Rebuilding the floors you just built";
		yield return new WaitForSeconds (0.1f);
		GenerateFloor ();
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: You built the floors, we place the walls!...";
		yield return new WaitForSeconds (0.1f);
		RandomMaze.GenerateWall (positions, planewidth, wallPrefab, torch, height, length, width, gameObject);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Dwogres wanted a red carpet to walk on, generating..";
		yield return new WaitForSeconds (0.1f);
		Nodes = RandomMaze.SpawnNodes (positions, nodeSize, planewidth, NodesPos, Nodes, length, width, drawNavigationGrid, false);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Giving birth to Player...=";
		yield return new WaitForSeconds (0.1f);
		RandomMaze.spawnPlayer (player, camera, resourceManager.GUI, resourceManager.eventListener, startPos * planewidth, Minimapcamera, width, length, planewidth);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Lightning torches..";
		yield return new WaitForSeconds (0.1f);
		RandomMaze.createSingleObjects (planewidth, enemySpawner, resourceManager.Goal, endPos, startPos);
		//generate Nodes;
		//MakeNodeList (nodeSize,NodesPos,Nodes);
		//create the minimap camera
		resourceManager.Nodes = Nodes;
		LoadingScreen.SetActive (false);

	}


	private void GenerateLevel ()
	{
		if (endPlane != null) {

			if (LevelEditor.posConnected.Contains (LevelEditor.endPlane.transform.position / planewidth)) {
				playing = true;
				for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array
					positions [i] = (Vector2)positions [i] / planewidth;
					Vector2 tempPos = (Vector2)positions [i];
					ResourceManager.mostNorth = Mathf.Max (ResourceManager.mostNorth, (int)tempPos.y);
					ResourceManager.mostEast = Mathf.Max (ResourceManager.mostEast, (int)tempPos.x);
					ResourceManager.mostSouth = Mathf.Min (ResourceManager.mostSouth, (int)tempPos.y);
					ResourceManager.mostWest = Mathf.Min (ResourceManager.mostWest, (int)tempPos.x);
				}
				startPos = new Vector2 (startPos3.x, startPos3.z);
				endPos = new Vector2 (endPos3.x, endPos3.z);
				resourceManager.startPos = startPos;
				resourceManager.endPos = endPos;
				LoadingScreen.SetActive (true);
				StartCoroutine (spawnLevel ());

//				RandomMaze.GenerateWall (positions, planewidth, wallPrefab, torch, height, length, width, gameObject);
//
//				GenerateFloor ();
//				GameObject player = resourceManager.player;
//				GameObject camera = resourceManager.mainCamera;
//				RandomMaze.spawnPlayer (player, camera,resourceManager.GUI,resourceManager.eventListener, startPos * planewidth, Minimapcamera, width, length, planewidth);
//				disableLevelEditor ();
//				RandomMaze.createSingleObjects (planewidth, enemySpawner, resourceManager.Goal, endPos, startPos);
//				RandomMaze.SpawnNodes (positions, nodeSize, planewidth, NodesPos, Nodes, length, width, drawNavGrid, true);

				resourceManager.Nodes = Nodes;
			} else
				setErrorTekst ("Connect end to start!");
		} else
			setErrorTekst ("Place end position!");

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

	//disable the gui, so you can play the game
	private void disableLevelEditor ()
	{
		foreach (GameObject floor1 in floors) {
			Destroy (floor1);
		}
		cam.gameObject.SetActive (false);
		Destroy (canvas);
		GameObject.Find ("EditorLight").SetActive (false);
	}

	//reset the floor. Clearing everything. Then reconstruct it with width and length
	private void Reset ()
	{
		foreach (GameObject floor1 in floors) {
			Destroy (floor1);
		}
		posConnected = new List<Vector3> ();
		allPos = new List<GameObject> ();
		floors = new List<GameObject> ();
		positions = new ArrayList ();
		amountOfEnds = 0;
		amountOfStarts = 0;

		for (int w = 0; w < width; w++) { //order important!
			for (int l = 0; l < length; l++) {
				GameObject floor = (GameObject)Instantiate (editorPlane, new Vector3 (l * planewidth, 0, w * planewidth), Quaternion.identity);
				floor.transform.localScale *= planewidth / 10;
				floor.renderer.material.color = CnoPlane;
				floor.transform.parent = transform;
				floors.Add (floor);
				allPos.Add (floor);
			}
		}
		float tempL = length - 1;
		float tempW = width - 1;
		cam.transform.position = new Vector3 (tempL / 2, 1, tempW / 2) * planewidth;
		cam.orthographicSize = Mathf.Max (length, width + 1) * planewidth / 2;
		cam.rect = new Rect (0.25f, 0f, 0.5f, 0.6f);
	}

	//saves the position to file.
	private void SavePositionsToFile ()
	{

		if (endPlane != null) {

			if (LevelEditor.posConnected.Contains (LevelEditor.endPlane.transform.position / planewidth)) {

				string res = "\r\n";

				res += resourceManager.length + "\r\n" + resourceManager.width + "\r\n" + startPos3.x.ToString () + "\r\n" + startPos3.z.ToString () + "\r\n";
				foreach (Vector2 pos in positions) {
					if (pos / planewidth != resourceManager.startPos && pos / planewidth != resourceManager.endPos) {
						for (int i = 0; i <= 1; i++) {
							res += (pos [i] / planewidth).ToString () + "\r\n";
						}
					}
				}
				res += endPos3.x.ToString () + "\r\n" + endPos3.z.ToString () + "\r\n";
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Create (AppPath + fileNameInput.text + ".txt");
				bf.Serialize (file, res);
				Debug.Log (AppPath);
			} else
				setErrorTekst ("Connect end to start!");
		} else
			setErrorTekst ("Place end position!");

	}

	//load from file and displays a simple minimap version in the loadscreen.
	private void loadMapFromFile (string fileName)
	{
		if (fileName != null) {
			string line;
			List<int> datas = new List<int> ();
			ArrayList positions = new ArrayList ();
			Debug.Log ("Getting Data from " + fileName);
			StreamReader file = new StreamReader (AppPath + fileName + ".txt");
			file.ReadLine ();
			length = int.Parse (file.ReadLine ());
			width = int.Parse (file.ReadLine ());
			int num1;
			while ((line = file.ReadLine ()) != null) {
				bool isInt = int.TryParse (line, out num1);
				if (isInt) {
					datas.Add (int.Parse (line));
				}
			}
			file.Close ();

			resourceManager.length = length;
			resourceManager.width = width;

			Reset ();
			ResourceManager.mostNorth = 0;
			ResourceManager.mostEast = 0;
			ResourceManager.mostSouth = 1000;
			ResourceManager.mostWest = 1000;
			//add positions
			for (int i = 0; i < datas.Count; i += 2) {
				if (!positions.Contains (new Vector2 (datas [i], datas [i + 1]))) {
					positions.Add (new Vector2 (datas [i], datas [i + 1]));
					ResourceManager.mostNorth = Mathf.Max (ResourceManager.mostNorth, (int)datas [i + 1]);
					ResourceManager.mostEast = Mathf.Max (ResourceManager.mostEast, (int)datas [i]);
					ResourceManager.mostSouth = Mathf.Min (ResourceManager.mostSouth, (int)datas [i + 1]);
					ResourceManager.mostWest = Mathf.Min (ResourceManager.mostWest, (int)datas [i]);
				}
			}
			//
			//Generate start point, end point, and all others, set start point to connected and run
			LevelEditor.startPos3 = new Vector3 (datas [0], 0, datas [1]);
			LevelEditor.endPos3 = new Vector3 (datas [datas.Count - 2], 0, datas [datas.Count - 1]);
			resourceManager.startPos = new Vector2 (startPos3.x, startPos3.z);
			resourceManager.endPos = new Vector2 (endPos3.x, endPos3.z);
			foreach (GameObject floor1 in floors) {
				if (floor1.transform.position / planewidth == startPos3) {
					LevelEditor.startPlane = floor1;	
					floor1.renderer.material.color = CstartEnd;
					LevelEditor.posConnected.Add (floor1.transform.position / planewidth);
				} else if (floor1.transform.position / planewidth == endPos3) {

					LevelEditor.endPlane = floor1;
					floor1.renderer.material.color = CstartEnd;
				} else if (positions.Contains (new Vector2 (floor1.transform.position.x, floor1.transform.position.z) / planewidth)) {			
					floor1.renderer.material.color = CNotConnected;
				}
			}
			LevelEditor.positions = positions;
			Vector3 panelPos = cam.WorldToScreenPoint (loadMapsPanel.transform.position) / 2;
			//ChangeTypes camera position and size to fit in load screen
			cam.transform.position = new Vector3 (length - 1, 1, width - 1) * resourceManager.planewidth / 2;
			cam.orthographicSize = Mathf.Max (length, width + 1) * resourceManager.planewidth / 2;
			cam.rect = new Rect (0.4f, 0.3f, 0.4f, 0.4f);


		}

	}


	private void generateEditorMap ()
	{
		if (currentFileSelected != null) {
			loadScreenOpen = false;
			loadMapsPanel.SetActive (false);
			//reconstruct connected
			Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, resourceManager.planewidth, resourceManager.length, resourceManager.width, CConnected, CNotConnected, CstartEnd);
			amountOfEnds = 1;
			amountOfStarts = 1;
			LevelEditor.positions = positions;

			for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array so Generate level can work
				positions [i] = (Vector2)positions [i] * planewidth;
			}
			//change camera position and size back
			cam.transform.position = new Vector3 (length - 1, 1, width - 1) * resourceManager.planewidth / 2;
			cam.orthographicSize = Mathf.Max (length, width + 1) * planewidth / 2;
			cam.rect = new Rect (0.25f, 0f, 0.5f, 0.6f);
			cam.Render ();
		} else {
			setErrorTekst ("No File Selected");
		}

	}

	// Use this for initialization
	private void ShowSavedMaps ()
	{
		loadScreenOpen = true;
		newMapScreen.SetActive (false);
		cam.pixelRect = new Rect (Screen.width / 2, Screen.height / 2 - 100, 200, 200);
		loadMapsPanel.gameObject.SetActive (true);
		foreach (Transform child in loadMapsPanel.transform) {
			if (child.gameObject.name.Contains ("load"))
				Destroy (child.gameObject);
		}
		int rows = (int)Mathf.Floor ((Screen.height - 200) / ((Screen.width + 350) / 35));
		int columns = 1;
		int filesPerPage = rows * columns;
		nextBut.gameObject.SetActive (true);
		prevBut.gameObject.SetActive (true);
		nextBut.GetComponentInChildren<Text> ().text = "Next" + filesPerPage;
		prevBut.GetComponentInChildren<Text> ().text = "Prev" + filesPerPage;
		if (currentPage == maxPages)
			nextBut.gameObject.SetActive (false);
		else if (currentPage == 1)
			prevBut.gameObject.SetActive (false);



		//create a list with the names of all layouts.
		BinaryFormatter bf = new BinaryFormatter ();
		//FileStream file = File.
		string[] dirFiles = Directory.GetFiles (AppPath, "*.txt"); 
		maxPages = (int)Mathf.Ceil ((float)dirFiles.Length / (float)filesPerPage);
		for (int i = 0; i < dirFiles.Length; i++) {
			if (i < filesPerPage * currentPage && i >= filesPerPage * (currentPage - 1)) {
				dirFiles [i] = dirFiles [i].Replace (AppPath, "");
				dirFiles [i] = dirFiles [i].Replace (".txt", "");
				int j = i % filesPerPage;
				//
				Button but = (Button)Instantiate (loadButton, new Vector3 (0, 0, 0), Quaternion.identity); 
				//but.transform.position = new Vector3 (300, 400 - 25f*(j % rows), 0);
				//Button but = (Button)Instantiate (loadButton, loadMapsPanel.transform.position + new Vector3 (Mathf.Floor (j / rows) * 50 - 180, 120 - 30 * (j % rows)), Quaternion.identity); //breedte,hoogte
				//Button but = (Button)Instantiate (loadButton, loadMapsPanel.transform.position + new Vector3 ((Mathf.Floor (j / rows) - 1.5f) * 25,0, 30 - 5 * (j % rows)), loadMapsPanel.transform.rotation);
				but.transform.SetParent (loadMapsPanel.gameObject.transform);
				but.GetComponent<RectTransform> ().offsetMin = new Vector2 (350, 50 + 120 * (rows - j));
				but.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1000, 200);

				but.GetComponentInChildren<Text> ().text = dirFiles [i];
				but.transform.localScale = new Vector3 (1, 1, 1) / 4;
				but.transform.GetChild (1).gameObject.SetActive (false);

				string fileName = dirFiles [i];
				but.onClick.AddListener (delegate {
					selectFileName (but);
				});
			}
		}


	}


	private void selectFileName (Button but)
	{
		if (currentButSelected != but) {
			but.GetComponent<Image> ().color = Color.gray;
			//but.transform.GetChild (1).gameObject.SetActive (true);
			if (currentButSelected != null) {
				currentButSelected.GetComponent<Image> ().color = Color.white;
				currentButSelected.transform.GetChild (1).gameObject.SetActive (false);
			}
			currentButSelected = but;
			currentFileSelected = but.GetComponentInChildren<Text> ().text;
			loadMapFromFile (currentFileSelected);
		} else { //else. Load him.
			loadScreenOpen = false;
			generateEditorMap ();
			currentButSelected = null;
			currentFileSelected = null;
		}
	}

	private void deleteFile (string fileName)
	{
		if (fileName != null) {
			File.Delete (AppPath + fileName + ".txt");
			ShowSavedMaps ();
		} else {
			setErrorTekst ("No File Selected");
		}
	}

	private void cancelLoadScreen ()
	{
		loadScreenOpen = false;
		currentButSelected = null;
		currentFileSelected = null;
		loadMapsPanel.SetActive (false);
		Reset ();
		foreach (GameObject plane in allPos) {
			Destroy (plane);
		}
	}

	//methods to display errortext
	private void setErrorTekst (string tekst)
	{

		CancelInvoke ();
		if (!playing) {
			ErrorText.text = tekst;
			ErrorText.color = new Color (255, 0, 0, 1);
			InvokeRepeating ("errorTekstFadeOut", 0.5f, 0.01f);
		}
	}

	void errorTekstFadeOut ()
	{
		if (!playing) {
			Color color = ErrorText.color;
			color.a -= 0.01f;
			ErrorText.color = color;
			if (color.a <= 0f) {
				color.a = 1;
				ErrorText.color = color;
				ErrorText.text = "";
				CancelInvoke ();
			}
		}
	}
}

