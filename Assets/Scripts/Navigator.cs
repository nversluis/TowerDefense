using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigator : MonoBehaviour {

    public float gridSize;

    public List<Vector3> Path(Vector3 startPoint, Vector3 endPoint) {

        /** INITIALIZATION **/

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
        float x1 = RoundUp(startPoint.x);
        float x2 = RoundDown(startPoint.x);
        float z1 = RoundUp(startPoint.z);
        float z2 = RoundDown(startPoint.z);

        // Also include diagonal destinations, making for a total of 8 possible destinations
        List<Vector3> startNodes = new List<Vector3>();
        startNodes[1] = new Vector3(x1, 0, 0);
        startNodes[2] = new Vector3(x1, 0, z1);
        startNodes[3] = new Vector3(x1, 0, z2);
        startNodes[4] = new Vector3(x2, 0, 0);
        startNodes[5] = new Vector3(x2, 0, z1);
        startNodes[6] = new Vector3(x2, 0, z2);
        startNodes[7] = new Vector3(0, 0, z1);
        startNodes[8] = new Vector3(0, 0, z2);

        // Add found nodes to destination list of start node if they are visible and set their state to open
        bool openDestinationsExist = false;
        for(int i = 0; i < 8; i++) {
            if(!Physics.Raycast(startPoint, startNodes[i], (startPoint - startNodes[i]).magnitude)) {
                // Add node to destination if it's reachable
                WayPoint newDest = FindWayPointAt(startNodes[i], grid);
                newDest.setCost(CalculateGCost(startWP, newDest));
                startWP.AddNode(newDest);
                // There are still open destinations
                openDestinationsExist = true;
            }
        }

        // Our current WP is the starting WP
        WayPoint currentWP = startWP;

        /** ROUTE CALCULATION **/

        while(openDestinationsExist) {
            // Set the cheapest possible destination as the next destination
            // Initialize cheapest point so far to be as expensive as possible
            WayPoint cheapestWP = new WayPoint(new Vector3(0, 0, 0), "closed");
            float cheapest = float.MaxValue;
            // Find the cheapest destination
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint destination = currentWP.getDestinations()[i];
                if(destination.getState().Equals("open")) {
                    float fCost = CalculateFCost(currentWP, destination, endWP);
                    if(fCost <= cheapest) {
                        cheapestWP = destination;
                    }
                }
            }
            if(cheapest == float.MaxValue) {
                Debug.LogError("Error: route stuck with no destinations, empty path returned");
                break;
            }

            // Move to new WP and set it as closed
            currentWP = cheapestWP;
            currentWP.setState("closed");

            // If the current waypoint is the endpoint, stop searching and build the route
            if(CloseEnoughToDestination(currentWP, endPoint)) {
                path = ReconstructPath(startPoint, endWP);
                openDestinationsExist = false;
                break;
            }

            // Find unexplored nodes and open them if they seem useful
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint neighbour = currentWP.getDestinations()[i];
                // Don't even try if it's closed
                if(!neighbour.getState().Equals("closed")) {
                    float potential_g_Cost = currentWP.getCost() + (currentWP.getPosition() - neighbour.getPosition()).magnitude;
                    // If a cheaper g cost is found via the current WayPoint, update it
                    if(potential_g_Cost < neighbour.getCost()) {
                        neighbour.setCost(potential_g_Cost);
                        neighbour.setPrevious(currentWP);
                    }
                    if(neighbour.getState().Equals("unexplored")) {
                        neighbour.setCost(CalculateGCost(currentWP, neighbour));
                        neighbour.setState("open");
                        neighbour.setPrevious(currentWP);
                    }
                }
            }
        }
        return path;
    }

    /** FUNCTIONS **/

    // Function that rounds up a certain number to the grid size
    float RoundUp(float toRound) {
        return (gridSize - toRound % gridSize) + toRound;
    }
    // Function that rounds down a certain number to the grid size
    float RoundDown(float toRound) {
        return toRound - toRound % gridSize;
    }
    // Function that checks if the given waypoint is close enough to the end point to navigate to it without using the grid
    bool CloseEnoughToDestination(WayPoint wp, Vector3 end) {
        Vector3 pos = wp.getPosition();
        if((!Physics.Raycast(pos,end,(pos-end).magnitude)) && Mathf.Abs(pos.x - end.x) <= gridSize &&  Mathf.Abs(pos.z - end.z) <= gridSize) {
            return true;
        }
        return false;
    }
    // Function that calculates the Manhattan distance between two points
    float ManhattanDist(Vector3 p1, Vector3 p2) {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.z - p2.z);
    }
    // Function that can reconstruct the path from an A* route
    List<Vector3> ReconstructPath(Vector3 startPos, WayPoint endWP) {
        List<Vector3> bestPath = new List<Vector3>();
        WayPoint currWP = endWP;
        bestPath.Add(endWP.getPosition());
        while(true) {
            currWP = currWP.getPrevious();
            if(currWP.getPosition() == startPos) {
                bestPath.Add(startPos);
                break;
            }
            bestPath.Add(currWP.getPosition());
        }
        return bestPath;
    }
    // Function that calculates the g-cost between two waypoints (cost based on distance from start point)
    float CalculateGCost(WayPoint current, WayPoint destination) {
        return current.getCost() + (current.getPosition() - destination.getPosition()).magnitude;
    }
    // Function that calculates the f-cost between two waypoints (cost based on distance from both start and end point)
    float CalculateFCost(WayPoint current, WayPoint destination, WayPoint endpoint) {
        // Distance from current node to destination
        float g_cost = current.getCost() + (current.getPosition() - destination.getPosition()).magnitude;
        // Heuristic, in our case the Manhattan distance
        float h_cost = ManhattanDist(current.getPosition(), endpoint.getPosition());
        return g_cost + h_cost;
    }
    // Function that can find a Waypoint at a certain location
    WayPoint FindWayPointAt(Vector3 position, List<WayPoint> grid) {
        foreach(WayPoint wp in grid) {
            if(wp.getPosition() == position) {
                return wp;
            }
        }
        Debug.LogError("No waypoint exists at given position, returning null!");
        return null;
    }

}
