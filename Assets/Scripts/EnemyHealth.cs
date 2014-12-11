using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
	public int defense;
	public EnemyStats enemyStats;
    public Vector3 startPosition;
    EnemyMovement enemyMovement;

    public bool isDead = false;
	public bool isPoisoned;
	public float poisonAmount = 0;

	public GameObject textObject;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void Start()
    {
        startingHealth = enemyStats.health*10;
        defense = enemyStats.defense;
        currentHealth = startingHealth;
        startPosition = new Vector3(50, 50, 50);
		InvokeRepeating ("doPoisonDamage", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {

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
				Debug.Log (poisonAmount);
				isPoisoned = false;
				Debug.Log (isPoisoned);
			}
		}
	}



    void Death()
    {
       
		List<WayPoint> WPoints = new List<WayPoint> ();
		WPoints=Navigator.FindWayPointsNear (transform.position, RandomMaze.Nodes);
		foreach (WayPoint wp in WPoints) {
			float newPenalty = wp.getPenalty ()+100;
			wp.setPenalty (newPenalty);
		}
		isDead = true;
		Destroy(this.gameObject);

        //currentHealth = startingHealth;
        //transform.position = startPosition;
        //Debug.Log("Ik ben dood");
    }

}
