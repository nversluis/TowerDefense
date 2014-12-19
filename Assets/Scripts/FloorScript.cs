using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour {

	Color transparentgreen = new Color(0,255,0,0.1f);
	private int i;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;

	void Start(){
		i = 0;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
	}
	void Update(){
		//Debug.Log (CameraController.hitObject.name);
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the floor
			WallScript.DestroyHotSpots (); //Destroy all hotspots
			int aantalPerPlane = WeaponController.trapGridSize;
			GameObject TowerPrefab = WeaponController.curFloorTower;
			if (TowerPrefab != null&&gameObject.transform.childCount==1) { 
				float planeW =resourceManager.planewidth;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position+new Vector3(0,planeW/1000,0), transform.rotation);
				//tower.gameObject.transform.Rotate(270, 0, 0);
				tower.gameObject.transform.localScale=new Vector3(1, 1, 1)*planeW*5;

				float randHoek = 90 * Mathf.Floor (Random.value * 4);
				tower.transform.RotateAround (transform.position, Vector3.up, randHoek);

				//tower.renderer.material.color =new Color(0,255,0,0.1f);
				//tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) {
					foreach (Material mat in child.materials) {
						mat.shader = Shader.Find ("Transparent/Diffuse");
						mat.color = transparentgreen;
					}
				}

				//tower.transform.GetChild (0).gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				//tower.transform.GetChild (0).gameObject.renderer.
				tower.transform.parent = gameObject.transform;			
			}
		}

	}

}
