﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

	private float lives;
	private float maxLives;
    private bool Lost;

    GameObject gui;
    GUIScript guiScript;

	// Use this for initialization
	void Start () {
      //  livesText = GameObject.Find("NumOfLives").GetComponent<Text>(); ;
        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
		lives = 15;
		maxLives = lives;
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
            Lost = false;
<<<<<<< .merge_file_a82232
            guiScript.EndGame("You Lose!", "Gate");
=======
            guiScript.EndGame("Gate");
>>>>>>> .merge_file_a81188
        }
	}

    public float getLives() {
        return lives;
    }

    public float getMaxLives() {
        return maxLives;
    }

    public void removeLife() {
        lives -= 1;
    }
}
