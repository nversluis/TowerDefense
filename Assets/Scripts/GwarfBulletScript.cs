﻿using UnityEngine;
using System.Collections;

public class GwarfBulletScript : MonoBehaviour
{
    private int damagePerShot = 1500;
    Transform Player;
    Vector3 PrevItLoc;
    public static float maxBulletDistance = 200;
    public static GameObject hitObject;
    public GameObject Boom;
    LayerMask ignoreMask = ~(1 << 13);

    void GotThrough()
    {

        RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            Destroy(this.gameObject);
            GameObject boom = (GameObject)Instantiate(Boom, PrevItLoc, Quaternion.identity);
            if (hit.rigidbody != null)
            {
                boom.rigidbody.velocity = hit.collider.rigidbody.velocity;
                boom.GetComponent<BoomParticleScript>().Hit = hit.collider.gameObject;
            }
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerShot);
            }
        }
        PrevItLoc = transform.position;
    }


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        PrevItLoc = transform.position;
    }

    void FixedUpdate()
    {
        GotThrough();
    }

    // Update is called once per frame
    void Update()
    {

        if ((Player.position - transform.position).magnitude > 200)
        {
            Destroy(this.gameObject);
        }

    }
}