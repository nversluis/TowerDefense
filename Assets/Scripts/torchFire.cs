using UnityEngine;
using System.Collections;

public class torchFire : MonoBehaviour {
//script dat het vuur laat flikkeren
	// Update is called once per frame
	private float startlifetime; //start lifetime of flame
	private float startIntensity; //start intensity of the lamp
	private float shakyness;
	void Start(){
		startlifetime = transform.transform.GetChild (1).light.intensity;
		startIntensity = transform.transform.GetChild (0).particleSystem.startSpeed;
		shakyness = 0.04f;
		InvokeRepeating ("MoveLight", 0, 0.1f);
	}


	void MoveLight () {
		//Debug.Log (NormalDist (orPos.x, shakyness));
		float x = NormalDist ();
		transform.transform.GetChild (1).light.intensity = startIntensity * (1 + 5*x);
		transform.transform.GetChild (0).particleSystem.startSpeed = startlifetime*(1 + x);

	}




	private float NormalDist(){
		float u1 =	Random.Range (0.001f, 100);
		float u2 = Random.Range (0.001f, 100);
		float randStdNormal = Mathf.Sqrt (-2 * Mathf.Log (u1/100)) * Mathf.Sin (2 * Mathf.PI * u2/100);
		//Debug.Log (u1);
		return shakyness * randStdNormal;
	}
}
