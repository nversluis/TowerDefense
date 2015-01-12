using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagicTowerBulletScript : MonoBehaviour
{
	public int damagePerShot;// = 1500;
    Transform Player;
    Vector3 PrevItLoc;
    public static float maxBulletDistance = 200;
    public GameObject Boom;
    LayerMask ignoreMask = ~(1 << 13);
    bool headshot;

    void GotThrough()
    {

        RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            Destroy(this.gameObject);
            GameObject boom = (GameObject)Instantiate(Boom, PrevItLoc, Quaternion.identity);
            if (hit.rigidbody != null)
            {
                boom.rigidbody.velocity = hit.collider.rigidbody.velocity;
                boom.GetComponent<BoomParticleScript>().Hit = hit.collider.gameObject;
            }
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, "magic", false);
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
