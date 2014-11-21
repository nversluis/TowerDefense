using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint {
	private Vector3 position;
	private List<WayPoint> destinations;
	private ArrayList availableNodes;

	 public WayPoint(Vector3 position){
		this.position = position;
		destinations=new List<WayPoint>();	
	}

	public void AddNode(WayPoint node){
		destinations.Add (node);
	}

	public List<WayPoint> getDestinations(){
		return destinations;
	}

	public Vector3 getPosition(){
				return position;
		}


}
