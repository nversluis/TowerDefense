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
	private Color Cstart;
    private Color Cend;
	private Color CConnected;
	private Color CNotConnected;
	private Color CHighlighted;
    bool highlighted;

	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planewidth = resourceManager.planewidth;
		CnoPlane = resourceManager.noPlane;
		Cstart = resourceManager.start;
        Cend = resourceManager.end;
		CConnected = resourceManager.connected;
		CNotConnected = resourceManager.notConnected;
		CHighlighted = resourceManager.highlighted;

	}

	void OnMouseEnter ()
	{

	}

    void OnMouseOver()
    {
        if (LevelEditor.editing)
        {
            if (Input.GetMouseButtonDown(2))
                Debug.Log(LevelEditor.posConnected.Contains(transform.position / planewidth));

            if (gameObject.renderer.material.color == CnoPlane)
            {
                if (LevelEditor.type == 2)
                {
                    Color currentColor = CConnected;
                    currentColor.a = 0.80f;
                    gameObject.renderer.material.color = currentColor;
                    highlighted = true;
                }
                else if (LevelEditor.type == 1)
                {
                    Color currentColor = Cend;
                    currentColor.a = 0.80f;
                    gameObject.renderer.material.color = currentColor;
                    highlighted = true;
                }
                else if (LevelEditor.type == 0)
                {
                    Color currentColor = Cstart;
                    currentColor.a = 0.80f;
                    gameObject.renderer.material.color = currentColor;
                    highlighted = true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (LevelEditor.type < 3)
                {
                    addPos();
                    LevelEditor.mapSaved = false;
                }
                else
                    removePos();
            }

            if (Input.GetMouseButton(1))
                removePos();

        }
    }
	void OnMouseExit ()
	{
        if (LevelEditor.editing)
        {
            if (highlighted == true)
                gameObject.renderer.material.color = CnoPlane;
            highlighted = false;
        }
	}


	private void addPos ()
	{
		if (highlighted) 
        {
			LevelEditor.addPos (new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z));

            if ((LevelEditor.type == 0 && LevelEditor.amountOfStarts == 0) && (gameObject.transform.position.x == 0 || gameObject.transform.position.z == 0 || transform.position.x / planewidth + 1 == (resourceManager.length) || transform.position.z / planewidth + 1 == (resourceManager.width)))
            { //check if it can be a start position
                highlighted = false;
				gameObject.renderer.material.color = Cstart;
				LevelEditor.amountOfStarts++;
				LevelEditor.startPos3 = transform.position / planewidth;
				resourceManager.startPos = new Vector2 (LevelEditor.startPos3.x, LevelEditor.startPos3.z);
				LevelEditor.posConnected.Add (transform.position / planewidth);
				LevelEditor.startPlane = gameObject;
                LevelEditor.Recalculate();

			}

            else if ((LevelEditor.type == 1 && LevelEditor.amountOfEnds == 0) && (gameObject.transform.position.x == 0 || gameObject.transform.position.z == 0 || transform.position.x / planewidth + 1 == (resourceManager.length) || transform.position.z / planewidth + 1 == (resourceManager.width)))
            { //check if it can be an end position
                highlighted = false;
				gameObject.renderer.material.color = Cend;
				LevelEditor.amountOfEnds++;
				LevelEditor.endPos3 = transform.position / planewidth;
				LevelEditor.endPlane = gameObject;
				resourceManager.endPos = new Vector2 (LevelEditor.endPos3.x, LevelEditor.endPos3.z);
                LevelEditor.Recalculate();

			}

            else if (LevelEditor.type == 0 && (LevelEditor.amountOfStarts > 0))
            {
                LevelEditor.setErrorTekst("You can only place one start position");
            }

            else if (LevelEditor.type == 0 && !(gameObject.transform.position.x == 0 || gameObject.transform.position.z == 0 || transform.position.x / planewidth + 1 == (resourceManager.length) || transform.position.z / planewidth + 1 == (resourceManager.width)))
            {
                LevelEditor.setErrorTekst("Place the start at the border of the plane");
            }

            else if (LevelEditor.type == 1 && (LevelEditor.amountOfEnds > 0))
            {
                LevelEditor.setErrorTekst("You can only place one end position");
            }

            else if (LevelEditor.type == 1 && !(gameObject.transform.position.x == 0 || gameObject.transform.position.z == 0 || transform.position.x / planewidth + 1 == (resourceManager.length) || transform.position.z / planewidth + 1 == (resourceManager.width)))
            {
                LevelEditor.setErrorTekst("Place the end at the border of the plane");
            }

            else if(LevelEditor.type == 2){
                gameObject.renderer.material.color = CNotConnected;
                highlighted = false;

				if (LevelEditor.checkConnected (gameObject)) {
                    LevelEditor.posConnected.Add (transform.position / planewidth);
					if (gameObject != LevelEditor.startPlane && gameObject != LevelEditor.endPlane) {
						gameObject.renderer.material.color = CConnected;
					}
				}
                LevelEditor.Recalculate();


            }
		}
	}

	private void removePos ()
	{

        if (!highlighted)
        {
            if (gameObject == LevelEditor.startPlane)
            { //remove start position
                LevelEditor.startPlane = null;
                LevelEditor.amountOfStarts--;
                resourceManager.startPos = new Vector2();
                LevelEditor.startPos3 = new Vector3();
            }
            if (gameObject == LevelEditor.endPlane)
            { //remove end position
                LevelEditor.amountOfEnds--;
                LevelEditor.endPlane = null;
                LevelEditor.endPos3 = new Vector3();
                resourceManager.endPos = new Vector2();
            }
            gameObject.renderer.material.color = CnoPlane;
            LevelEditor.removePos(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));
            LevelEditor.Recalculate();
        }
	}
}
