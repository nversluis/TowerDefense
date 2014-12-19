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
		realTower = resourceManager.tower1;
		cost = resourceManager.costMagicTower;
    }

    //Method to build a tower. Will destroy the prefab and build a new tower there.
    public void BuildTower()
    {
		if (cost <= playerData.getGold()) {
			GameObject tower = (GameObject)Instantiate (realTower, transform.position, transform.rotation); //The instantiantion of the tower
			tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW / 10; //Scaling the tower

			tower.tag = "Tower"; //Give tower a new tag, so it wont be destroyed because its a hotspot
			tower.transform.parent = gameObject.transform.parent;
			//tower.collider.isTrigger = false; //remove the trigger, cant walk trough it
			Destroy (gameObject); // Destroy all hotspots
			tower.SetActiveRecursively (true); //Active its children (the trigger)
			gameObject.layer = 13;
			playerData.addGold (-cost);
		}else {
			Debug.Log("Not enough gold to build " + realTower.name);
		}
    }

    void OnMouseOver()
    {
        //If mouse button on tower, remove the tower
        //todo - Give 1/2 money back
        //todo - Menu with options (sell, upgrade)
        if (Input.GetMouseButtonUp(1) && gameObject.tag.Equals("Tower"))
            Destroy(gameObject);

    }

    void Update()
    {
        //check for click to build tower
        if (Input.GetMouseButtonUp(0) && gameObject.tag.Equals("TowerHotSpot"))
        {
            BuildTower();
        }

	

        //Delete hotspots
        if (gameObject == CameraController.hitObject && gameObject.tag.Equals("Tower"))
            WallScript.DestroyHotSpots();

        //change hotspots according to distance
        if (!tag.Equals("Tower"))
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(Player.transform.position, transform.position) >= MaxDistance)
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
