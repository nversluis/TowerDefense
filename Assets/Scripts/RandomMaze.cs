﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generates the map
public class RandomMaze : MonoBehaviour
{

    public int length; //Length of the map
    public int width; //With of the map
    public float planewidth; //Size of the planes
    public float nodeSize; //Distance between nodepoints
    public int NumberOfPaths; //Number of paths to the end

    public GameObject planePrefab; //Floor prefab
    public GameObject wallPrefab; //Wall prefab
    public GameObject node; //Node prefab
    //public GameObject bidarraSpawnerPrefab; //Prefab to spawn enemy´s

    private ArrayList positions = new ArrayList(); //Positions of the floors
    private List<Vector3> NodesPos = new List<Vector3>(); //Positions of the waypoints/nodes
    public static List<WayPoint> Nodes = new List<WayPoint>(); //List with all Nodes

	public static float planewidthS; //Variable to share planewidth in other scripts
    public static float gridSize;

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
        DrawNodeLines();
        //Navigator.Test();
        Vector3 start = Nodes[4].getPosition();
        start.x += Random.value;
        start.z += Random.value;
        Debug.Log("start: ");
        Debug.Log(start);
        Vector3 end = Nodes[20].getPosition();
        end.x += Random.value;
        end.z += Random.value;
        Debug.Log("end: ");
        Debug.Log(end);
        List<Vector3> testPath = Navigator.Path(start, end);
        Debug.Log("Path found!");
        Debug.Log(testPath);
    }

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
                        floor.gameObject.transform.localScale = new Vector3(planewidth, 0.1f, planewidth); //Scale the floor
                        floor.transform.parent = gameObject.transform; //Set the floor to the gameObject.
                        floor.name = "Floor"; //name the floor Floor

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
        floor2.gameObject.transform.localScale = new Vector3(planewidth, 0.1f, planewidth);
        floor2.transform.parent = gameObject.transform;
        //GameObject bidarraSpawner = (GameObject)Instantiate(bidarraSpawnerPrefab, new Vector3(endPos[0], 1.6f, endPos[1]) * planewidth, Quaternion.identity); //Spawn bidarraSpawner
        //bidarraSpawner.transform.parent = gameObject.transform;
        //bidarraSpawner.name = "bidarraSpawner";

        positions.Add(endPos); //Add the end position to position

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
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l + 0.5f) * planewidth, planewidth / 2, w * planewidth), Quaternion.Euler(0, 270, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";
                    }
                    if (!positions.Contains(new Vector2(l - 1, w))) //If there is no floor west, create a wall west
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l - 0.5f) * planewidth, planewidth / 2, w * planewidth), Quaternion.Euler(0, 90, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
					if (!positions.Contains(new Vector2(l, w + 1))) //If there is no floor north, create a wall north
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, planewidth / 2, (w + 0.5f) * planewidth), Quaternion.Euler(0, 180, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
					if (!positions.Contains(new Vector2(l, w - 1))) //If there is no floor south, create a wall south
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, planewidth / 2, (w - 0.5f) * planewidth), Quaternion.Euler(0, 0, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
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
    }

	//Method to find all Nodes around a current node

    private void DrawNodeLines()
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

	//method to retrun the planewidth for use in other scripts
	public static float getPlaneWidth(){
		return planewidthS;
	}


}


