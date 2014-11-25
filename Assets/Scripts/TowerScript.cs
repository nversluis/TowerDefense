using UnityEngine;
using System.Collections;

//Script to be added to the tower prefabs


public class TowerScript : MonoBehaviour {
	//Initizialising
	float MaxDistance=TowerVariables.Distance;
	Color transparentgreen = new Color(0,255,0,0.1f); //Color of the green prefab
	Color transparentred = new Color(255,0,0,0.1f); //Color of the red prefab

	//Method to build a tower. Will destroy the prefab and build a new tower there.
	public void BuildTower(){
		GameObject tower = (GameObject)Instantiate (TowerVariables.curTower,transform.position, transform.rotation); //The instantiantion of the tower
		tower.gameObject.transform.localScale=new Vector3(RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/100)*100; //Scaling the tower
		tower.tag = "Tower"; //Give tower a new tag, so it wont be destroyed because its a hotspot
		tower.collider.isTrigger = false; //remove the trigger, cant walk trough it
		Destroy (gameObject); // Destroy all hotspots
		tower.SetActiveRecursively (true); //Active its children (the trigger)
	}

	void OnMouseOver(){
		//If richt mouse button on tower, remove the tower
		//todo - Give 1/2 money back
		//todo - Menu with options (sell, upgrade)
		if (Input.GetMouseButtonUp (1)&&gameObject.tag.Equals("Tower"))
			Destroy (gameObject);

	}

	void Update(){
		//check for click to build tower
		if (Input.GetMouseButtonUp (0) && gameObject.tag.Equals ("TowerHotSpot")) {
			BuildTower ();
		}

		//Delete hotspots
        if (gameObject == CameraController.getHitObject() && gameObject.tag.Equals("Tower")) 	
			WallScript.DestroyHotSpots ();

		//change hotspots according to distance
		if (!tag.Equals ("Tower")) {
			GameObject Player = GameObject.FindGameObjectWithTag ("Player");
			if (Vector3.Distance (Player.transform.position, transform.position) >= MaxDistance) {
				if (!tag.Equals ("HotSpotRed"))
					setRed ();
			}		
			else {	
				setGreen ();
			}
		}
	}

	//set the tower to a green hotspot
	private void setGreen(){
	
		gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
		gameObject.renderer.material.color = transparentgreen;
		gameObject.tag = "TowerHotSpot";
		gameObject.collider.isTrigger = true;
		}
	
	//set the tower to a red hotspot
	private void setRed(){
		gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
		gameObject.renderer.material.color = transparentred;
		gameObject.tag="HotSpotRed";		
		gameObject.collider.isTrigger = true;
		}

}
