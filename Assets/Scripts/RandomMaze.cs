using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generates the map
public class RandomMaze : MonoBehaviour
{

    public int length; //Length of the map
    public int width; //With of the map
    public float planewidth; //Size of the planes
	public float height;
    public float nodeSize; //Distance between nodepoints
    public int NumberOfPaths; //Number of paths to the end

    public GameObject planePrefab; //Floor prefab
    public GameObject wallPrefab; //Wall prefab
    public GameObject node; //Node prefab
    public GameObject EnemySpawner;
    //public GameObject bidarraSpawnerPrefab; //Prefab to spawn enemy´s

    private ArrayList positions = new ArrayList(); //Positions of the floors
    private List<Vector3> NodesPos = new List<Vector3>(); //Positions of the waypoints/nodes
    public static List<WayPoint> Nodes = new List<WayPoint>(); //List with all Nodes

	public static float planewidthS; //Variable to share planewidth in other scripts
    public static float gridSize;

    /* NAVIGATOR TEST CODE */
    //static GameObject enemy;
    //static Transform enemyTr;
    static List<Vector3> testPath;
    static Vector3 start;
    static Vector3 end;
    //static Vector3 prevPos;
    //static Vector3 currPos;
    //static float startTime;
    //static float speed = .05f;
    //static float navLength;
    //static float smooth = .5f;
    //static int i;
    /* NAVIGATOR TEST CODE */

    //Use this for initialization
    void Awake()
    {
        gridSize = nodeSize;
		planewidthS = planewidth;
        //Generate floors
        GenerateFloor();
        //Generate walls
        GenerateWall();
        SpawnNodes();
        //generate Nodes;
       	MakeNodeList();
    }

    void Start() {
        /* NAVIGATOR TEST CODE */
        //start = Nodes[4].getPosition();
        //start.x += Random.value;
        //start.z += Random.value;

        //int val = (int)Mathf.Ceil(Nodes.Count / 2);
        //end = Nodes[val].getPosition();
        //end.x += Random.value;
        //end.z += Random.value;

        //testPath = Navigator.Path(start, end);

        //Debug.Log(start);
        //Debug.Log(end);
        //for (int ii = 0; ii < testPath.Count; ii++)
        //{
        //    Debug.Log("Path position " + ii + " =" + testPath[ii]);
        //}

        //enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //enemyTr = enemy.GetComponent<Transform>();

        //enemyTr.position = start;
        //currPos = start;
        //i = 1;
        /* NAVIGATOR TEST CODE */
    }

    void Update() {
    /* NAVIGATOR TEST CODE */
    //    if(Time.realtimeSinceStartup > 2 && i < testPath.Count){
    //        navLength = (testPath[i - 1] - testPath[i]).magnitude;
    //        Debug.Log("Section length = " + navLength);
    //        float distTravelled = (Time.time - startTime) * speed;
    //        float fracJourney = distTravelled / navLength;
    //        prevPos = currPos;
    //        currPos = Vector3.Lerp(prevPos, testPath[i], fracJourney);
    //        enemyTr.position = currPos;
    //        Debug.Log("fracJourney = " + fracJourney);
    //        if(fracJourney == 1) {
    //            Debug.Log("Position " + i + " is:");
    //            Debug.Log(testPath[i]);
    //            startTime = Time.time;
    //            i++;
    //        }
    //    }
    //    else {
    //        startTime = Time.time;
    //    }
    }
    /* NAVIGATOR TEST CODE */

