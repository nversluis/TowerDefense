using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour {

	public float planewidth; //Size of the planes
	public float height;
	public float nodeSize; //Distance between nodepoints

	public GameObject planePrefab; //Floor prefab
	public GameObject wallPrefab; //Wall prefab
	public GameObject node; //Node prefab
	public GameObject EnemySpawner;
	public GameObject Minimapcamera;
	public GameObject Gate;
	public GameObject torch;
	public Camera cam;

	private ArrayList positions = new ArrayList(); //Positions of the floors
	private List<Vector3> NodesPos = new List<Vector3>(); //Positions of the waypoints/nodes
	public static List<WayPoint> Nodes = new List<WayPoint>(); //List with all Nodes
	private int length;
	private int width;
	private List<GameObject> floors = new List<GameObject> ();

	public InputField lengthInput;
	public InputField widthInput;
	public Button SubmitButton;
	// Use this for initialization

	void Start () {
		SubmitButton.onClick.AddListener(delegate{submitSize();});
	}

	// Update is called once per frame
	void Update () {	

			int length1;
			int width1;
			bool resultLength = int.TryParse (lengthInput.text, out length1);
			bool resultWidth = int.TryParse (widthInput.text, out width1);
		if (resultWidth && resultLength&&int.Parse(lengthInput.text)>0&&int.Parse(widthInput.text)>0) {
			SubmitButton.GetComponentInChildren<Text> ().text = "Confirm/Reset";
			SubmitButton.enabled = true;
		}
		else {
			SubmitButton.enabled = false;
			SubmitButton.GetComponentInChildren<Text> ().text = "Positive Integers Only";
		}


	}

	private void submitSize(){

		length = int.Parse(lengthInput.text);
		width = int.Parse(widthInput.text);
		if (length < 1 || width < 1) {
			SubmitButton.gameObject.transform.renderer.material.color = Color.red;
		} else {

			SubmitButton.GetComponentInChildren<Text> ().text = "Reset";
			foreach (GameObject floor1 in floors)
				Destroy (floor1);

			for (int l = 0; l < length; l++) {
				for (int w = 0; w < width; w++) {
					GameObject floor = (GameObject)Instantiate (planePrefab, new Vector3 (l * planewidth, 0, w * planewidth), Quaternion.identity);
					floors.Add (floor);
				}
			}
			float tempL = length - 1;
			float tempW = width - 1;
			cam.transform.position = new Vector3 (tempL / 2, 1, tempW / 2) * planewidth;
			cam.orthographicSize = Mathf.Max (length, width) * planewidth / 3 * 2;
		}
	}
}
