using UnityEngine;
using System.Collections;

// Class for determining where to spawn the player
public class PlayerSpawner : MonoBehaviour
{

    // Player and camera prefabs
    public GameObject player;
    public GameObject camera;
    public GameObject Gui;

    // when starting the game
    void Start()
    {
        // create player and camera
        GameObject Player = (GameObject)Instantiate(player, transform.position, Quaternion.identity);
		Player.gameObject.transform.localScale = new Vector3(0.05f,0.05f,0.05f) ;
        Player.name = "Player";
        GameObject MainCamera = (GameObject)Instantiate(camera, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
        MainCamera.name = "Main Camera";
        GameObject MainGui = (GameObject)Instantiate(Gui, transform.position, Quaternion.identity);
        MainGui.name = "GUI";


    }

}
