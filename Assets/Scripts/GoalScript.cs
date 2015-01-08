using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

    Text livesText;
	public static float lives;
	public static float maxLives;
    public static bool Lost;

    GameObject gui;
    GUIScript guiScript;


	// Use this for initialization
	void Start () {
      //  livesText = GameObject.Find("NumOfLives").GetComponent<Text>(); ;
        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
		lives = 15;
		maxLives = 15;
	}
	
	// Update is called once per frame
	void Update () {
       // livesText.text = lives.ToString();

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
