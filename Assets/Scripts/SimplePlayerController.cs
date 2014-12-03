using UnityEngine;
using System.Collections;

public class SimplePlayerController : MonoBehaviour {

	public int speed = 100;

	// Use this for initialization
	void Start () {
        // Sets color of player to blue
        renderer.material.color = new Color(0.0f, 0.0f, 1);
    }

	void Update () {	

		// Movement
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		transform.Translate (movement * speed * Time.deltaTime);
	}
}
