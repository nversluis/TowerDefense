using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigator : MonoBehaviour {

    float gridSize = 2;

    public List<Vector3> Path(Vector3 startPoint, Vector3 endPoint) {

        /** INITIALIZATION **/

        // Load the grid
        List<WayPoint> grid = RandomMaze.Nodes;
        // Create a new (empty) route
        List<WayPoint> route = new List<WayPoint>();
        List<Vector3> path = new List<Vector3>();
        // Create a new waypoint for the starting position
        WayPoint startWP = new WayPoint(startPoint, "start");
        // Initiate best cost so far and estimate total expected cost using Manhattan distance
        float costSoFar = 0;
        float estimatedCost = costSoFar + ManhattanDist(startPoint, endPoint);

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
                startWP.AddNode(new WayPoint(startNodes[i], "open"));
                // There are still open destinations
                openDestinationsExist = true;
            }
        }
        // Set state to closed (we don't want to go back to the starting node)
        startWP.setState("closed");

        // Our current WP is the starting WP
        WayPoint currentWP = startWP;

        /** ROUTE CALCULATION **/
        while(openDestinationsExist) {
            // Set the cheapest possible destination as the next destination
            WayPoint cheapestWP = new WayPoint(new Vector3(0, 0, 0));
            cheapestWP.setCost(float.MaxValue);
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint destination = currentWP.getDestinations()[i];
                if(destination.getCost() < cheapestWP.getCost() && destination.getState().Equals("open")) {
                    cheapestWP = currentWP.getDestinations()[i];
                }
            }
            cheapestWP.setPrevious(currentWP);
            currentWP = cheapestWP;

            // If the current waypoint is the endpoint, stop searching and build the route
            if(CloseEnoughToDestination(currentWP)) {
                route.Add(new WayPoint(endPoint));
                path = ReconstructPath(route, currentWP);
                openDestinationsExist = false;
                break;
            }
            // Close current waypoint so we can't go back to it
            currentWP.setState("closed");

            // Find unexplored nodes and open them if they seem useful
            for(int i = 0; i < currentWP.getDestinations().Count; i++) {
                WayPoint neighbour = currentWP.getDestinations()[i];
                // Don't even try if it's closed
                if(!neighbour.getState().Equals("closed")) {
                    float potentialCost = costSoFar + (currentWP.getPosition() - neighbour.getPosition()).magnitude;
                    // Explore unexplored if it is interesting (cheap)
                    if(neighbour.getState().Equals("unexplored") || potentialCost < neighbour.getCost()) {
                        // If interesting, add to open list
                        neighbour.setPrevious(currentWP);
                        route.Add(neighbour);
                        costSoFar = potentialCost;
                        estimatedCost = costSoFar + ManhattanDist(neighbour.getPosition(), endPoint);
                        neighbour.setState("open");
                    }
                }
            }
        }
        return path;

    }

    float RoundUp(float toRound) {
        return (gridSize - toRound % gridSize) + toRound;
    }

    float RoundDown(float toRound) {
        return toRound - toRound % gridSize;
    }

    bool CloseEnoughToDestination(WayPoint wp) {
        return false;
    }

    float ManhattanDist(Vector3 p1, Vector3 p2) {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.z - p2.z);
    }

    List<Vector3> ReconstructPath(List<WayPoint> path, WayPoint currWP) {
        List<Vector3> bestPath = new List<Vector3>();
        bestPath.Add(currWP.getPosition());
        foreach(WayPoint WP in path){
            currWP = currWP.getPrevious();
            bestPath.Add(currWP.getPosition());
        }
        return bestPath;
    }
}
