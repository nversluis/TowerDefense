using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

	private float health;
	private float maxHealth;
    private bool Lost;

    GameObject gui;
    GUIScript guiScript;

	// Use this for initialization
	void Start () {
      //  livesText = GameObject.Find("NumOfLives").GetComponent<Text>(); ;
        //gui = GameObject.Find("GUIMain");
        //guiScript = gui.GetComponent<GUIScript>();
		health = 5000;
		maxHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
       // livesText.text = lives.ToString();
		if (health <= 0)
        {
            health = 0;
            Lost = true;
        }

        if (Lost)
        {
            Lost = false;
            guiScript.EndGame("Gate");
        }
	}

    public float getLives() {
		return health;
    }

    public float getMaxLives() {
        return maxHealth;
    }

	public void removeLife(int damage) {
		health -= damage;
    }

	public void removeLife() {
		health -= 1000;
	}

	public void setGui()
	{
		gui = GameObject.Find("GUIMain");
		guiScript = gui.GetComponent<GUIScript>();
	}
}
