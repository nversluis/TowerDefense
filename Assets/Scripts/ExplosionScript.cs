using UnityEngine;
using System.Collections;


public class ExplosionScript : MonoBehaviour {

    AudioSource audio;
    AudioClip kaboom;

	// Use this for initialization
	void Start () {
        audio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        kaboom = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().kaboom;
        audio.PlayOneShot(kaboom,2);
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
