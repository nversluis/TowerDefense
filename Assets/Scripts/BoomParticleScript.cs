using UnityEngine;
using System.Collections;

public class BoomParticleScript : MonoBehaviour
{
    public GameObject Hit;
    ResourceManager resourceManager;
    AudioClip bulletHit;
    AudioSource Audio;
    float volume;

    void Start()
    {
        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        bulletHit = resourceManager.bulletHit;
        Audio = GetComponent<AudioSource>();
        
        Audio.pitch=Random.Range(0.3f, 1f);
        Audio.PlayOneShot(bulletHit,3f*volume);
        Invoke("DeleteBoom", 0.2f);
        Invoke("DeleteThis", 1f);
    }

    void DeleteBoom()
    {
        Destroy(GetComponent<ParticleSystem>());
    }

    void DeleteThis()
    {
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (Hit!= null && Hit.rigidbody != null)
            rigidbody.velocity = Hit.rigidbody.velocity;
    }
}
