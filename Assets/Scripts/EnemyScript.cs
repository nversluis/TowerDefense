﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private float nodeSize;
	private List<WayPoint> grid;
	List<Vector3> Path;
	int i = 0;
	float walkSpeed;
	float orcHeigthSpawn = 3.27f;
	CharacterController characterController;
	public bool automaticPathUpdating;
	EnemyStats enemystats;


	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		grid = resourceManager.Nodes;
		float nodeSize = resourceManager.nodeSize;

		characterController = GetComponent<CharacterController>();
		Path = Navigator.Path(transform.position, PlayerController.location - new Vector3(0f, PlayerController.location.y,0f),nodeSize,grid);

		enemystats = GetComponent<EnemyStats>();
		walkSpeed = 0.03f*enemystats.speed + 12;


		if (automaticPathUpdating)
		{
			InvokeRepeating("BuildPath", 0, 0.5f);
		}

	}

	//Update is called once per frame
	void FixedUpdate()
	{

		if (Path != null)
		{
			Vector3 dir;

			if (i != 0)
			{
				dir = (Path[i] - transform.position).normalized * walkSpeed;
			}

			else
			{
				dir = (Path[i+1] - transform.position).normalized * walkSpeed;

			}

			rigidbody.velocity = (dir+new Vector3(0f,rigidbody.velocity.y,0f));
			rigidbody.angularVelocity = Vector3.zero;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * 5f);
			transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
			Vector3 nextPointDistance = (Path[i] - transform.position);
			nextPointDistance.y = 0;
			if (nextPointDistance.magnitude < 1f && i < Path.Count - 1)

			{
				i++;
			}

			if ((Path[i] - transform.position).magnitude < 1f && i == Path.Count - 1)
			{
				rigidbody.velocity = Vector3.zero;

			}

		}


		if (Input.GetKeyDown(KeyCode.Q) && !automaticPathUpdating)
		{
			Path = Navigator.Path(transform.position, PlayerController.location,nodeSize,grid);

			i = 0;
		}


	}

	void BuildPath(){

		Path = Navigator.Path(transform.position, PlayerController.location,resourceManager.nodeSize,resourceManager.Nodes);

		i = 0;

	}

}
