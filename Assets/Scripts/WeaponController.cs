﻿using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public static int weapon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weapon = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weapon = 2;
            }

	}
}
