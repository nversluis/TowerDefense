using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigator : MonoBehaviour {

    float gridSize = 2;

    public List<Vector3> Path(Vector3 startPoint, Vector3 endPoint) {


        List<WayPoint> grid = RandomMaze.Nodes;

        List<Vector3> route = new List<Vector3>();

        WayPoint startWP = new WayPoint(startPoint, "start");

        List<Vector3> startNodes = new List<Vector3>();

        float x1 = RoundUp(startPoint.x);
        float x2 = RoundDown(startPoint.x);
        float y1 = RoundUp(startPoint.z);
        float y2 = RoundDown(startPoint.z);

        for(int i = 0; i < 8; i++) {
            if(!Physics.Raycast(startPoint, startNodes[i], (startPoint - startNodes[i]).magnitude)) {
                startWP.AddNode(new WayPoint(startNodes[i], "open"));
            }
        }
        return route;

    }

    float RoundUp(float toRound) {
        return (gridSize - toRound % gridSize) + toRound;
    }

    float RoundDown(float toRound) {
        return toRound - toRound % gridSize;
    }
}
