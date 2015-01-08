using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    PlayerHealth playerHealth;
    public Text healthText;
    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        healthText.text = playerHealth.currentHealth.ToString();
	}
}
