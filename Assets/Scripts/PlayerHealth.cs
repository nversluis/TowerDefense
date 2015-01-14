using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 5000;
    public int currentHealth;
    int defence;
    PlayerData playerData = GUIScript.player;
    GameObject gui;
    GUIScript guiScript;

    bool isDead = false;

    public void addPlayerDefense(int addDefense)
    {
        defence += addDefense;
    }
    
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
        defence = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().defense;
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
