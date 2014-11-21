using UnityEngine;
using System.Collections;

public class TowerScript : MonoBehaviour {

	float MaxDistance=TowerVariables.Distance;
	Color transparentgreen = new Color(0,255,0,0.1f);
	Color transparentred = new Color(255,0,0,0.1f);

	public void BuildTower(){
		GameObject tower = (GameObject)Instantiate (TowerVariables.curTower,transform.position, transform.rotation);
		tower.gameObject.transform.localScale=new Vector3(RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/100);
			tower.tag = "Tower";
			tower.collider.isTrigger = false;
			Destroy (gameObject);
	}

	void OnMouseOver(){
		//todo - Give 1/2 money back
		if (Input.GetMouseButtonUp (1)&&gameObject.tag.Equals("Tower"))
						Destroy (gameObject);

	}

	void Update(){
		BaseUpdate ();
		}

	public void BaseUpdate(){
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
	private void setGreen(){
		gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
		gameObject.renderer.material.color = transparentgreen;
		gameObject.tag = "TowerHotSpot";
		gameObject.collider.isTrigger = true;
		}
	private void setRed(){
		gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
		gameObject.renderer.material.color = transparentred;
		gameObject.tag="HotSpotRed";		
		gameObject.collider.isTrigger = true;
		}

}
