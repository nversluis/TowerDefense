using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEditorPlane : MonoBehaviour
{

	private ArrayList positions = new ArrayList ();
	//Positions of the floors

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private float planewidth;

	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planewidth = resourceManager.planewidth;

	}

	void OnMouseOver ()
	{
		if (gameObject.renderer.material.color == Color.white) {
			gameObject.renderer.material.color = Color.green;
		}
		if (Input.GetMouseButton (0))
			addPos ();

		if (Input.GetMouseButton (1))
			removePos ();
	}

	void OnMouseExit ()
	{
		if (gameObject.renderer.material.color == Color.green)
			gameObject.renderer.material.color = Color.white;

	}


	private void addPos ()
	{

		if (gameObject.renderer.material.color == Color.green) {
			gameObject.renderer.material.color = Color.black;
			LevelEditor.addPos (new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z));

		

			if (LevelEditor.type == 0 && LevelEditor.amountOfStarts == 0 && gameObject.transform.position.x == 0) { //check if it can be a start position
				gameObject.renderer.material.color = Color.blue;
				LevelEditor.amountOfStarts++;
				LevelEditor.startPos3 = transform.position / planewidth;
				LevelEditor.posConnected.Add (transform.position / planewidth);
				LevelEditor.startPlane = gameObject;
				LevelEditor.convertAround (LevelEditor.startPlane, LevelEditor.posConnected, LevelEditor.allPos, positions, planewidth, resourceManager.length);
				//LevelEditor.Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, planewidth, resourceManager.length);

			} else if (LevelEditor.type == 1 && LevelEditor.amountOfEnds == 0 && resourceManager.length - 1 == gameObject.transform.position.x / planewidth) { //check if it can be an end position
				gameObject.renderer.material.color = Color.blue;
				LevelEditor.amountOfEnds++;
				LevelEditor.endPos3 = transform.position / planewidth;
				LevelEditor.endPlane = gameObject;
			}

			if (LevelEditor.checkConnected (gameObject, LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, planewidth, resourceManager.length)) {
				LevelEditor.posConnected.Add (transform.position / planewidth);
				if(gameObject!=LevelEditor.startPlane&&gameObject!=LevelEditor.endPlane)
				gameObject.renderer.material.color = Color.yellow;
			}
		}
	}

	private void removePos ()
	{
		if (gameObject == LevelEditor.startPlane) {
			LevelEditor.startPlane = null;
			LevelEditor.amountOfStarts--;
		}
		if (gameObject == LevelEditor.endPlane) {
			LevelEditor.amountOfEnds--;
			LevelEditor.endPlane = null;
		}
		gameObject.renderer.material.color = Color.white;
		LevelEditor.removePos (new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z));
		LevelEditor.Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, planewidth, resourceManager.length);
	}


	
	// Update is called once per frame
	void Update ()
	{
	
	}


}
