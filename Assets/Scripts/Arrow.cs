﻿using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public int damagePerShot;// = 1500;
	//Transform Player;
	Vector3 PrevItLoc;
	private float maxBulletDistance = 200;
	public GameObject Boom;
	LayerMask ignoreMask = ~(1 << 13);
	bool headshot;

	void GotThrough()
	{
		RaycastHit hit;
		if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
		{
			EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
			Destroy(this.gameObject);
			GameObject boom = (GameObject)Instantiate(Boom, PrevItLoc, Quaternion.identity);
			if (hit.rigidbody != null)
			{
				boom.rigidbody.velocity = hit.collider.rigidbody.velocity;
				boom.GetComponent<BoomParticleScript>().Hit = hit.collider.gameObject;
			}
			if (enemyHealth != null)
			{
				enemyHealth.TakeDamage(damagePerShot, "physical");
			}

		}
		PrevItLoc = transform.position;
	}


	// Use this for initialization
	void Start()
	{
		//Player = GameObject.Find("Player").transform;
		PrevItLoc = transform.position;
	}

	void FixedUpdate()
	{
		GotThrough();
	}

	// Update is called once per frame
	void Update()
	{

		if ((PrevItLoc- transform.position).magnitude >maxBulletDistance)
		{
			Destroy(this.gameObject);
		}

	}
}
