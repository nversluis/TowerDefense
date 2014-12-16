using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
	private int damagePerShot = 1500;
	Transform Player;
	Vector3 PrevItLoc;
	public static float maxBulletDistance = 200;
	public static GameObject hitObject;

    LayerMask ignoreMask = ~(1 << 13);

	void GotThrough()
	{

		RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
		{
			EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
			Destroy(this.gameObject);
			if (enemyHealth != null)
			{
				//Debug.Log("Ik hit de enemy!");
				//Debug.Log("Current enemy health: " + enemyHealth.currentHealth);
				enemyHealth.TakeDamage(damagePerShot,"magic");
			}

			/*hitObject = hit.collider.gameObject;
            Destroy(this.gameObject);
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("Current enemy health: " + enemyHealth.currentHealth);
                //Destroy(hit.collider.gameObject);
                if (enemyHealth.currentHealth > 0)
                {
                    enemyHealth.TakeDamage(damagePerShot);
                }
            }*/

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
