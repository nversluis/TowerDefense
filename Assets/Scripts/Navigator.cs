using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigator : MonoBehaviour {

    // Create a list containing the open Nodes
    static List<WayPoint> openNodes = new List<WayPoint>();
    // Retrieve gridSize
    static float gridSize = RandomMaze.gridSize;
    // Make a layer mask for the ray casts
    static LayerMask layerMask = 1<<10;
    // The higher the D-factor, the faster, but less accurate the algorithm becomes
    public static float D = 0.15f;

    /* DEBUG */
    static float drawTime1;
    static float drawTime2;
    static float drawTime3;
    /* DEBUG */

    public static List<Vector3> Path(Vector3 startPoint, Vector3 endPoint) {
        /** INITIALIZATION **/

        /* DEBUG */
        //Debug.Log("Start point: " + startPoint);

        //List<WayPoint> openNodesClone = openNodes;

        float startTime = Time.realtimeSinceStartup;

        drawTime1 = 0;
        drawTime2 = 0;
        drawTime3 = 0;

        float temp = Time.realtimeSinceStartup;
        Debug.DrawLine(startPoint, endPoint, Color.yellow, Mathf.Infinity, false);
        drawTime1 = Time.realtimeSinceStartup - temp;
        /* DEBUG */

        // Load the grid and clear any leftovers from possible previous navigation attempts.
        List<WayPoint> grid = RandomMaze.Nodes;
        foreach(WayPoint wp in grid) {
            wp.setState("unexplored");
            wp.setGCost(0);
            wp.setFCost(0);
            wp.setPrevious(null);
        }

        openNodes.Clear();

        // Create a new (empty) route
        List<Vector3> path = new List<Vector3>();
        // Create a new waypoint for the starting and end position
        WayPoint startWP = new WayPoint(startPoint, "closed");
        startWP.setGCost(0);
        WayPoint endWP = new WayPoint(endPoint);

        // Find the nearest possible destination nodes and add them to the destinations of the starting node

        // Do this by rounding the current coordinates to the grid step size, thus finding the 4 nearest nodes
        float start_x = startPoint.x;
        float start_z = startPoint.z;

        // Add small noise to the coordinates if they are already exactly on the grid, ensuring unique locations.
        if((start_x % gridSize) == 0) {
            start_x += 0.01f * Random.value;
        }
        else if((start_z % gridSize) == 0) {
            start_z += 0.01f * Random.value;
        }

        /* DEBUG */
        //Debug.Log("First node position =" + grid[0].getPosition());
        /* DEBUG */
        
        // Find surrounding node coordinates
        float x1 = RoundUp(start_x, gridSize);
        float x2 = RoundDown(start_x, gridSize);
        float z1 = RoundUp(start_z, gridSize);
        float z2 = RoundDown(start_z, gridSize);

        // Compensate for offset

        float x_offset = Mathf.Abs(grid[0].getPosition().x % gridSize);
        float z_offset = Mathf.Abs(grid[0].getPosition().z % gridSize);
        
        /* DEBUG */
        //Debug.Log("X offset =" + x_offset);
        //Debug.Log("Z offset =" + z_offset);
        /* DEBUG */

        x1 += x_offset;
        x2 += x_offset;
        z1 += z_offset;
        z2 += z_offset;

        // Add potential node locations to a list.
        List<Vector3> startNodes = new List<Vector3>();
        startNodes.Add(new Vector3(x1, 0, z1));
        startNodes.Add(new Vector3(x1, 0, z2));
        startNodes.Add(new Vector3(x2, 0, z1));
        startNodes.Add(new Vector3(x2, 0, z2));

        /* DEBUG */
        //Debug.Log("There are " + startNodes.Count + " locations to look for nodes:");
        for(int i = 0; i < startNodes.Count; i++) {
            //Debug.Log("Node " + i + ": " + startNodes[i]);
            Debug.DrawLine(startPoint, startNodes[i], Color.white, Mathf.Infinity, false);
        }
        /* DEBUG */
        // Add found nodes to destination list of start node if they are visible and set their state to open
        bool openDestinationsExist = false;
        for(int i = 0; i < startNodes.Count; i++) {
            if(startPoint != startNodes[i] && !Physics.Raycast(startPoint, startNodes[i] - startPoint, (startPoint - startNodes[i]).magnitude + .1f, layerMask)) {
                // Add node to destination if it's reachable
                WayPoint newDest = FindWayPointAt(startNodes[i], grid);
                newDest.setGCost(CalculateGCost(startWP, newDest));
                newDest.setFCost(CalculateFCost(startWP, newDest, endWP));
                newDest.setPrevious(startWP);
                newDest.setState("open");
                startWP.AddNode(newDest);
                AddToOpenNodes(newDest);
                // There are still open destinations
                openDestinationsExist = true;
            }
        }
        // Our current WP is the starting WP
        WayPoint currentWP = startWP;
        path.Add(currentWP.getPosition());

        /** ROUTE CALCULATION **/
        while(openDestinationsExist) {

            // There should not be a scenario in which there are no more open destinations, but still:            
            if(openNodes.Count == 0) {
                Debug.LogError("Error: route stuck with no destinations, empty path returned");
                return null;
            }

            // Move to cheapest WP and set it as closed
            /* DEBUG */
            WayPoint previousWP = currentWP;
            /* DEBUG */
            currentWP = openNodes[0];
            currentWP.setState("closed");
            openNodes.Remove(currentWP);

            /* DEBUG */
            //temp = Time.realtimeSinceStartup;
            //Debug.DrawLine(currentWP.getPosition(), previousWP.getPosition(), Color.red, Mathf.Infinity, false);
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
                    if(neighbour.getState() == "open" && potential_g_Cost < neighbour.getGCost()) {
                        neighbour.setGCost(potential_g_Cost);
                        neighbour.setFCost(CalculateFCost(currentWP, neighbour, endWP));
                        neighbour.setPrevious(currentWP);
                        // Remove and re-add to keep the list sorted
                        openNodes.Remove(neighbour);
                        AddToOpenNodes(neighbour);
                    }
                    if(neighbour.getState() == "unexplored") {
                        neighbour.setGCost(potential_g_Cost);
                        neighbour.setFCost(CalculateFCost(currentWP, neighbour, endWP));
                        neighbour.setPrevious(currentWP);
                        neighbour.setState("open");
                        AddToOpenNodes(neighbour);
                    }
                }
            }
        }

        /* DEBUG */
        float timeSpent = Time.realtimeSinceStartup - startTime - drawTime1 - drawTime2;
        Debug.Log("Time spent calculating:" + timeSpent);
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
        else if((!Physics.Raycast(pos, end - pos, (pos - end).magnitude + .1f, layerMask)) && Mathf.Abs(pos.x - end.x) <= gridSize && Mathf.Abs(pos.z - end.z) <= gridSize) {
            return true;
        }
        return false;
    }

    // Function that calculates the Chebyshev distance betweent two points
    static float Heuristic(Vector3 p1, Vector3 p2) {
        // Current heuristic: Diagonal shortcut
        float dx = Mathf.Abs(p1.x - p2.x);
        float dy = Mathf.Abs(p1.z - p2.z);
        if(dx > dy) {
            return 14 * dy + 10 * (dx - dy);
        }
        else {
            return 14 * dx + 10 * (dy - dx);
        }
    }

    // Function that can reconstruct the path from an A* route
    static List<Vector3> ReconstructPath(Vector3 startPos, WayPoint endWP) {
        List<Vector3> bestPath = new List<Vector3>();
        WayPoint currWP = endWP;
        while(true) {
            if(currWP.getPosition() == startPos) {
                bestPath.Insert(0, startPos);
                break;
            }
            bestPath.Insert(0, currWP.getPosition());
            /* DEBUG */
            float temp = Time.realtimeSinceStartup;
            Debug.DrawLine(currWP.getPosition(), currWP.getPrevious().getPosition(), Color.blue, Mathf.Infinity, false);
            drawTime3 += Time.realtimeSinceStartup - temp;
            /* DEBUG */
            currWP = currWP.getPrevious();
        }
        return bestPath;
    }

    // Function that calculates the g-cost between two waypoints (cost based on distance from start point)
    static float CalculateGCost(WayPoint current, WayPoint destination) {
        return current.getGCost() + (current.getPosition() - destination.getPosition()).magnitude;
    }

    // Function that calculates the f-cost between two waypoints (cost based on distance from both start and end point)
    static float CalculateFCost(WayPoint current, WayPoint destination, WayPoint endpoint) {
        // Distance from current node to destination
        float g_cost = CalculateGCost(current, destination);

        // Heuristic, in our case the Diagonal Shortcut
        float h_cost = Heuristic(destination.getPosition(), endpoint.getPosition());
        
        /* DEBUG */
        //Debug.Log("g-cost: " + g_cost);
        //Debug.Log("h-cost: " + D * h_cost);
        /* DEBUG */

        // Balance heuristic influence using D (the higher D, the faster the calculation, but the lower the accuracy);
        return g_cost + D * h_cost;
    }

    // Function that can find a Waypoint at a certain location
    static WayPoint FindWayPointAt(Vector3 position, List<WayPoint> grid) {
        /* DEBUG */
        //Debug.Log("Looking for waypoint at position: " + position);
        /* DEBUG */
        foreach(WayPoint wp in grid) {
            if(wp.getPosition() == position) {
                return wp;
            }
        }
        Debug.LogError("No waypoint exists at given position, returning null!");
        return null;
    }

    // Fuction that adds a WayPoint to the list of open nodes, while keeping the list sorted by lowest cost.
    static void AddToOpenNodes(WayPoint newWp) {
        float newFCost = newWp.getFCost();
        if(openNodes.Count == 0) {
            openNodes.Add(newWp);
        }
        for(int i = 0; i < openNodes.Count; i++) {
            if(newFCost < openNodes[i].getFCost()) {
                openNodes.Insert(i, newWp);
                return;
            }
        }
        // If the code reaches this line, the new WP has the highest value so far, so add it to the list's end.
        openNodes.Add(newWp);
    }
}
