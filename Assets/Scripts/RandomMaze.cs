using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Generates the map
public class RandomMaze : MonoBehaviour
{

	private int length;
	//Length of the map
	private int width;
	//With of the map
	private float planewidth;
	//Size of the planes
	private float height;
	private float nodeSize;
	//Distance between nodepoints
	private int NumberOfPaths;
	//Number of paths to the end

	private GameObject planePrefab;
	//Floor prefab
	private GameObject wallPrefab;
	//Wall prefab
	private GameObject node;
	//Node prefab
	private GameObject enemySpawner;
	private GameObject Minimapcamera;
	private GameObject Gate;
	private GameObject torch;
	public GameObject LoadingScreen;

	private GameObject player;
	private GameObject camera;
	private Vector2 startPos;
	private Vector2 endPos;
	private Vector4 lastPos;

	private ArrayList positions;
	//Positions of the floors
	public static List<Vector3> NodesPos;
	//Positions of the waypoints/nodes
	public static List<WayPoint> Nodes;
	//List with all Nodes
    private static bool drawNavigationGrid;
    AudioSource cameraAudioSource;
    public AudioClip startGame;
    public GameObject mainCamera;






	private GameObject ResourceManagerObj;
	ResourceManager resourceManager;

	//Use this for initialization
	void Awake ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		mainCamera = resourceManager.mainCamera;
        cameraAudioSource = mainCamera.GetComponent<AudioSource>();
		length = resourceManager.length;
		width = resourceManager.width;
		planewidth = resourceManager.planewidth;
		height = resourceManager.height;
		nodeSize = resourceManager.nodeSize;
		NumberOfPaths = resourceManager.NumberOfPaths;
		planePrefab = resourceManager.planePrefab;
		wallPrefab = resourceManager.wallPrefab;
		enemySpawner = resourceManager.enemySpawner;
		Minimapcamera = resourceManager.Minimapcamera;
		Gate = resourceManager.Gate;
		torch = resourceManager.torch;
        drawNavigationGrid = resourceManager.drawNavigationGrid;
		player = resourceManager.player;
		camera = resourceManager.mainCamera;
		startPos = new Vector2 (0, 0); //Start position where enemy´s spawn
		endPos = new Vector2 (length, 0); //End position where enemy´s go
		resourceManager.startPos = startPos;
		resourceManager.endPos = endPos;
		lastPos = new Vector4 (1, 1, 1, 1); //position where the map last was

		positions = new ArrayList ();
		NodesPos = new List<Vector3> ();
		Nodes = new List<WayPoint> ();
        LoadingScreen.SetActive(true);

        cameraAudioSource.PlayOneShot(startGame, 5f);

