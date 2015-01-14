using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]

public class MagicTowerScript : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    private List<GameObject> enemysInRange;
   	private GameObject bullet;
    private int bulletSpeed;
    private float fireRate;
	private string enemyTag;
	private AudioClip magic;

	private static GameObject parent;
    
    GameObject enemy;
    Vector3 enemyVel;
	bool isShooting;
    //CharacterController enemyCharController;


    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
        enemysInRange = new List<GameObject>();
		isShooting = false;
		bullet = resourceManager.magicTowerBullet;
		magic = resourceManager.magicBulletSound;
		enemyTag = resourceManager.enemyTag;
		bulletSpeed = resourceManager.bulletSpeed;
		TowerStats stats = transform.parent.GetComponent<TowerStats> ();
		fireRate = stats.speed;
		InvokeRepeating("Shooting", 0f, 1/fireRate);
		scaleRange (stats.range);
		stats.sellCost = resourceManager.costMagicTower / 2;
		stats.upgradeCost = resourceManager.costMagicTower;
		stats.attackUpgrade = stats.attack / 2;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == (enemyTag))

        {
            enemysInRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == (enemyTag))

        {
            enemysInRange.Remove(col.gameObject);
        }
    }

    void Update()
    {
		for (int i = 0; i < enemysInRange.Count; i++) {
            EnemyResources enemyResources;
            if (enemysInRange[i] != null)
            {
                enemyResources = enemysInRange[i].collider.GetComponent<EnemyResources>();
            }
            else
            {
                enemyResources = null;
            }

            if (enemyResources == null || enemyResources.isDead)
            {
				enemysInRange.Remove (enemysInRange [i]);
			}
		}


		if (enemysInRange.Count == 0) {
			CancelInvoke ();
			isShooting = false;
		} else if(!isShooting) {
			TowerStats stats = transform.parent.GetComponent<TowerStats> ();
			fireRate = stats.speed;
			InvokeRepeating ("Shooting", 0f, 1 / fireRate);
			isShooting = true;
		}
    }

    void Shooting()
    {

        if (enemysInRange.Count > 0) 
        {
            RaycastHit hit;
            int i = 0;
            bool justHit;
            do
            {

                if (enemysInRange.Count == 0)
                {
                    return;
                }

                if (enemysInRange[i] != null)
                {
					justHit = Physics.Raycast(transform.parent.position, enemysInRange[i].transform.position - transform.parent.position, out hit,Mathf.Infinity,CameraController.ignoreMask);
                }

                else
                {

                    while (enemysInRange.Count > i && enemysInRange[i] == null)
                    {
                        enemysInRange.Remove(enemysInRange[i]);
                    }

                    if(enemysInRange.Count>i)
                        {
						justHit = Physics.Raycast(transform.parent.position, enemysInRange[i].transform.position - transform.parent.position, out hit,Mathf.Infinity,CameraController.ignoreMask);
                        }
                    else
                    {
                        return;

                    }

                }
               
                enemy = enemysInRange[i];
                //enemyCharController = enemy.GetComponent<CharacterController>();

                i++;

            } while (justHit && hit.collider.tag != enemyTag && i < enemysInRange.Count);


            i--;
			Vector3 toTarget = enemy.transform.position - transform.parent.position;
            enemyVelocity(enemy);

            float a = Vector3.Dot(enemyVel, enemyVel) - (bulletSpeed * bulletSpeed);
            float b = 2 * Vector3.Dot(enemyVel, toTarget);
            float c = Vector3.Dot(toTarget, toTarget);

            float d = (b * b) - 4 * a * c;

            if (d < 0)
                return;

            float t1 = (-b - Mathf.Sqrt(d)) / (2 * a);
            float t2 = (-b + Mathf.Sqrt(d)) / (2 * a);

            float t;

            if (t1 > t2 && t2 > 0)
            {
                t = t2;
            }
            else
            {
                t = t1;
            }

            Vector3 target = enemy.transform.position + enemyVel * t;
			Vector3 shootDir = (target - transform.parent.position).normalized;
            Vector3 Shoot = shootDir * bulletSpeed;

			GameObject Bullet = (GameObject)Instantiate(bullet, transform.parent.position, Quaternion.identity);
            Bullet.rigidbody.velocity = Shoot;
			Bullet.GetComponent<MagicTowerBulletScript> ().damagePerShot = transform.parent.GetComponent<TowerStats> ().attack;
			audio.PlayOneShot(magic,15f);

        }
    }

    void enemyVelocity(GameObject enemy)
    {
		enemyVel = enemy.rigidbody.velocity;

        //enemyVel = enemyCharController.velocity;

    }

	public void scaleRange(float range){
		gameObject.transform.localScale *= range;
		gameObject.transform.localPosition = new Vector3(5 * range,0,0);
	}
}
