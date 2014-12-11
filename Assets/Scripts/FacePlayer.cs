using UnityEngine;
using System.Collections;

public class FacePlayer : MonoBehaviour {
	public GameObject PlayerPrefab;
	// Use this for initialization
	void Start () {
		PlayerPrefab = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation=Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,-(PlayerPrefab.transform.position-transform.position),6,0));
		transform.position += new Vector3 (0, 1, 0)/10;
	}

	void OnEnable(){
		InvokeRepeating("DestroyAfterSeconds", 0.5f, 0.3F);
	}

	void DestroyAfterSeconds(){
		Destroy (gameObject);
		CancelInvoke();
	}

}