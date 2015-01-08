using UnityEngine;
using System.Collections;

// Class for determining where to spawn the player
public class PlayerSpawner : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;

    // Player and camera prefabs
    private GameObject player;
    private GameObject camera;
    private GameObject Gui;

    // when starting the game
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		player = resourceManager.player;
		camera = resourceManager.mainCamera;
        // create player and camera
        GameObject Player = (GameObject)Instantiate(player, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
		Player.gameObject.transform.localScale = new Vector3(0.05f,0.05f,0.05f) ;
        Player.name = "Player";
        GameObject MainCamera = (GameObject)Instantiate(camera, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
        MainCamera.name = "Main Camera";
        GameObject MainGui = (GameObject)Instantiate(Gui, transform.position, Quaternion.identity);
        MainGui.name = "GUI";   

    }

}
