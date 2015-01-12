using UnityEngine;
using System.Collections;

public class BoomParticleScript : MonoBehaviour
{
    public GameObject Hit;
    ResourceManager resourceManager;
    AudioClip bulletHit;
    AudioSource Audio;

    void Start()
    {
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        bulletHit = resourceManager.bulletHit;
        Audio = this.gameObject.GetComponent<AudioSource>();
        Audio.PlayOneShot(bulletHit,3f);
        Invoke("DeleteBoom", 0.1f);
    }

    void DeleteBoom()
    {
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (Hit!= null && Hit.rigidbody != null)
            rigidbody.velocity = Hit.rigidbody.velocity;
    }
}
