using UnityEngine;
using System.Collections;

public class LevelEditorPlane : MonoBehaviour {

	private ArrayList positions = new ArrayList(); //Positions of the floors

	void OnMouseOver(){
		if (gameObject.renderer.material.color != Color.black) {
			gameObject.renderer.material.color = Color.green;
			if (Input.GetMouseButton (0))
				addPos ();
		}
		if (Input.GetMouseButton (1))
			removePos ();
	}

	void OnMouseExit(){
		if(gameObject.renderer.material.color != Color.black)
		gameObject.renderer.material.color = Color.white;

	}


	private void addPos(){
		gameObject.renderer.material.color = Color.black;
		LevelEditor.addPos(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z));

	}

	private void removePos(){
		gameObject.renderer.material.color = Color.white;
		LevelEditor.removePos(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
