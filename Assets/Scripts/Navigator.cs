using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigator : MonoBehaviour {

    static float gridSize = RandomMaze.gridSize;

    /* DEBUG */
    //static float drawTime1;
    //static float drawTime2;
    //static float drawTime3;
    /* DEBUG */

    public static List<Vector3> Path(Vector3 startPoint, Vector3 endPoint) {
        /** INITIALIZATION **/

        /* DEBUG */
        //float startTime = Time.realtimeSinceStartup;

        //drawTime1 = 0;
        //drawTime2 = 0;
        //drawTime3 = 0;

        //float temp = Time.realtimeSinceStartup;
        Debug.DrawLine(startPoint, endPoint, Color.yellow, Mathf.Infinity, false);
        //drawTime1 = Time.realtimeSinceStartup - temp;
        /* DEBUG */

        // Load the grid
        List<WayPoint> grid = RandomMaze.Nodes;
        // Create a new (empty) route
        List<Vector3> path = new List<Vector3>();
        // Create a new waypoint for the starting and end position
        WayPoint startWP = new WayPoint(startPoint, "closed");
        startWP.setCost(0);
        WayPoint endWP = new WayPoint(endPoint);

        // Find the nearest possible destination nodes and add them to the destinations of the starting node

        // Do this by rounding the current coordinates to the grid step size, thus finding the 4 nearest nodes
        float start_x = startPoint.x;
        float start_z = startPoint.z;

        // Add small noise to the coordinates if they are already exactly on the grid, ensuring 8 unique locations.
        if(start_x % gridSize == 0) {
            start_x += 0.1f * Random.value;
        }
        else if(start_z % gridSize == 0) {
            start_z += 0.1f * Random.value;
        }
        // Find surrounding node coordinates
        float x1 = RoundUp(start_x, gridSize);
        float x2 = RoundDown(start_x, gridSize);
        float z1 = RoundUp(start_z, gridSize);
        float z2 = RoundDown(start_z, gridSize);

        // Add potential node locations to a list.
        List<Vector3> startNodes = new List<Vector3>();
        startNodes.Add(new Vector3(x1, 0, z1));
        startNodes.Add(new Vector3(x1, 0, z2));
        startNodes.Add(new Vector3(x2, 0, z1));
        startNodes.Add(new Vector3(x2, 0, z2));

        // Add found nodes to destination list of start node if they are visible and set their state to open
        bool openDestinationsExist = false;
        for(int i = 0; i < startNodes.Count; i++) {
            if(startPoint != startNodes[i]) {
                if(!Physics.Raycast(startPoint, startNodes[i] - startPoint, (startPoint - startNodes[i]).magnitude)) {
                    // Add node to destination if it's reachable
                    WayPoint newDest = FindWayPointAt(startNodes[i], grid);
                    newDest.setCost(CalculateGCost(startWP, newDest));
                    newDest.setState("open");
                    startWP.AddNode(newDest);
                    // There are still open destinations
                    openDestinationsExist = true;
                }
            }
        }

        // Our current WP is the starting WP
        WayPoint currentWP = startWP;
        path.Add(currentWP.getPosition());

        /** ROUTE CALCULATION **/

        while(openDestinationsExist) {
            // Set the cheapest possible destination as the next destination
            // Initialize cheapest point so far to be as expensive as possible
            
            float cheapest = float.MaxValue;
            // Find the cheapest destination
            WayPoint cheapestWP = new WayPoint(new Vector3(0, 0, 0));
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint destination = currentWP.getDestinations()[i];
                if(destination.getState() == "open") {
                    float fCost = CalculateFCost(currentWP, destination, endWP);
                    if(fCost <= cheapest) {
                        cheapest = fCost;
                        cheapestWP = destination;
                    }
                }
            }
            if(cheapest == float.MaxValue) {
                Debug.LogError("Error: route stuck with no destinations, empty path returned");
                break;
            }

            // Move to new WP and set it as closed
            cheapestWP.setPrevious(currentWP);
            currentWP = cheapestWP;
            currentWP.setState("closed");

            /* DEBUG */
            //temp = Time.realtimeSinceStartup;
            //Debug.DrawLine(cheapestWP.getPosition(), cheapestWP.getPrevious().getPosition(), Color.red, Mathf.Infinity, false);
            //drawTime2 += temp - Time.realtimeSinceStartup;
            /* DEBUG */

            // If the current waypoint is the endpoint, stop searching and build the route
            if(CloseEnoughToDestination(currentWP, endPoint)) {
                endWP.setPrevious(currentWP);
                path = ReconstructPath(startPoint, endWP);
                openDestinationsExist = false;
                break;
            }

            // Find unexplored nodes and open them if they seem useful
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint neighbour = currentWP.getDestinations()[i];
                // Don't even try if it's closed
                if(!(neighbour.getState() == "closed")) {
                    float potential_g_Cost = CalculateGCost(currentWP, neighbour);
                    // If a cheaper g cost is found via the current WayPoint, update it
                    if(neighbour.getState() == "open" && potential_g_Cost < neighbour.getCost()) {
                        neighbour.setCost(potential_g_Cost);
                        neighbour.setPrevious(currentWP);
                    }
                    if(neighbour.getState() == "unexplored") {
                        neighbour.setCost(CalculateGCost(currentWP, neighbour));
                        neighbour.setState("open");
                        neighbour.setPrevious(currentWP);
                    }
                }
            }
        }
        /* DEBUG */
        //float timeSpent = Time.realtimeSinceStartup - startTime - drawTime1 - drawTime2;
        //Debug.Log("Time spent calculating:");
        //Debug.Log(timeSpent);
        /* DEBUG */
        return path;
    }

    /** FUNCTIONS **/

    // function that rounds up a certain number to the grid size
    static float RoundUp(float toRound, float nearest) {
        float res;
        if(toRound % nearest != 0) {
            res = nearest - toRound % nearest + toRound;
            if(toRound < 0) {
                res -= nearest;
            }
        }
        else {
            res = toRound;
        }
        return res;
    }
    // Function that rounds down a certain number to the grid size
    static float RoundDown(float toRound, float nearest) {
        float res;
        if(toRound % nearest != 0) {
            res = toRound - toRound % nearest;
            if(toRound < 0) {
                res -= nearest;
            }
        }
        else {
            res = toRound;
        }
        return res;
    }

    // Function that checks if the given waypoint is close enough to the end point to navigate to it without using the grid
    static bool CloseEnoughToDestination(WayPoint wp, Vector3 end) {
        Vector3 pos = wp.getPosition();
        if(pos == end) {
            return true;
        } 
        else if((!Physics.Raycast(pos, end - pos, (pos - end).magnitude)) && Mathf.Abs(pos.x - end.x) <= gridSize && Mathf.Abs(pos.z - end.z) <= gridSize) {
            return true;
        }
        return false;
    }
    // Function that calculates the Manhattan distance between two points
    static float ManhattanDist(Vector3 p1, Vector3 p2) {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z);
    }
    // Function that can reconstruct the path from an A* route
    static List<Vector3> ReconstructPath(Vector3 startPos, WayPoint endWP) {
        List<Vector3> bestPath = new List<Vector3>();
        WayPoint currWP = endWP;
        bestPath.Add(endWP.getPosition());
        while(true) {
            /* DEBUG */
            //float temp = Time.realtimeSinceStartup;
            Debug.DrawLine(currWP.getPosition(), currWP.getPrevious().getPosition(), Color.blue, Mathf.Infinity, false);
            //drawTime3 += Time.realtimeSinceStartup - temp;
            /* DEBUG */
            currWP = currWP.getPrevious();
            if(currWP.getPosition() == startPos) {
                bestPath.Insert(0,startPos);
                break;
            }
            bestPath.Add(currWP.getPosition());
        }
        return bestPath;
    }
    // Function that calculates the g-cost between two waypoints (cost based on distance from start point)
    static float CalculateGCost(WayPoint current, WayPoint destination) {
        return current.getCost() + (current.getPosition() - destination.getPosition()).magnitude;
    }
    // Function that calculates the f-cost between two waypoints (cost based on distance from both start and end point)
    static float CalculateFCost(WayPoint current, WayPoint destination, WayPoint endpoint) {
        // Distance from current node to destination
        float g_cost = CalculateGCost(current, destination);

        // Heuristic, in our case the Manhattan distance
        float h_cost = ManhattanDist(destination.getPosition(), endpoint.getPosition());
        return g_cost + h_cost;
    }
    // Function that can find a Waypoint at a certain location
    static WayPoint FindWayPointAt(Vector3 position, List<WayPoint> grid) {
        foreach(WayPoint wp in grid) {
            if(wp.getPosition() == position) {
                return wp;
            }
        }
        Debug.LogError("No waypoint exists at given position, returning null!");
        return null;
    }

}
