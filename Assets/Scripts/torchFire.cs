using UnityEngine;
using System.Collections;

public class torchFire : MonoBehaviour {
//script dat het vuur laat flikkeren
	// Update is called once per frame
	private float startlifetime; //start lifetime of flame
	private float startIntensity; //start intensity of the lamp
	private float shakyness;
	void Start(){
		startIntensity = transform.transform.GetChild (1).light.intensity;
		startlifetime = transform.transform.GetChild (0).particleSystem.startSize;
		shakyness = 0.04f;
		InvokeRepeating ("MoveLight", 0, 0.1f);
	}


	void MoveLight () {
		//Debug.Log (NormalDist (orPos.x, shakyness));
		float x = NormalDist ();
		transform.transform.GetChild (1).light.intensity = startIntensity * (1 + 5*x);
		transform.transform.GetChild (0).particleSystem.startSize = startlifetime*(1 + 5*x);
	}


	private float NormalDist(){
		float u1 =	Random.Range (0.001f, 100);
		float u2 = Random.Range (0.001f, 100);
		float randStdNormal = Mathf.Sqrt (-2 * Mathf.Log (u1/100)) * Mathf.Sin (2 * Mathf.PI * u2/100);
		//Debug.Log (u1);
		return shakyness * randStdNormal;
	}
}
