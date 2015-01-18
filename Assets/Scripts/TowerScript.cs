using UnityEngine;
using System.Collections;

//Script to be added to the tower prefabs


public class TowerScript : MonoBehaviour
{
    //Initizialising
    private GameObject ResourceManagerObj;
    private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;

	private GameObject realTower;
	private GameObject redTower;
	private int cost;
	GameObject player;
    float MaxDistance;
    Color transparentgreen = new Color(0, 255, 0, 0.1f); //Color of the green prefab
    Color transparentred = new Color(255, 0, 0, 0.1f); //Color of the red prefab
    float planeW;
    void Start()
    {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        planeW = resourceManager.planewidth;
        MaxDistance = resourceManager.maxTowerDistance;
		if (gameObject.name.Contains ("magic")) {
			realTower = resourceManager.magicTower;
			cost = resourceManager.costMagicTower;
		} else if (gameObject.name.Contains ("Arrow")) {
			realTower = resourceManager.arrowTower;
			cost = resourceManager.costArrowTower;
		}
		player = GameObject.Find ("Player");

    }

    //Method to build a tower. Will destroy the prefab and build a new tower there.
    public void BuildTower()
    {
		if (cost <= playerData.getGold()) {
			GameObject tower = (GameObject)Instantiate (realTower, transform.position, transform.rotation); //The instantiantion of the tower
			tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW / 10/2; //Scaling the tower

			tower.tag = "Tower"; //Give tower a new tag, so it wont be destroyed because its a hotspot
			tower.transform.parent = gameObject.transform.parent;
			//tower.collider.isTrigger = false; //remove the trigger, cant walk trough it
			tower.SetActiveRecursively (true); //Active its children (the trigger)
			gameObject.layer = 13;
			playerData.addGold (-cost);
			Destroy (gameObject); // Destroy all hotspots

		}
		else {
			GameObject.Find("GUIMain").GetComponent<GUIScript>().Notification ("NoGold");
		}
    }

   

    void Update()
    {
        //check for click to build tower
        if (Input.GetMouseButtonUp(0) && gameObject.tag.Contains("HotSpot"))
        {
            BuildTower();
        }

	

        //Delete hotspots
		if (gameObject == CameraController.hitObject && gameObject.tag.Equals("Tower") || (player.transform.position-transform.position).magnitude>resourceManager.maxTowerDistance)
            WallScript.DestroyHotSpots();

        //change hotspots according to distance
        if (!tag.Equals("Tower"))
        {
            GameObject Player = GameObject.Find("Player");
			if ((Vector3.Distance(Player.transform.position, transform.position) >= MaxDistance)||(cost>playerData.getGold()))
            {
                if (!tag.Equals("HotSpotRed"))
                    setRed();
            }
            else
            {
                setGreen();
            }
        }
    }


    //set the tower to a green hotspot
    private void setGreen()
    {
		foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {

			child.material.color = transparentgreen;
			child.material.shader = Shader.Find ("Transparent/Diffuse");
		}
        gameObject.tag = "TowerHotSpot";
        gameObject.collider.isTrigger = true;
        gameObject.layer = 14;

    }

    //set the tower to a red hotspot
    private void setRed()
	{
		foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {

			child.material.color = transparentred;
			child.material.shader = Shader.Find ("Transparent/Diffuse");
		}
        gameObject.tag = "HotSpotRed";
        gameObject.collider.isTrigger = true;
        gameObject.layer = 14;
    }

}
