using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;

    public int startingHealth = 100;
    public int currentHealth;
	public int defense;
	public EnemyStats enemyStats;
    public Vector3 spawnPosition;
    public Vector3 deathPosition;
    EnemyMovement enemyMovement;

    public bool isDead = false;
    public bool hasWon = false;
	public bool isPoisoned;
	public float poisonAmount = 0;

	private GameObject textObject;
	private float nodeSize;

    Animator animator;

    float counter;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
        startingHealth = enemyStats.health*10;
        defense = enemyStats.defense;
        currentHealth = startingHealth;
        deathPosition = new Vector3(0, 100, 0);
		InvokeRepeating ("doPoisonDamage", 0, 5);
		textObject = resourceManager.damageText;
		nodeSize = resourceManager.nodeSize;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWon)
        {
            GoalScript.lives -= 1;
            hasWon = false;
        }

        if(isDead)
        {
            counter += Time.deltaTime;
            collider.enabled = (false);
            if (counter > 4)
            {
                //Destroy(this.gameObject);
                this.transform.position = deathPosition;
                counter = 0;
            }
        }

        else
        {
            collider.enabled = true;
        }

    }

	public void TakeDamage(int amount,string damageType)
    {
		Color kleur;
		if (damageType.Equals ("magic"))
			kleur = Color.blue;
		else if (damageType.Equals ("physical"))
			kleur = Color.red;
		else if (damageType.Equals ("poison"))
			kleur = Color.green;
		else
			kleur = Color.black;
        if (isDead)
        {
            return;
        }
		int damageDone = amount / defense;
		if (damageDone <= 1)
			damageDone = 1;
		currentHealth -= damageDone;
		if (currentHealth < 0) {
			damageDone += currentHealth;
			currentHealth = 0;

		}
		GameObject textObj = (GameObject)Instantiate (textObject, transform.position, Quaternion.identity);
		//textObj.GetComponent<TextMesh>().text = (Application.dataPath).ToString();
		textObj.GetComponent<TextMesh>().text = (damageDone + "/"+currentHealth).ToString();
		textObj.GetComponent<TextMesh> ().color = kleur;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

	public void TakePoisonDamage(int amount)
	{
		poisonAmount = Mathf.Max (amount, poisonAmount);
		isPoisoned = true;
	}

	private void doPoisonDamage()
	{
		if (isPoisoned) {
			TakeDamage ((int)poisonAmount,"poison");
			poisonAmount *= 0.5f;
			if (poisonAmount <= 1) {
				isPoisoned = false;
			}
		}
	}

    public void Death()
    {
		playerData.addGold(resourceManager.rewardEnemy);
        List<WayPoint> WPoints = new List<WayPoint>();
        WPoints = Navigator.FindWayPointsNear(transform.position, resourceManager.Nodes, nodeSize);
        foreach (WayPoint wp in WPoints)
        {
            float newPenalty = wp.getPenalty() +15;
            wp.setPenalty(newPenalty);
        }
		isDead = true;

        //currentHealth = startingHealth;
        //transform.position = startPosition;
        //Debug.Log("Ik ben dood");
    }

}
