using UnityEngine;
using System.Collections;


public class ExplosionScript : MonoBehaviour {

    AudioSource audioBoom;
    AudioClip kaboom;

	// Use this for initialization
	void Start () {
        float volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        audioBoom = GetComponent<AudioSource>();
        kaboom = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().kaboom;
        audioBoom.PlayOneShot(kaboom, volume);
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
