using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomMaze : MonoBehaviour
{

    public int length;
    public int width;
    public float planewidth;
    public float nodeSize;
    public int NumberOfPaths;

    public GameObject planePrefab;
    public GameObject wallPrefab;
    public GameObject node;
    public GameObject bidarraSpawnerPrefab;

    private ArrayList positions = new ArrayList();
    private List<Vector3> NodesPos = new List<Vector3>();
    public static List<WayPoint> Nodes = new List<WayPoint>();

	public static float planewidthS;
    //Use this for initialization
    void Awake()
    {
		planewidthS = planewidth;
        //Generate floors
        GenerateFloor();
        //Generate walls
        GenerateWall();
        SpawnNodes();
        //generate Nodes;
        DrawNodeLines();

    }

    //generate floors
    private void GenerateFloor()
    {
        float north;
        float east;
        float south;
        float west;
        float rnd;
        Vector2 startPos = new Vector2(0, 0);
        Vector2 endPos = new Vector2(length, 0);
        Vector4 lastPos = new Vector4(1, 1, 1, 1);


        int N = NumberOfPaths;
        for (int i = 0; i < N; i++)
        {
            Vector2 curPos = startPos;
            for (int ba = 0; ba < 300; ba++)
            {
                if (!curPos.Equals(endPos))
                {
                    if (!positions.Contains(curPos))
                    {
                        positions.Add(curPos); //add current position to arraylist
                        GameObject floor = (GameObject)Instantiate(planePrefab, new Vector3(curPos[0] * planewidth, 0, curPos[1] * planewidth), Quaternion.identity);
                        floor.gameObject.transform.localScale = new Vector3(planewidth, 0.1f, planewidth);
                        floor.transform.parent = gameObject.transform;
                        floor.name = "Floor";

                        if (ba % 2 == 0)
                        {
                            GameObject lightGameObject = new GameObject("Light");
                            lightGameObject.AddComponent<Light>();

                            lightGameObject.transform.position = new Vector3(curPos[0] * planewidth, 5, curPos[1] * planewidth);
                            lightGameObject.transform.parent = gameObject.transform;
                        }
                    }

                    //next position
                    north = 0.4f * ConvertBool(curPos[1] + 1 <= width / 2) * lastPos[3] / (1 + Vector2.Distance((curPos + new Vector2(0, 1)), endPos));
                    south = 0.4f * ConvertBool(curPos[1] - 1 >= -width / 2) * lastPos[0] / (1 + Vector2.Distance((curPos + new Vector2(0, -1)), endPos));
                    east = 0.7f * ConvertBool(curPos[0] + 1 <= length) * lastPos[2] / (1 + Vector2.Distance((curPos + new Vector2(1, 0)), endPos));
                    west = 0.0f * ConvertBool(curPos[0] > 0) * lastPos[1] / (1 + Vector2.Distance((curPos + new Vector2(-1, 0)), endPos));
                    rnd = Random.value * (north + south + east + west);

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
                else
                {
                    ba = 500;


                }

            }
        }

        GameObject floor2 = (GameObject)Instantiate(planePrefab, new Vector3(endPos[0] * planewidth, 0, endPos[1] * planewidth), Quaternion.identity);
        floor2.gameObject.transform.localScale = new Vector3(planewidth, 0.1f, planewidth);
        floor2.transform.parent = gameObject.transform;
        GameObject bidarraSpawner = (GameObject)Instantiate(bidarraSpawnerPrefab, new Vector3(endPos[0], 1.6f, endPos[1]) * planewidth, Quaternion.identity);
        bidarraSpawner.transform.parent = gameObject.transform;
        bidarraSpawner.name = "bidarraSpawner";

        positions.Add(endPos);

    }

    private void GenerateWall()
    {
        for (int l = 0; l <= length; l++)
        {
            for (int w = -width; w <= width; w++)
            {
                if (positions.Contains(new Vector2(l, w)))
                {
                    if (!positions.Contains(new Vector2(l + 1, w)))
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l + 0.5f) * planewidth, planewidth / 2, w * planewidth), Quaternion.Euler(0, 270, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";
                    }
                    if (!positions.Contains(new Vector2(l - 1, w)))
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3((l - 0.5f) * planewidth, planewidth / 2, w * planewidth), Quaternion.Euler(0, 90, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
                    if (!positions.Contains(new Vector2(l, w + 1)))
                    {
                        GameObject wall = (GameObject)Instantiate(wallPrefab, new Vector3(l * planewidth, planewidth / 2, (w + 0.5f) * planewidth), Quaternion.Euler(0, 180, 0));
                        wall.gameObject.transform.localScale = new Vector3(planewidth, planewidth, 0.1f);
                        wall.transform.parent = gameObject.transform;
                        wall.name = "Wall";

                    }
                    if (!positions.Contains(new Vector2(l, w - 1)))
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

    private void SpawnNodes()
    {
        float timeStart = Time.realtimeSinceStartup;
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 curPosi = (Vector2)positions[i];
            float l = curPosi[0];
            float w = curPosi[1];
            float nstart = 0;
            float estart = 0;
            float nend = 0;
            float eend = 0;

            if (!positions.Contains(new Vector2(l - 1, w)))
                nstart = nodeSize;
            if (!positions.Contains(new Vector2(l + 1, w)))
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
                    //node2.transform.parent = gameObject.transform;
                    //node2.name = "Node";
                    
                    if (!NodesPos.Contains(node2.transform.position))
                    {
                        NodesPos.Add(node2.transform.position);
                        Nodes.Add(new WayPoint(node2.transform.position));
                    }
                    else
                        Destroy(node2);
                }
            }
        }

        float timeTaken = Time.realtimeSinceStartup - timeStart;
        Debug.Log(timeTaken);
    }

    private void DrawNodeLines()
    {
        List<Vector3> directions = new List<Vector3>();
        directions.Add(new Vector3(-nodeSize, 0, -nodeSize));
        directions.Add(new Vector3(-nodeSize, 0, nodeSize));
        directions.Add(new Vector3(-nodeSize, 0, 0));
        directions.Add(new Vector3(nodeSize, 0, nodeSize));
        directions.Add(new Vector3(nodeSize, 0, -nodeSize));
        directions.Add(new Vector3(nodeSize, 0, 0));
        directions.Add(new Vector3(0, 0, -nodeSize));
        directions.Add(new Vector3(0, 0, nodeSize));


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

    private int ConvertBool(bool Bool)
    {
        if (Bool)
            return 1;
        else
            return 0;
    }

	public static float getPlaneWidth(){
		return planewidthS;
	}


}


