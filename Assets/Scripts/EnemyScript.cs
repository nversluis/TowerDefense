﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {
    List<Vector3> Path;
    int i = 0;
    float walkSpeed = 10f;
    float orcHeigthSpawn = 3.27f;
    CharacterController characterController;

	// Use this for initialization
	void Start () {

        characterController = GetComponent<CharacterController>();
        Path = Navigator.Path(transform.FindChild("Floor").transform.position + new Vector3(0.1f, 0f, 0f), PlayerController.location - new Vector3(0f, PlayerController.location.y, 0f));

	}
	
	// Update is called once per frame
    void FixedUpdate()
    {

        if (Path != null)
        {
            Vector3 dir = (Path[i] - transform.FindChild("Floor").position).normalized * walkSpeed;
            characterController.SimpleMove(dir);
            if ((Path[i] - transform.FindChild("Floor").position).magnitude < 2f && i < Path.Count - 1)
            {
                i++;
            }
            transform.LookAt(Path[i] + new Vector3(0f, transform.position.y, 0f));
        }

    }

   
}