using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

    Text livesText;
	public static float lives = 15;
    public static float maxLives = lives;
    public static bool Lost;

    GameObject gui;
    GUIScript guiScript;


	// Use this for initialization
	void Start () {
        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
		lives = 15;
	}
	
	// Update is called once per frame
	void Update () {

        if (lives < 1)
        {
            lives = 0;
            Lost = true;
        }

        if (Lost)
        {
            guiScript.EndGame("You Lose!");
            Lost = false;
        }
	}

}