        Invoke("LevelSpawner", 1.802f);
	}

    void LevelSpawner()
    {

        StartCoroutine(spawnLevel());

    }

	IEnumerator spawnLevel(){

        //Destroy(mainCamera);
		LoadingScreen.GetComponentInChildren<Text>().text = "Loading: Building a castle..";
        //Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
		GenerateFloor ();
		//Generate walls
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: We forgot walls, building walls...";
		yield return new WaitForSeconds(0.1f);
		GenerateWall (positions,planewidth,wallPrefab,torch,height,length,width,gameObject,endPos);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Dwogres wanted a red carpet to walk on, generating..";
		yield return new WaitForSeconds(0.1f);
		Nodes=SpawnNodes (positions,nodeSize, planewidth, Nodes,length,width,drawNavigationGrid,false,endPos);
		LoadingScreen.GetComponentInChildren<Text>().text = "Loading: Giving birth to Player...=";
		yield return new WaitForSeconds(0.1f);
        spawnPlayer(player, camera, resourceManager.Goal, enemySpawner, resourceManager.GUI, resourceManager.eventListener, startPos * planewidth, endPos, Minimapcamera, width, length, planewidth);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Lightning torches..";
        //yield return new WaitForSeconds(0.1f);
        //createSingleObjects (planewidth,enemySpawner,endPos,startPos);
		//generate Nodes;
		//MakeNodeList (nodeSize,NodesPos,Nodes);
		//create the minimap camera
		resourceManager.Nodes = Nodes;
		LoadingScreen.SetActive (false);

	}

	//generate floors
	private void GenerateFloor ()
	{

		float north;
		float east;
		float south;
		float west;
		float rnd;
		ResourceManager.mostNorth =0;
		ResourceManager.mostEast = 0;
		ResourceManager.mostSouth = 10000;
		ResourceManager.mostWest = 100000;
		int N = NumberOfPaths;
		for (int i = 0; i < N; i++) { //run this the amount of timres of the numper of paths
			Vector2 curPos = startPos; //current position is start position
			for (int ba = 0; ba < 300; ba++) { //if it takes longer than 300 steps, stop 
				if (!curPos.Equals (endPos)) { //Continue to run if curPos is not equal to the end position
					if (!positions.Contains (curPos)) { //Continue only if curPos is not yet in positions
						positions.Add (curPos); //add current position to arraylist								
						GameObject floor = (GameObject)Instantiate (planePrefab, new Vector3 ((curPos [0]) * planewidth, 0, (curPos [1]) * planewidth), Quaternion.identity); //Instantiate a floor at current position
						floor.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
						floor.transform.parent = gameObject.transform; //Set the floor to the gameObject.
						floor.name = "Floor"; //name the floor Floor
						GameObject ceil = (GameObject)Instantiate (planePrefab, new Vector3 ((curPos [0]) * planewidth, height * planewidth, (curPos [1]) * planewidth), Quaternion.identity); //Instantiate a floor at current position
						ceil.transform.Rotate (new Vector3 (180, 0, 0));
						ceil.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
						ResourceManager.mostNorth = Mathf.Max (ResourceManager.mostNorth, (int)curPos [1]);
						ResourceManager.mostEast = Mathf.Max (ResourceManager.mostEast, (int)curPos [0]);
						ResourceManager.mostSouth = Mathf.Min (ResourceManager.mostSouth, (int)curPos [1]);
						ResourceManager.mostWest = Mathf.Min (ResourceManager.mostWest, (int)curPos [0]);

					}

					//next position
					//Is biased to go east. Can go west
					//Second term is to keep it in bounds [0, length], [-width,width]/2
					//Third term is to make sure you can´t go back, and it´s more likely to go the same way as the last time.
					//If it´s closer to end, biased to go that way, last term
					north = 0.4f * ConvertBool (curPos [1] + 1 <= width / 2) * lastPos [3] / (1 + Vector2.Distance ((curPos + new Vector2 (0, 1)), endPos));
					south = 0.4f * ConvertBool (curPos [1] - 1 >= -width / 2) * lastPos [0] / (1 + Vector2.Distance ((curPos + new Vector2 (0, -1)), endPos));
					east = 0.7f * ConvertBool (curPos [0] + 1 <= length) * lastPos [2] / (1 + Vector2.Distance ((curPos + new Vector2 (1, 0)), endPos));
					west = 0.0f;
					rnd = Random.value * (north + south + east + west); //random value

					if (rnd < north) { //go north
						curPos [1]++;
						lastPos *= 0;
						lastPos += new Vector4 (0, 1, 1, 2);
					} else if (rnd < north + east) { //go east
						curPos [0]++;
						lastPos *= 0;
						lastPos += new Vector4 (1, 0, 2, 1);
					} else if (rnd < north + east + south) { //go south
						curPos [1]--;
						lastPos *= 0;
						lastPos += new Vector4 (2, 1, 1, 0);
					} else { //go west
						curPos [0]--;
						lastPos *= 0;
						lastPos += new Vector4 (1, 2, 0, 1);
					}
				
				} else { //if you cant continue, just stop
					ba = 500;


				}

			}

		}
		GameObject floor2 = (GameObject)Instantiate (planePrefab, new Vector3 (endPos [0] * planewidth, 0, endPos [1] * planewidth), Quaternion.identity); //Generate floor at end position
		floor2.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20);
		floor2.transform.parent = gameObject.transform;
		GameObject ceil2 = (GameObject)Instantiate (planePrefab, new Vector3 (endPos [0] * planewidth, height * planewidth, endPos [1] * planewidth), Quaternion.identity); //Instantiate a floor at current position
		ceil2.transform.Rotate (new Vector3 (180, 0, 0));
		ceil2.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor

		positions.Add (endPos); //Add the end position to position
		   

	}


	public static void GenerateTorch (float n, float w, float angle,GameObject torch,float planewidth,float height)
	{
		float torchGrootte = 2;

		GameObject torchObj = (GameObject)Instantiate (torch, new Vector3 (n * planewidth, height * planewidth / 8, w * planewidth), Quaternion.Euler (0, angle, 0));
		torchObj.transform.localScale = new Vector3 (1, 1, 1) * planewidth * torchGrootte / 50;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (1).gameObject.light.range *= planewidth * torchGrootte / 10;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (0).gameObject.particleSystem.startSize *= planewidth * torchGrootte / 2;
		torchObj.transform.GetChild (3).gameObject.transform.GetChild (0).gameObject.particleSystem.startSpeed *= planewidth * torchGrootte / 2;

	}

	static int hasGate;
	//Method to generate walls
	public static void GenerateWall (ArrayList positions,float planewidth,GameObject wallPrefab, GameObject torch,float height,int length,int width,GameObject parent,Vector2 endPos)
	{
		hasGate = 0;
		int n;
		GameObject ResourceManagerObj = GameObject.Find ("ResourceManager");
		ResourceManager resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		for (int l = 0; l <= length; l++) { //for the complete length of the map
			for (int w = -width; w <= width; w++) { //and for the complete width of the map
				if (positions.Contains (new Vector2 (l, w))) {
					if (!positions.Contains (new Vector2 (l + 1, w))){ //If there no floor east, create a wall east
						if ((l + w) % 2 == 0 && new Vector2 (l, w) != endPos)
							GenerateTorch (l + 0.5f, w, 90,torch,planewidth,height);
						if (l==length && new Vector2 (l, w) == endPos && hasGate==0) {
							n = 2;
							hasGate = 1;
							GameObject gate = (GameObject)Instantiate(resourceManager.Gate, new Vector3(endPos.x+0.55f, 0, endPos.y)*planewidth, Quaternion.Euler(0,-90,0));
							gate.transform.localScale /= 1.9f;
						} else {
							n = 0;
						}
						for (int h = n; h < height; h++) {
							GameObject wall = (GameObject)Instantiate (wallPrefab, new Vector3 ((l + 0.5f) * planewidth, (h+.5f)* planewidth, w * planewidth), Quaternion.Euler (90, -90, 0));
							//wall.gameObject.transform.localScale = new Vector3 (planewidth / 10 + .001f,planewidth, height * planewidth / 10 + .001f);
							wall.transform.parent = parent.gameObject.transform;
							wall.name = "Wall";
						}
					}
					if (!positions.Contains (new Vector2 (l - 1, w))) { //If there is no floor west, create a wall west
						if ((l + w) % 2 == 0 && new Vector2 (l, w) != endPos)
							GenerateTorch (l - 0.5f, w, -90,torch,planewidth,height);
						if (l==0 &&new Vector2 (l, w) == endPos && hasGate==0) {
							n = 2;
							hasGate = 2;
							GameObject gate = (GameObject)Instantiate(resourceManager.Gate, new Vector3(endPos.x-0.55f, 0, endPos.y)*planewidth, Quaternion.Euler(0,90,0));
							gate.transform.localScale /= 1.9f;
						} else {
							n = 0;
						}
						for (int h = n; h<height; h++){
							GameObject wall = (GameObject)Instantiate (wallPrefab, new Vector3 ((l - 0.5f) * planewidth, (h+.5f) * planewidth, w * planewidth), Quaternion.Euler (90, 90, 0));
						//wall.gameObject.transform.localScale = new Vector3 (planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
						wall.transform.parent = parent.gameObject.transform;
						wall.name = "Wall";
						}

					}
					if (!positions.Contains (new Vector2 (l, w + 1))) { //If there is no floor north, create a wall north
						if ((l + w) % 2 == 1 && new Vector2 (l, w) != endPos)
							GenerateTorch (l, w + 0.5f, 0,torch,planewidth,height);
						if (w==width &&new Vector2 (l, w) == endPos && hasGate==0) {
							n = 2;
							hasGate = 3;
							GameObject gate = (GameObject)Instantiate(resourceManager.Gate, new Vector3(endPos.x, 0, endPos.y+0.55f)*planewidth, Quaternion.Euler(0,180,0));
							gate.transform.localScale /= 1.9f;
						} else {
							n = 0;
						}
						for (int h = n; h < height; h++) {
							GameObject wall = (GameObject)Instantiate (wallPrefab, new Vector3 (l * planewidth, (h+.5f) * planewidth, (w + 0.5f) * planewidth), Quaternion.Euler (-90, 0, 0));
							//wall.gameObject.transform.localScale = new Vector3 (planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
							wall.transform.parent = parent.gameObject.transform;
							wall.name = "Wall";
						}
					}
					if (!positions.Contains (new Vector2 (l, w - 1))) { //If there is no floor south, create a wall south
						if ((l + w) % 2 == 1 && new Vector2 (l, w) != endPos)
							GenerateTorch (l, w - 0.5f, 180,torch,planewidth,height);
						if (new Vector2 (l, w) == endPos && hasGate==0) {
							n = 2;
							hasGate = 4;
							GameObject gate = (GameObject)Instantiate(resourceManager.Gate, new Vector3(endPos.x, 0, endPos.y-0.55f)*planewidth, Quaternion.Euler(0,0,0));
							gate.transform.localScale /= 1.9f;
						} else {
							n = 0;
						}
						for (int h = n; h < height; h++) {
							GameObject wall = (GameObject)Instantiate (wallPrefab, new Vector3 (l * planewidth, (h + .5f) * planewidth, (w - 0.5f) * planewidth), Quaternion.Euler (90, 0, 0));
							//wall.gameObject.transform.localScale = new Vector3 (planewidth / 10 + .001f, height * planewidth, height * planewidth / 10 + .001f);
							wall.transform.parent = parent.gameObject.transform;
							wall.name = "Wall";
						}

					}
				}
			}
		}
	}

	//Method to spawn nodes
	public static List<WayPoint> SpawnNodes (ArrayList positions, float nodeSize, float planewidth, List<WayPoint> Nodes,int length, int width, bool drawNavigationGrid, bool isLevelEdMap, Vector3 endPos)
	{
        NodesPos = new List<Vector3>();

		for (int i = 0; i < positions.Count; i++) {
			Vector2 curPosi = (Vector2)positions [i];
			float l = curPosi [0];
			float w = curPosi [1];
			float nstart = 0;
			float estart = 0;
			float nend = 0;
			float eend = 0;

			if (!positions.Contains (new Vector2 (l - 1, w))) //If there is no floor south, start the nodes one nodeSize from the south wall. 
                nstart = nodeSize;
			if (!positions.Contains (new Vector2 (l + 1, w))) //If there is no floor north, end the nodes one nodeSize from the north wall
                nend = nodeSize;
			if (!positions.Contains (new Vector2 (l, w - 1)))
				estart = nodeSize;
			if (!positions.Contains (new Vector2 (l, w + 1)))
				eend = nodeSize;

			for (float n = nstart; n <= planewidth - nend; n = n + nodeSize) {

				for (float e = estart; e <= planewidth - eend; e = e + nodeSize) {
					Vector2 curPos = (Vector2)positions [i];                   
					Vector3 WPpos = new Vector3 (curPos [0] * planewidth + (n - planewidth / 2), 0, curPos [1] * planewidth + (e - planewidth / 2));

					if (!NodesPos.Contains (WPpos)) {
						NodesPos.Add (WPpos); //Add instantiated node position to NodePos
						Nodes.Add (new WayPoint (WPpos)); //Add Instantiated node as waypoint to Nodes
					}
                   
				}
			}
		}

		//Nodes at end position:
		for (float i = -nodeSize; i <= nodeSize; i+=nodeSize) {
			for (float j = 0; j < 6*nodeSize; j+=nodeSize) {
				if (hasGate == 3) {
					Vector3 WPpos = new Vector3 (endPos.x * planewidth + i, 0, (endPos.y + 0.5f) * planewidth + j);
					if (!NodesPos.Contains (WPpos)) {
						NodesPos.Add (WPpos);
						Nodes.Add (new WayPoint (WPpos));
					}
				}
				else if(hasGate == 1){
					Vector3 WPpos = new Vector3 ((endPos.x+0.5f) * planewidth + j, 0, (endPos.y) * planewidth + i);
					if (!NodesPos.Contains (WPpos)) {
						NodesPos.Add (WPpos);
						Nodes.Add (new WayPoint (WPpos));
					}
				}
				else if(hasGate == 2){
					Vector3 WPpos = new Vector3 ((endPos.x-0.5f) * planewidth - j, 0, (endPos.y) * planewidth + i);
					if (!NodesPos.Contains (WPpos)) {
						NodesPos.Add (WPpos);
						Nodes.Add (new WayPoint (WPpos));
					}
				}
				if (hasGate == 4) {
					Vector3 WPpos = new Vector3 (endPos.x * planewidth + i, 0, (endPos.y - 0.5f) * planewidth - j);
					if (!NodesPos.Contains (WPpos)) {
						NodesPos.Add (WPpos);
						Nodes.Add (new WayPoint (WPpos));
					}
				}
			}
		}


		float minOffset;
		float maxOffset;
		if (isLevelEdMap) {
			minOffset = -0.5f;
			maxOffset = width;
		} else {
			minOffset = -width / 2 - 0.5f;
			maxOffset = width / 2 + 1;
		}

		//remove nodes
		for (float l = -0.5f; l <= length; l++) {
			//for(float w = -0.5f;w<=width+1;w++){
			for (float w = minOffset; w <= maxOffset; w++) {
				int amountPlanesAround = 0;
				for (float x = -1; x <= 1; x += 2) {
					for (float y = -1; y <= 1; y += 2) {					
						amountPlanesAround += ConvertBool (positions.Contains (new Vector2 (l + x * 1 / 2, w + y * 1 / 2)));
					}
				}
				//Debug.Log (amountPlanesAround);
				if (amountPlanesAround == 3) {
					int index = NodesPos.IndexOf (new Vector3 (l * planewidth, 0, w * planewidth));
					NodesPos.RemoveAt (index);
					Nodes.RemoveAt (index);
					index = NodesPos.IndexOf (new Vector3 (l * planewidth, 0, w * planewidth));
					if (index >= 0) {
						NodesPos.RemoveAt (index);
						Nodes.RemoveAt (index);
					}
				}
			}
		}


	//}

	//Method to find all Nodes around a current node

//	public static void MakeNodeList (float nodeSize,List<Vector3> NodesPos,List<WayPoint> Nodes)
//	{
		//List of possible directions
		List<Vector3> directions = new List<Vector3> ();
		directions.Add (new Vector3 (-nodeSize, 0, -nodeSize));
		directions.Add (new Vector3 (-nodeSize, 0, nodeSize));
		directions.Add (new Vector3 (-nodeSize, 0, 0));
		directions.Add (new Vector3 (nodeSize, 0, nodeSize));
		directions.Add (new Vector3 (nodeSize, 0, -nodeSize));
		directions.Add (new Vector3 (nodeSize, 0, 0));
		directions.Add (new Vector3 (0, 0, -nodeSize));
		directions.Add (new Vector3 (0, 0, nodeSize));

		//checks if there are nodes in all directions
		for (int i = 0; i < NodesPos.Count; i++) {
			foreach (Vector3 dir in directions) {
				if (NodesPos.Contains (dir + (Vector3)NodesPos [i])) {
					int r = NodesPos.IndexOf (dir + (Vector3)NodesPos [i]);
					Nodes [i].AddNode (Nodes [r]);

                    if (drawNavigationGrid)
                    {
                        Debug.DrawLine((Vector3)NodesPos[i], dir + (Vector3)NodesPos[i], Color.green, 200f, false);
                    }
				}
			}
		}
		return Nodes;
	}

	//method to convert a bool from true to 1 or from false to zero.
	public static int ConvertBool (bool Bool)
	{
		if (Bool)
			return 1;
		else
			return 0;
	}

