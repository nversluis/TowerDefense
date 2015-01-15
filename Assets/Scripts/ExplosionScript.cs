using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyExplosion());
	}
    IEnumerator DestroyExplosion()
    {

        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
