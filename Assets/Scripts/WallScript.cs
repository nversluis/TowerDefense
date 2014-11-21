using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
			
	Color transparentgreen = new Color(0,255,0,0.1f);
	
	void Update(){
		if (gameObject == CameraController.getHitObject()) {
			
			DestroyHotSpots ();
		
			GameObject TowerPrefab = TowerVariables.curTower;
			if (TowerPrefab != null) {
				Vector3 TowerOffset = new Vector3 (Mathf.Sin (transform.eulerAngles.y / 180 * Mathf.PI), 0, Mathf.Cos (transform.eulerAngles.y / 180 * Mathf.PI)) * RandomMaze.getPlaneWidth()/20;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position + TowerOffset, transform.rotation);
				tower.gameObject.transform.localScale=new Vector3(RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/100);
				tower.renderer.material.color = transparentgreen;
				tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
			}
		}

//		if(!CameraController.getHitObject ().tag.Contains("HotSpot"))
//		   DestroyHotSpots ();
	}


	public static void DestroyHotSpots(){
		GameObject[] GreenHotspot = GameObject.FindGameObjectsWithTag ("TowerHotSpot");
		GameObject[] RedHotspot = GameObject.FindGameObjectsWithTag ("HotSpotRed");
		foreach (GameObject hotspot in GreenHotspot)
						Destroy (hotspot);
		foreach (GameObject hotspot in RedHotspot)
						Destroy (hotspot);

		}


}