//	public static void RemoveNodes (ArrayList positions,int length, int width,float planewidth,List<Vector3> NodesPos,List<WayPoint> Nodes)
//	{
//
//	}



    //public static void createSingleObjects (float planewidth,GameObject enemySpawner, Vector2 endPos, Vector2 startPos)
    //{
    //    //Gate
    //    //GameObject GateObj = (GameObject)Instantiate (Gate, new Vector3 (-planewidth / 2, height * planewidth / 2, -planewidth / 2), Quaternion.identity);
    //    //GateObj.transform.localScale = new Vector3 (planewidth * 0.028f, planewidth * height / 150, planewidth);
    //}

    public static void spawnPlayer(GameObject player, GameObject camera, GameObject Goal2, GameObject enemySpawner, GameObject Gui, GameObject EventList, Vector2 startPos, Vector2 endPos, GameObject Minimapcamera, int width, int length, float planewidth)
	{			

		GameObject MainCamera = (GameObject)Instantiate (camera, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		MainCamera.name = "Main Camera";
        if (GameObject.Find("EventListener") != null)
        {
            GameObject EventListener = (GameObject)Instantiate(EventList, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
		GameObject Player = (GameObject)Instantiate (player, new Vector3 (startPos.x, 0.5f, startPos.y)* planewidth, Quaternion.identity);
		Player.gameObject.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
		Player.name = "Player";
        GameObject Goal = (GameObject)Instantiate(Goal2, new Vector3(startPos.x, 0.01f, startPos.y) * planewidth, Quaternion.identity);
        Goal.transform.name = "Goal";
		Vector3 enemyPos = new Vector3 ();
		if (hasGate == 1) {
			enemyPos = new Vector3 ((endPos.x + 1.5f) * planewidth, 0f, endPos.y * planewidth);
		} else if(hasGate == 2) {
			enemyPos = new Vector3 ((endPos.x - 1.5f) * planewidth, 0f, endPos.y * planewidth);
		} else if(hasGate == 3) {
			enemyPos = new Vector3 (endPos.x * planewidth, 0f, (endPos.y+1.5f) * planewidth);
		}else if(hasGate == 4) {
			enemyPos = new Vector3 (endPos.x * planewidth, 0f, (endPos.y-1.5f) * planewidth);
		}  

		enemySpawner = (GameObject)Instantiate(enemySpawner, enemyPos, Quaternion.identity); 
		GameObject gui = (GameObject)Instantiate (Gui, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		gui.transform.name = "GUIMain";
		//minimapcamera
		GameObject cam = (GameObject)Instantiate (Minimapcamera, new Vector3 (length / 2, Mathf.Max (width, length), 0) * planewidth, Quaternion.Euler (90, 0, 0));
		//cam.camera.rect = new Rect (0.8f, 0.7f, 0.3f, 0.3f);
		cam.camera.orthographicSize = 7.5f * planewidth;
		// create player and camera
		//Minimap camera

	}

}


