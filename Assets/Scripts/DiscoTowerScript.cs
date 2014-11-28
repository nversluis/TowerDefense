using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscoTowerScript : MonoBehaviour {

    private List<GameObject> enemysInRange;
    public GameObject bullet;
    public int bulletSpeed;
    public float coolDownTime;
	public string enemyTag;
    private Vector3 prevLoc;
    float timeSince;
    GameObject enemy;
    Vector3 enemyVel;


    void Start()
    {
        enemysInRange = new List<GameObject>();
        InvokeRepeating("Shooting", 0f, coolDownTime);
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

    }

    void Shooting()
    {
		Debug.Log (enemysInRange.Count);
        if (enemysInRange.Count > 0) 
        {
            RaycastHit hit;
            int i = 0;
            bool justHit;
            do
            {
                if (enemysInRange.Contains(BulletController.hitObject))
                {
                    enemysInRange.Remove(BulletController.hitObject);
                    return;
                }

                if (enemysInRange.Count == 0)
                {
                    return;
                }

                if (enemysInRange[i] != null)
                {
                    justHit = Physics.Raycast(transform.position, enemysInRange[i].transform.position - transform.position, out hit);
                }

                else
                {
                    enemysInRange.Remove(enemysInRange[i]);
                    return;
                }
                if (enemy == null || !enemy.Equals(enemysInRange[i]))
                {
                    enemy = enemysInRange[i];
                    enemyVel = EnemyVelocity(enemy);
                    return;
                    }

                i++;

            } while (justHit && hit.collider.tag != enemyTag && i < enemysInRange.Count);

            i--;
            Vector3 toTarget = enemy.transform.position - transform.position;
            enemyVel = EnemyVelocity(enemy);
            Vector3 enemyDir = (enemyVel.normalized);

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
            Vector3 shootDir = (target - transform.position).normalized;
            Vector3 Shoot = shootDir * bulletSpeed;
            if (Physics.Raycast(transform.position, Shoot, out hit)) 
            {
                GameObject Bullet = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
                Bullet.rigidbody.velocity = Shoot;
            }
            //Debug.Log(enemyVel);
            //Debug.Log(t);
            //Debug.Log(enemyVel * t);

            Debug.DrawRay(transform.position, Shoot);

        }
    }

    Vector3 EnemyVelocity(GameObject enemy)
    {
        float timeNow = Time.realtimeSinceStartup;
        Vector3 curLoc = enemy.transform.position;
        Vector3 velocity = (curLoc - prevLoc) / (timeNow - timeSince);
        timeSince = Time.realtimeSinceStartup;
        prevLoc = curLoc;
        return velocity;

    }
}
