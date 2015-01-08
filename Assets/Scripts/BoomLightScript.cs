using UnityEngine;
using System.Collections;

public class BoomLightScript : MonoBehaviour {

	void Start () {
        Invoke("DeleteBoom", 0.05f);
	}
	
	void DeleteBoom () {
        Destroy(this.gameObject);
	}
}
