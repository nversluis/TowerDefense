using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
	public int defense;
	public EnemyStats enemyStats;
    public Vector3 startPosition;
    EnemyMovement enemyMovement;

    public bool isDead = false;

	private GameObject textObject;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void Start()
    {
        startingHealth = enemyStats.health;
        defense = enemyStats.defense;
        currentHealth = startingHealth;
        startPosition = new Vector3(50, 50, 50);
		textObject = gameObject.transform.GetChild (4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }
		int damageDone = amount / defense;
		if (damageDone <= 1)
			damageDone = 1;
		currentHealth -= damageDone;
		textObject.SetActive (true);
		textObject.GetComponent<TextMesh>().text = (damageDone + "/"+currentHealth).ToString();
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        Destroy(this.gameObject);

        //currentHealth = startingHealth;
        //transform.position = startPosition;
        //Debug.Log("Ik ben dood");
    }

}
