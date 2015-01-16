using UnityEngine;
using System.Collections;

public class StartTrigger : MonoBehaviour {

	void OnTriggerExit(Collider col)
	{
		GameObject enemy = col.gameObject;
		if (enemy.tag == "Enemy") {
			if (enemy.name == "Gwarf") {
				enemy.GetComponent<GwarfScript> ().throughGate = true;
			}
			else if (enemy.name == "Grobble") {
				enemy.GetComponent<GrobbleScript> ().throughGate = true;
			}
			else if (enemy.name == "Guyant") {
				enemy.GetComponent<GuyantScript> ().throughGate = true;
			}
		}
	}
}
