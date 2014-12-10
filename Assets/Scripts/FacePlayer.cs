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
	}

	void OnEnable(){
		InvokeRepeating("DestroyAfterSeconds", 0.5f, 0.3F);
	}

	void DestroyAfterSeconds(){
		gameObject.SetActive (false);
		CancelInvoke();
	}

}