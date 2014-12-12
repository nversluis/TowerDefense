using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour {

	private float planewidth; //Size of the planes
	private float height;
	private float nodeSize; //Distance between nodepoints

	private GameObject editorPlane;
	private GameObject planePrefab; //Floor prefab
	private GameObject wallPrefab; //Wall prefab
	private GameObject EnemySpawner;
	private GameObject Minimapcamera;
	private GameObject Gate;
	private GameObject torch;
	public Camera cam;

	private static ArrayList positions = new ArrayList(); //Positions of the floors
	private List<Vector3> NodesPos = new List<Vector3>(); //Positions of the waypoints/nodes
	public static List<WayPoint> Nodes = new List<WayPoint>(); //List with all Nodes
	private int length;
	private int width;
	private List<GameObject> floors = new List<GameObject> ();

	public InputField lengthInput;
	public InputField widthInput;
	public Button SubmitButton;
	public Button GenerateLevelButton;
	// Use this for initialization
	private GameObject ResourceManagerObj;

	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		ResourceManager resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
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

		SubmitButton.onClick.AddListener(delegate{submitSize();});
		GenerateLevelButton.onClick.AddListener(delegate{GenerateLevel();});
	}

	// Update is called once per frame
	void Update () {	
			
			int length1;
			int width1;
			bool resultLength = int.TryParse (lengthInput.text, out length1);
			bool resultWidth = int.TryParse (widthInput.text, out width1);
		if (resultWidth && resultLength&&int.Parse(lengthInput.text)>0&&int.Parse(widthInput.text)>0) {
			SubmitButton.GetComponentInChildren<Text> ().text = "Confirm/Reset";
			SubmitButton.enabled = true;
		}
		else {
			SubmitButton.enabled = false;
			SubmitButton.GetComponentInChildren<Text> ().text = "Positive Integers Only";
		}


	}

	private void submitSize(){

		length = int.Parse(lengthInput.text);
		width = int.Parse(widthInput.text);
		if (length < 1 || width < 1) {
			SubmitButton.gameObject.transform.renderer.material.color = Color.red;
		} else {

			SubmitButton.GetComponentInChildren<Text> ().text = "Reset";
			foreach (GameObject floor1 in floors)
				Destroy (floor1);

			for (int l = 0; l < length; l++) {
				for (int w = 0; w < width; w++) {
					GameObject floor = (GameObject)Instantiate (editorPlane, new Vector3 (l * planewidth, 0, w * planewidth), Quaternion.identity);
					floors.Add (floor);
				}
			}
			float tempL = length - 1;
			float tempW = width - 1;
			cam.transform.position = new Vector3 (tempL / 2, 1, tempW / 2) * planewidth;
			cam.orthographicSize = Mathf.Max (length, width) * planewidth / 3 * 2;
		}
	}





	public static void addPos(Vector2 pos){
		positions.Add (pos);
	}

	public static void removePos(Vector2 pos){
		positions.Remove (pos);
	}

	private void GenerateLevel(){
		for (int i=0; i < positions.Count; i++) {
			positions [i] =(Vector2)positions[i]/ planewidth;

		}
		GenerateWall ();
	}


	//copy paste from randommaze
	private void GenerateTorch (float n, float w, float angle){
		float torchGrootte = 2;
		GameObject torchObj = (GameObject)Instantiate (torch, new Vector3 (n*planewidth, height * planewidth / 8, w*planewidth), Quaternion.Euler (0, angle, 0));
		torchObj.transform.localScale = new Vector3 (1, 1, 1) * planewidth*torchGrootte/50;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (1).gameObject.light.range *= planewidth*torchGrootte/10;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (0).gameObject.particleSystem.startSize *= planewidth*torchGrootte/2;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (0).gameObject.particleSystem.startSpeed *= planewidth*torchGrootte/2;

	}

	//Method to generate walls
	private void GenerateWall()
	{

		for (int l = 0; l <= length; l++) //for the complete length of the map
		{
			for (int w = -width; w <= width; w++) //and for the complete width of the map
			{
				if (positions.Contains(new Vector2(l, w))) 
				{
					if (!positions.Contains(new Vector2(l + 1, w))) //If there no floor east, create a wall east
					{
						if((l+w)%2==0)
							GenerateTorch(l + 0.5f, w , 90);
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l + 0.5f) * planewidth, height*planewidth / 2, w * planewidth), Quaternion.Euler(90, -90, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
						wall.transform.parent = gameObject.transform;
						wall.name = "Wall";
					}
					if (!positions.Contains(new Vector2(l - 1, w))&&new Vector2(l,w)!=new Vector2(0,0)&&new Vector2(l,w)!=new Vector2(0,-1)) //If there is no floor west, create a wall west
					{
						if((l+w)%2==0)
							GenerateTorch (l - 0.5f, w , -90);
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l - 0.5f) * planewidth, height*planewidth / 2, w * planewidth), Quaternion.Euler(90, 90, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
						wall.transform.parent = gameObject.transform;
						wall.name = "Wall";

					}
					if (!positions.Contains(new Vector2(l, w + 1))) //If there is no floor north, create a wall north
					{
						if((l+w)%2==1)
							GenerateTorch (l, w+0.5f , 0);
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, height*planewidth / 2, (w + 0.5f) * planewidth), Quaternion.Euler(-90, 0, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
						wall.transform.parent = gameObject.transform;
						wall.name = "Wall";

					}
					if (!positions.Contains(new Vector2(l, w - 1))) //If there is no floor south, create a wall south
					{
						if((l+w)%2==1)
							GenerateTorch (l,w-0.5f , 180);
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, height*planewidth / 2, (w - 0.5f) * planewidth), Quaternion.Euler(90, 0, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
						wall.transform.parent = gameObject.transform;
						wall.name = "Wall";

					}
				}
			}
		}
	}



}
