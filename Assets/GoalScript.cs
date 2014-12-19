using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

    Text livesText;
    public int lives = 15;
    public static bool Lost;

	// Use this for initialization
	void Start () {
        livesText = GameObject.Find("NumOfLives").GetComponent<Text>(); ;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(lives);
        livesText.text = lives.ToString();

        if (lives < 1)
        {
            lives = 0;
            Lost = true;
        }
	}

}
