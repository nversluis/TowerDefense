using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 5000;
    public int currentHealth;
    public int defence = 10;
    PlayerData playerData = GUIScript.player;
    GameObject gui;
    GUIScript guiScript;

    bool isDead = false;

    void Awake()
    {
        currentHealth = startingHealth;
    }

	// Use this for initialization
	void Start () 
    {
        playerData.setMaxHP(startingHealth);
        gui = GameObject.Find("GUIMain");
        guiScript = gui.GetComponent<GUIScript>();
	}
	
	// Update is called once per frame
    void Update()
    {
        playerData.setCurrentHP(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount/defence;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        guiScript.resultScoreText.text = Statistics.Score().ToString();
        guiScript.EndGame("Player");

        //Destroy(this.gameObject);
    }

}
