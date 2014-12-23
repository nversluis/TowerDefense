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
	private Color CnoPlane;
	private Color CstartEnd;
	private Color CConnected;
	private Color CNotConnected;
	private Color CHighlighted;

	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planewidth = resourceManager.planewidth;
		CnoPlane = resourceManager.noPlane;
		CstartEnd = resourceManager.startOrEnd;
		CConnected = resourceManager.connected;
		CNotConnected = resourceManager.notConnected;
		CHighlighted = resourceManager.highlighted;

	}

	void OnMouseEnter(){

	}

	void OnMouseOver ()
	{
		if (!LevelEditor.loadScreenOpen) {
			if (Input.GetMouseButtonDown (2))
				Debug.Log (LevelEditor.posConnected.Contains (transform.position / planewidth));

			if (gameObject.renderer.material.color == CnoPlane) {
				gameObject.renderer.material.color = CHighlighted;
			}
			if (Input.GetMouseButton (0))
				addPos ();

			if (Input.GetMouseButton (1))
				removePos ();
		}
	}

	void OnMouseExit ()
	{
		if (gameObject.renderer.material.color == CHighlighted)
			gameObject.renderer.material.color = CnoPlane;

	}


	private void addPos ()
	{
		if (!LevelEditor.loadScreenOpen) {
			if (gameObject.renderer.material.color == CHighlighted) {
				gameObject.renderer.material.color = CNotConnected;
				LevelEditor.addPos (new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z));

		

				if (LevelEditor.type == 0 && LevelEditor.amountOfStarts == 0 && gameObject.transform.position.x == 0) { //check if it can be a start position
					gameObject.renderer.material.color = CstartEnd;
					LevelEditor.amountOfStarts++;
					LevelEditor.startPos3 = transform.position / planewidth;
					resourceManager.startPos = new Vector2 (LevelEditor.startPos3.x, LevelEditor.startPos3.z);
					LevelEditor.posConnected.Add (transform.position / planewidth);
					LevelEditor.startPlane = gameObject;
					LevelEditor.convertAround (LevelEditor.startPlane, LevelEditor.posConnected, LevelEditor.allPos, positions, planewidth, resourceManager.length, resourceManager.width, CstartEnd, CNotConnected, CConnected);
					//LevelEditor.Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, planewidth, resourceManager.length);

				} else if (LevelEditor.type == 1 && LevelEditor.amountOfEnds == 0 && resourceManager.length - 1 == gameObject.transform.position.x / planewidth) { //check if it can be an end position
					gameObject.renderer.material.color = CstartEnd;
					LevelEditor.amountOfEnds++;
					LevelEditor.endPos3 = transform.position / planewidth;
					LevelEditor.endPlane = gameObject;
					resourceManager.endPos = new Vector2 (LevelEditor.endPos3.x, LevelEditor.endPos3.z);
				}

				if (LevelEditor.checkConnected (gameObject, LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, resourceManager.planewidth, resourceManager.length, resourceManager.width, CConnected, CNotConnected, CstartEnd)) {
					LevelEditor.posConnected.Add (transform.position / planewidth);
					if (gameObject != LevelEditor.startPlane && gameObject != LevelEditor.endPlane) {
						gameObject.renderer.material.color = CConnected;
					}
				}
			}
		}
	}

	private void removePos ()
	{
		if (gameObject == LevelEditor.startPlane) { //remove start position
			LevelEditor.startPlane = null;
			LevelEditor.amountOfStarts--;
			resourceManager.startPos = new Vector2();
			LevelEditor.startPos3 = new Vector3();
		}
		if (gameObject == LevelEditor.endPlane) { //remove end position
			LevelEditor.amountOfEnds--;
			LevelEditor.endPlane = null;
			LevelEditor.endPos3 = new Vector3();
			resourceManager.endPos =new Vector2();
		}
		gameObject.renderer.material.color = CnoPlane;
		LevelEditor.removePos (new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z));
		LevelEditor.Recalculate (LevelEditor.posConnected, LevelEditor.allPos, LevelEditor.positions, planewidth, resourceManager.length,resourceManager.width, CConnected, CNotConnected, CstartEnd);
	}


	
	// Update is called once per frame
	void Update ()
	{
	
	}


}
