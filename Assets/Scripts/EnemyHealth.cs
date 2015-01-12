﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    EnemyResources enemyResources;
	private PlayerData playerData = GUIScript.player;

    public int startingHealth = 100;
    public int currentHealth;
	public int defense;
    public int magicDefense;
	public EnemyStats enemyStats;
    public Vector3 spawnPosition;
    public Vector3 deathPosition;
    EnemyMovement enemyMovement;

    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;

    Text guiHeadShot;
    AudioSource cameraAudioSource;
    AudioClip headShot;


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
        enemyResources = GetComponent<EnemyResources>();
        resourceManager = GetComponent<ResourceManager>();
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
        startingHealth = enemyStats.health*10;
        defense = enemyStats.defense;
        magicDefense = enemyStats.magicDefense;
        currentHealth = startingHealth;
        deathPosition = new Vector3(0, 100, 0);
		InvokeRepeating ("doPoisonDamage", 0, 5);
		textObject = resourceManager.damageText;
		nodeSize = resourceManager.nodeSize;
        animator = GetComponent<Animator>();
        guiHeadShot = GameObject.Find("HeadShotText").GetComponent<Text>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();
        cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        headShot = resourceManager.headShot;


        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyResources.isDead)
        {
            counter += Time.deltaTime;

            if (boxCollider != null)
            {
                boxCollider.enabled = (false);
            }
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = (false);
            }

            if (counter > 4)
            {
                
                Destroy(this.gameObject);
            }
        }

    }

	public void TakeDamage(int amount,string damageType, bool attackedByPlayer)
    {
        int damageDone = amount;
		Color kleur;
        if (damageType.Equals("magic"))
        {
            kleur = Color.blue;
            damageDone = amount / magicDefense;
        }
        else if (damageType.Equals("physical"))
        {
            kleur = Color.red;
            damageDone = amount / defense;
        }
        else if (damageType.Equals("poison"))
            kleur = Color.green;
        else
            kleur = Color.black;
        if (enemyResources.isDead)
        {
            return;
        }

        if (damageDone <= 1)
			damageDone = 1;
		currentHealth -= damageDone;
		if (currentHealth < 0) {
			damageDone += currentHealth;
			currentHealth = 0;

		}

        if (currentHealth <= 0 && !enemyResources.isDead)
        {
            Death(attackedByPlayer,false);
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
			TakeDamage ((int)poisonAmount,"poison", false);
			poisonAmount *= 0.5f;
			if (poisonAmount <= 1) {
				isPoisoned = false;
			}
		}
	}

    public void Death(bool killedByPlayer, bool headShot)
    {
        if (!enemyResources.isDead)
        {
            playerData.addGold(resourceManager.rewardenemy);
            List<WayPoint> WPoints = new List<WayPoint>();
            WPoints = Navigator.FindWayPointsNear(transform.position, resourceManager.Nodes, nodeSize);
            foreach (WayPoint wp in WPoints)
            {
                try
                {
                    float newPenalty = wp.getPenalty() + 15;
                    wp.setPenalty(newPenalty);
                }
                catch
                {
                    Debug.Log("penalty mislukt...");


                }
     
            }

            enemyResources.isDead = true;
            enemyResources.walking = false;
            enemyResources.attacking = false;

            int enemyType;

            if (this.gameObject.name == "Guyant")
            {
                enemyType = 0;
            }
            else if (this.gameObject.name == "Gwarf")
            {
                enemyType = 1;
            }
            else
            {
                enemyType = 2;
            }

            Statistics.Kill(enemyType, killedByPlayer, headShot);
        }
        else
        {
            return;
        }


    }

    public void HeadShot()
    {
        currentHealth = 0;
        if (currentHealth <= 0 && !enemyResources.isDead)
        {
            cameraAudioSource.PlayOneShot(headShot,2f);
            guiHeadShot.text = "HeadShot!";
            Death(true,true);
            Invoke("DeleteHeadshotText",1.5f);
        }
    }

    void DeleteHeadshotText()
    {
        guiHeadShot.text = "";

    }

}