    //generate floors
    private void GenerateFloor()
    {
        float north;
        float east;
        float south;
        float west;
        float rnd;
        Vector2 startPos = new Vector2(0, 0); //Start position where enemy´s spawn
		Vector2 endPos = new Vector2(length, 0); //End position where enemy´s go
        Vector4 lastPos = new Vector4(1, 1, 1, 1); //position where the map last was
	

        int N = NumberOfPaths;
        for (int i = 0; i < N; i++) //run this the amount of timres of the numper of paths
        {
            Vector2 curPos = startPos; //current position is start position
            for (int ba = 0; ba < 300; ba++) //if it takes longer than 300 steps, stop 
            {
                if (!curPos.Equals(endPos)) //Continue to run if curPos is not equal to the end position
                {
                    if (!positions.Contains(curPos)) //Continue only if curPos is not yet in positions
                    {
                        positions.Add(curPos); //add current position to arraylist
                        GameObject floor = (GameObject)Instantiate(planePrefab, new Vector3(curPos[0] * planewidth, 0, curPos[1] * planewidth), Quaternion.identity); //Instantiate a floor at current position
                        floor.gameObject.transform.localScale = new Vector3(planewidth/10, 0.1f, planewidth/10); //Scale the floor
                        floor.transform.parent = gameObject.transform; //Set the floor to the gameObject.
                        floor.name = "Floor"; //name the floor Floor
						GameObject ceil = (GameObject)Instantiate(planePrefab, new Vector3(curPos[0] * planewidth, height*planewidth, curPos[1] * planewidth), Quaternion.identity); //Instantiate a floor at current position
						ceil.transform.Rotate (new Vector3 (180, 0, 0));
						ceil.gameObject.transform.localScale = new Vector3(planewidth/10, 0.1f, planewidth/10); //Scale the floor

                        if (ba % 2 == 0) //if ba is even generate a light at current position
                        {
                            GameObject lightGameObject = new GameObject("Light");
                            lightGameObject.AddComponent<Light>();

                            lightGameObject.transform.position = new Vector3(curPos[0] * planewidth, 5, curPos[1] * planewidth);
                            lightGameObject.transform.parent = gameObject.transform;
                        }
                    }

                    //next position
					//Is biased to go east. Can go west
					//Second term is to keep it in bounds [0, length], [-width,width]/2
					//Third term is to make sure you can´t go back, and it´s more likely to go the same way as the last time.
					//If it´s closer to end, biased to go that way, last term
                    north = 0.4f * ConvertBool(curPos[1] + 1 <= width / 2) * lastPos[3] / (1 + Vector2.Distance((curPos + new Vector2(0, 1)), endPos));
                    south = 0.4f * ConvertBool(curPos[1] - 1 >= -width / 2) * lastPos[0] / (1 + Vector2.Distance((curPos + new Vector2(0, -1)), endPos));
                    east = 0.7f * ConvertBool(curPos[0] + 1 <= length) * lastPos[2] / (1 + Vector2.Distance((curPos + new Vector2(1, 0)), endPos));
					west = 0.0f;
                    rnd = Random.value * (north + south + east + west); //random value

                    if (rnd < north)
                    { //go north
                        curPos[1]++;
                        lastPos *= 0;
                        lastPos += new Vector4(0, 1, 1, 2);
                    }
                    else if (rnd < north + east)
                    { //go east
                        curPos[0]++;
                        lastPos *= 0;
                        lastPos += new Vector4(1, 0, 2, 1);
                    }

                    else if (rnd < north + east + south)
                    { //go south
                        curPos[1]--;
                        lastPos *= 0;
                        lastPos += new Vector4(2, 1, 1, 0);
                    }
                    else
                    { //go west
                        curPos[0]--;
                        lastPos *= 0;
                        lastPos += new Vector4(1, 2, 0, 1);
                    }
                }
                else //if you cant continue, just stop
                {
                    ba = 500;


                }

            }

        }

        GameObject floor2 = (GameObject)Instantiate(planePrefab, new Vector3(endPos[0] * planewidth, 0, endPos[1] * planewidth), Quaternion.identity); //Generate floor at end position
		floor2.gameObject.transform.localScale = new Vector3(planewidth/10, planewidth/10, planewidth/10);

        floor2.transform.parent = gameObject.transform;
        //GameObject bidarraSpawner = (GameObject)Instantiate(bidarraSpawnerPrefab, new Vector3(endPos[0], 1.6f, endPos[1]) * planewidth, Quaternion.identity); //Spawn bidarraSpawner
        //bidarraSpawner.transform.parent = gameObject.transform;
        //bidarraSpawner.name = "bidarraSpawner";

        positions.Add(endPos); //Add the end position to position
        GameObject enemySpawner = (GameObject)Instantiate(EnemySpawner, new Vector3(endPos.x*planewidth, 0f, endPos.y*planewidth), Quaternion.identity);    

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
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l + 0.5f) * planewidth, height*planewidth / 2, w * planewidth), Quaternion.Euler(90, -90, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth/10, height*planewidth, height*planewidth/10);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";
                    }
                    if (!positions.Contains(new Vector2(l - 1, w))) //If there is no floor west, create a wall west
                    {
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l - 0.5f) * planewidth, height*planewidth / 2, w * planewidth), Quaternion.Euler(90, 90, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth/10, height*planewidth, height*planewidth/10);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
					if (!positions.Contains(new Vector2(l, w + 1))) //If there is no floor north, create a wall north
                    {
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, height*planewidth / 2, (w + 0.5f) * planewidth), Quaternion.Euler(-90, 0, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth/10, height*planewidth,height*planewidth/10);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
					if (!positions.Contains(new Vector2(l, w - 1))) //If there is no floor south, create a wall south
                    {
						GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, height*planewidth / 2, (w - 0.5f) * planewidth), Quaternion.Euler(90, 0, 0));
						wall.gameObject.transform.localScale = new Vector3(planewidth/10, height*planewidth, height*planewidth/10);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
                }
            }
        }
    }

	//Method to spawn nodes
    private void SpawnNodes()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 curPosi = (Vector2)positions[i];
            float l = curPosi[0];
            float w = curPosi[1];
            float nstart = 0;
            float estart = 0;
            float nend = 0;
            float eend = 0;

            if (!positions.Contains(new Vector2(l - 1, w))) //If there is no floor south, start the nodes one nodeSize from the south wall. 
                nstart = nodeSize;
            if (!positions.Contains(new Vector2(l + 1, w))) //If there is no floor north, end the nodes one nodeSize from the north wall
                nend = nodeSize;
            if (!positions.Contains(new Vector2(l, w - 1)))
                estart = nodeSize;
            if (!positions.Contains(new Vector2(l, w + 1)))
                eend = nodeSize;

            for (float n = nstart; n <= planewidth - nend; n = n + nodeSize)
            {

                for (float e = estart; e <= planewidth - eend; e = e + nodeSize)
                {
                    Vector2 curPos = (Vector2)positions[i];
                    GameObject node2 = (GameObject)Instantiate(node, new Vector3(curPos[0] * planewidth + (n - planewidth/2), 0, curPos[1] * planewidth + (e - planewidth/2)), Quaternion.identity);
                    
                    if (!NodesPos.Contains(node2.transform.position))
                    {
                        NodesPos.Add(node2.transform.position); //Add instantiated node position to NodePos
                        Nodes.Add(new WayPoint(node2.transform.position)); //Add Instantiated node as waypoint to Nodes
                    }
                    else
                        Destroy(node2); //Destroy the node if there already is a node at that position
                }
            }
        }
		RemoveNodes ();
    }

	//Method to find all Nodes around a current node

    private void MakeNodeList()
    {
		//List of possible directions
        List<Vector3> directions = new List<Vector3>();
        directions.Add(new Vector3(-nodeSize, 0, -nodeSize));
        directions.Add(new Vector3(-nodeSize, 0, nodeSize));
        directions.Add(new Vector3(-nodeSize, 0, 0));
        directions.Add(new Vector3(nodeSize, 0, nodeSize));
        directions.Add(new Vector3(nodeSize, 0, -nodeSize));
        directions.Add(new Vector3(nodeSize, 0, 0));
        directions.Add(new Vector3(0, 0, -nodeSize));
        directions.Add(new Vector3(0, 0, nodeSize));

		//checks if there are nodes in all directions
        for (int i = 0; i < NodesPos.Count; i++) 
        {
            foreach (Vector3 dir in directions)
            {
                if (NodesPos.Contains(dir + (Vector3)NodesPos[i]))
                {
                    int r = NodesPos.IndexOf(dir + (Vector3)NodesPos[i]);
                    Nodes[i].AddNode(Nodes[r]);
                    Debug.DrawLine((Vector3)NodesPos[i], dir + (Vector3)NodesPos[i], Color.green, 200f, false);

                }
            }
        }
    }

	//method to convert a bool from true to 1 or from false to zero.
    private int ConvertBool(bool Bool)
    {
        if (Bool)
            return 1;
        else
            return 0;
    }

	public void RemoveNodes(){
		for(float l=-0.5f;l<=length;l++){
			for (float w = -width / 2-0.5f; w <= width / 2 + 1; w ++) {
				int amountPlanesAround = 0;
				for(float x=-1;x<=1;x+=2){
					for(float y=-1;y<=1;y+=2){
						//Debug.Log(new Vector4(l,x,w,y));
						//Debug.Log(new Vector3(l+x,w+y,ConvertBool(positions.Contains(new Vector2(l+x/2,w+y/2)))));
						//Debug.Log(new Vector2(x,y));
						amountPlanesAround+=ConvertBool(positions.Contains(new Vector2(l+x*1/2,w+y*1/2)));
					}
				}
				//Debug.Log (amountPlanesAround);
				if (amountPlanesAround == 3) {
					int index =NodesPos.IndexOf (new Vector3(l*planewidth, 0, w*planewidth));
					NodesPos.RemoveAt (index);
					Nodes.RemoveAt (index);

				}
			}
		}
	}




	//method to retrun the planewidth for use in other scripts
	public static float getPlaneWidth(){
		return planewidthS;
	}





}


