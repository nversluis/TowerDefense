using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Astar : MonoBehaviour {

    List<Vector3> castDirections;
    public float step;

    class Node
    {
        Vector3 pos;
        string state = "unexplored";
        float cost = 0;
        Node prevNode;

        List<Node> destinations = new List<Node>();
        List<Node> potentialPrevNodes = new List<Node>();

        public Node(Vector3 pos,string state = "unexplored")
        {
            this.pos = pos;
            this.state = state;
        }

        public Vector3 GetPos()
        {
            return pos;
        }

        public string GetState()
        {
            return state;
        }

        public float GetCost()
        {
            return cost;
        }

        public Node GetPrevNode(){
            return prevNode;
        }

        public List<Node> GetDestinations()
        {
            return destinations;
        }

        public List<Node> GetPotentialPrevNodes()
        {
            return potentialPrevNodes;
        }

        public void SetPrevNode(Node node)
        {
            prevNode = node;
        }

        public void SetState(string state)
        {
            this.state = state;
        }

        public void SetCost(float cost)
        {
            this.cost = cost;
        }

        public void AddDestination(Node node)
        {
            destinations.Add(node);
        }

        public void AddPotentialPrevPoint(Node node)
        {
            potentialPrevNodes.Add(node);
        }


    }

    public List<Vector3> Path(Vector3 startpos, Vector3 endpos)
    {
        float endDist = Vector3.Distance(startpos, endpos);
        if (!Physics.Raycast(startpos, endpos - startpos, endDist))
        {
            List<Vector3> path = new List<Vector3>();
            path.Add(startpos);
            path.Add(endpos);
            return path;
        }

        GameObject[] waypointlist = GameObject.FindGameObjectsWithTag("waypoint");
        List<Node> waypoints = new List<Node>();

        foreach (GameObject waypoint in waypointlist)
        {
            Node curNode = new Node(waypoint.transform.position);
            waypoints.Add(curNode);
        }

        Node endNode = new Node(endpos,"end");

        foreach (Node node in waypoints)
        {
            float distance2 = (node.GetPos() - endpos).magnitude;
            if (!Physics.Raycast(node.GetPos(), endpos - node.GetPos(), distance2))
            {
                node.AddDestination(endNode);
            }

            else
            {
                for (int i = 0; i < 8; i++)
                {
                    float distance1 = (node.GetPos() - castDirections[i]).magnitude;
                    if (!Physics.Raycast(node.GetPos(), node.GetPos() + castDirections[i], distance1))
                    {
                        node.AddDestination(new Node(node.GetPos() + castDirections[i]));
                    }
                }
            }
        }

        Node startNode = new Node(startpos, "start");
        for (int i = 0; i < 8; i++)
        {
            float distance = (startpos - castDirections[i]).magnitude;
            if (!Physics.Raycast(startpos, castDirections[i] - startpos, distance))
            {

            }
        }

        bool done = false;

        while (true)
        {
            List<Node> unexplored = new List<Node>();

            foreach (Node node in waypoints)
            {
                if(node.GetState() == "open")
                {
                    foreach (Node node2 in node.GetDestinations())
                    {
                        if (node2.GetState() == "unexplored")
                        {
                            node2.AddPotentialPrevPoint(node);
                            unexplored.Add(node2);
                            node2.SetCost((startpos + node2.GetPos()).magnitude + (endpos + node2.GetPos()).magnitude);
                        }
                        else if (node2.GetPos().Equals(endNode.GetPos()))
                        {
                            endNode.AddDestination(node);
                            done = true;
                            break;
                        }
                    }
                    node.SetState("closed");
                }
            }

            foreach (Node u_node in unexplored)
            {
                u_node.SetState("open");
                float lowScore = float.MaxValue;
                Node bestPrevNode = null;

                foreach (Node prevNodes in u_node.GetPotentialPrevNodes())
                {

                    if (lowScore > prevNodes.GetCost())
                    {
                        lowScore = prevNodes.GetCost();
                        bestPrevNode = prevNodes;
                    }
                }

                u_node.SetPrevNode(bestPrevNode);
            }

            if (done)
            {
                List<Node> AstarPath = null;
                float lowScore = float.MaxValue;

                foreach (Node node in endNode.GetDestinations())
                {

                }
            }
        }
    }



	// Use this for initialization
	void Start () {
        castDirections = new List<Vector3>();
        castDirections.Add(new Vector3(step, 0, 0));
        castDirections.Add(new Vector3(-step, 0, 0));
        castDirections.Add(new Vector3(0, 0, step));
        castDirections.Add(new Vector3(0, 0, -step));
        castDirections.Add(new Vector3(step, 0, step));
        castDirections.Add(new Vector3(step, 0, -step));
        castDirections.Add(new Vector3(-step, 0, step));
        castDirections.Add(new Vector3(-step, 0, -step));

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
