using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
	private int currentPage;
	private int maxPages;
	//colors
	private Color cNoPlane;
	private Color cStartEnd;
	private Color cConnected;
	private Color cNotConnected;
	private Color cHighlighted;


	public InputField lengthInput;
	public InputField widthInput;
	public Button SubmitButton;
	public Button GenerateLevelButton;
	public Button selectStart;
	public Button selectEnd;
	public Button selectNormal;
	public InputField fileNameInput;
	public Button saveLayout;
	public Button loadLayout;
	public GameObject canvas;
	public Text ErrorText;
	public GameObject loadMapsPanel;
	public Button loadButton;



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
		currentPage = 1;
		cNoPlane = resourceManager.noPlane;
		cStartEnd = resourceManager.startOrEnd;
		cConnected = resourceManager.connected;
		cNotConnected = resourceManager.notConnected;
		cHighlighted = resourceManager.highlighted;

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

		saveLayout.onClick.AddListener (delegate {
			SavePositionsToFile ();
		});

		loadLayout.onClick.AddListener (delegate {
			ShowSavedMaps ();
		});
		amountOfEnds = 0;
		amountOfStarts = 0;
		loadMapsPanel.SetActive (true);

		Button nextBut = loadMapsPanel.GetComponentsInChildren<Button> () [0];
		Button prevBut = loadMapsPanel.GetComponentsInChildren<Button> () [1];
		nextBut.onClick.AddListener (delegate {	
			if(currentPage<maxPages){
			currentPage++;
			ShowSavedMaps ();
				}

		});

	
		prevBut.onClick.AddListener (delegate {	
			if(currentPage>1){
			currentPage--;
			ShowSavedMaps ();
			}
		});
		loadMapsPanel.SetActive (false);
	}
		


	// Update is called once per frame
	void Update ()
	{

		if (!playing) {
			int length1;
			int width1;
			bool resultLength = int.TryParse (lengthInput.text, out length1);
			bool resultWidth = int.TryParse (widthInput.text, out width1);
			if (resultWidth && resultLength && int.Parse (lengthInput.text) > 0 && int.Parse (widthInput.text) > 0) {
				SubmitButton.GetComponentInChildren<Text> ().text = "Confirm/Reset";
				SubmitButton.enabled = true;
			} else {
				SubmitButton.enabled = false;			
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
			Reset ();

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

	public static bool checkConnected (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width)
	{
		Vector3 posNormalized = plane.transform.position / planewidth;
		bool res = (posConnected.Contains (posNormalized + new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized - new Vector3 (1, 0, 0)) || posConnected.Contains (posNormalized + new Vector3 (0, 0, 1)) || posConnected.Contains (posNormalized - new Vector3 (0, 0, 1)));
		if (res) {
			convertAround (plane, posConnected, allPos, positions, planewidth, length, width);
		}
		return res;
	}

	//clear and recheck if everything is connected
	public static void Recalculate (List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width)
	{
		LevelEditor.posConnected = new List<Vector3> ();
		foreach (GameObject plane in LevelEditor.allPos) {
			if (plane.renderer.material.color == Color.yellow && plane != LevelEditor.startPlane && plane != LevelEditor.endPlane)
				plane.renderer.material.color = Color.black;
		}	
		if (LevelEditor.startPlane != null) {
			LevelEditor.posConnected.Add (LevelEditor.startPlane.transform.position / planewidth);
			LevelEditor.convertAround (startPlane, posConnected, allPos, positions, planewidth, length, width);		
		}
	}

	//method that checks all positions around a new connected plane
	public static void convertAround (GameObject plane, List<Vector3> posConnected, List<GameObject> allPos, ArrayList positions, float planewidth, int length, int width)
	{
		Vector2 planePos = new Vector2 (plane.transform.position.x, plane.transform.position.z) / planewidth;
		int index = allPos.IndexOf (plane);
		GameObject checkPlane;

		//north
		for (int i = 0; i < 4; i++) { //check in all directions
			if (i == 0 && plane.transform.position.z > 0 && (index - length) >= 0)
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
				if (checkPlane.renderer.material.color == Color.black || (checkPlane.renderer.material.color == Color.blue && !posConnected.Contains (checkPlane.transform.position / planewidth))) {

					if (checkPlane.renderer.material.color == Color.black) {
						checkPlane.renderer.material.color = Color.yellow;
					}
					LevelEditor.posConnected.Add (checkPlane.transform.position / planewidth);
					convertAround (checkPlane, posConnected, allPos, positions, planewidth, length, width);
				}
			}
		}

		
	}



	private void GenerateLevel ()
	{
		if (endPlane != null) {
			if (LevelEditor.posConnected.Contains (LevelEditor.endPlane.transform.position / planewidth)) {
				playing = true;
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
			} else
				setErrorTekst ("Connect end to start!");
		} else
			setErrorTekst ("Place end position!");
		
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

	//saves the position to file.
	private void SavePositionsToFile ()
	{

		if (endPlane != null) {
			if (LevelEditor.posConnected.Contains (LevelEditor.endPlane.transform.position / planewidth)) {

				string res = "";

				res += resourceManager.length+"\r\n" +resourceManager.width+ "\r\n" +startPos3.x.ToString () + "\r\n" + startPos3.z.ToString () + "\r\n";
				foreach (Vector2 pos in positions) {
					if (pos / planewidth != resourceManager.startPos && pos / planewidth != resourceManager.endPos) {
						for (int i = 0; i <= 1; i++) {
							res += (pos [i] / planewidth).ToString () + "\r\n";
						}
					}
				}
				res += endPos3.x.ToString () + "\r\n" + endPos3.z.ToString () + "\r\n";
				File.WriteAllText (Application.dataPath + "/MapLayouts/" + fileNameInput.text + ".txt", res);
			} else
				setErrorTekst ("Connect end to start!");
		} else
			setErrorTekst ("Place end position!");

	}


	private void loadMapFromFile (string fileName)
	{
		loadMapsPanel.SetActive (false);
		string line;
		List<int> datas = new List<int> ();
		ArrayList positions = new ArrayList ();
		Debug.Log ("Getting Data from " + fileName);
		StreamReader file = new StreamReader (Application.dataPath + "/MapLayouts/" + fileName + ".txt");
		length = int.Parse (file.ReadLine ());
		width = int.Parse (file.ReadLine ());
		while ((line = file.ReadLine ()) != null) {
			datas.Add (int.Parse (line));
		}
		file.Close ();

		resourceManager.length = length;
		resourceManager.width = width;

		Reset ();

		//add positions
		for (int i = 0; i < datas.Count; i += 2) {
			positions.Add (new Vector2 (datas [i], datas [i + 1]));
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
				floor1.renderer.material.color = Color.blue;
				LevelEditor.posConnected.Add (floor1.transform.position / planewidth);
			} else if (floor1.transform.position / planewidth == endPos3) {

				LevelEditor.endPlane = floor1;
				floor1.renderer.material.color = Color.blue;
			} else if (positions.Contains (new Vector2 (floor1.transform.position.x, floor1.transform.position.z) / planewidth)) {			
				floor1.renderer.material.color = Color.black;
			}
		}
		//reconstruct connected

		Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, resourceManager.planewidth, resourceManager.length, resourceManager.width);
		amountOfEnds = 1;
		amountOfStarts = 1;
		LevelEditor.positions = positions;

		for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array so Generate level can work
			positions [i] = (Vector2)positions [i] * planewidth;

		}

	}

	// Use this for initialization
	private void ShowSavedMaps ()
	{
		foreach (Transform child in loadMapsPanel.transform) {
			if (child.gameObject.name.Contains ("load"))
				Destroy (child.gameObject);
		}
		int rows = 10;
		int columns = 3;
		int filesPerPage = rows * columns;
		loadMapsPanel.gameObject.SetActive (true);
		//create a list with the names of all layouts.
		string[] dirFiles = Directory.GetFiles (Application.dataPath + "/MapLayouts/", "*.txt");
		maxPages = (int)Mathf.Ceil ((float)dirFiles.Length / (float)filesPerPage);
		for (int i = 0; i < dirFiles.Length; i++) {
			if (i < filesPerPage * currentPage && i >= filesPerPage * (currentPage - 1)) {
				dirFiles [i] = dirFiles [i].Replace (Application.dataPath + "/MapLayouts/", "");
				dirFiles [i] = dirFiles [i].Replace (".txt", "");
				int j = i % filesPerPage;
				Button but = (Button)Instantiate (loadButton, loadMapsPanel.transform.position + new Vector3 ((Mathf.Floor (j / rows) - 1) * 150, 130 - 30 * (j % rows), 0), Quaternion.identity);
				but.transform.SetParent (loadMapsPanel.gameObject.transform);
				but.GetComponentInChildren<Text> ().text = "Load: " + dirFiles [i];
				string fileName = dirFiles [i];
				but.onClick.AddListener (delegate {
					loadMapFromFile (fileName);
				});
			}
		}

		//script for the previous/next buttons

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

