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

    public int getDefStat()
    {
        return defence;
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
        InvokeRepeating("RegainHealth",0f,1f);
	}

    void RegainHealth()
    {
        if (currentHealth < 5001)
        {
            currentHealth += 17;
        }
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
        ScoreServer.sendScoreToServer();
        //new ScoreServer().sendScoreToServer(); 

        //Destroy(this.gameObject);
    }

}
