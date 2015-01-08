using UnityEngine;
using System.Collections;

public class BoomParticleScript : MonoBehaviour
{
    public GameObject Hit;
    void Start()
    {
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
