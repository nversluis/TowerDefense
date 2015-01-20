using UnityEngine;
using System.Collections;

public class GwarfBulletScript : MonoBehaviour
{
    public int damagePerShot;
    Transform Player;
    public GameObject gwarf;
    Vector3 PrevItLoc;
    public static float maxBulletDistance = 200;
    public static GameObject hitObject;
    EnemyResources enemyResources;
    public GameObject Boom;
    LayerMask ignoreMask = ~(1 << 13);

    void GotThrough()
    {

        RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            Destroy(this.gameObject);
			//Debug.Log (damagePerShot);
            GameObject boom = (GameObject)Instantiate(Boom, PrevItLoc, Quaternion.identity);
            if (hit.rigidbody != null)
            {
                boom.rigidbody.velocity = hit.collider.rigidbody.velocity;
                boom.GetComponent<BoomParticleScript>().Hit = hit.collider.gameObject;
            }
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerShot);
				gwarf.GetComponent<EnemyResources> ().totalDamage += damagePerShot;
			} else if(hit.transform.name.Contains("arricade")) {
				hit.transform.gameObject.GetComponent<barricade> ().TakeDamage(damagePerShot);
				gwarf.GetComponent<EnemyResources> ().totalDamage += damagePerShot;
			} else if(hit.transform.name.Contains("oal")) {
				hit.transform.gameObject.GetComponent<GoalScript> ().removeLife(damagePerShot);
				gwarf.GetComponent<EnemyResources> ().totalGateDamage += damagePerShot;
			}
        }
        PrevItLoc = transform.position;
    }


    // Use this for initialization
    void Awake()
    {
        Player = GameObject.Find("Player").transform;
		PrevItLoc = transform.position;
		//gwarf = gameObject.transform.parent;
		if (gwarf != null) {
			//damagePerShot = gwarf.GetComponent<GwarfAttack> ().attackDamage;        
			enemyResources = gwarf.GetComponent<EnemyResources> ();
		}
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
