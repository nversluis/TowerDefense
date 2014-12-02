using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscoTowerScript : MonoBehaviour {

    private List<GameObject> enemysInRange;
    public GameObject bullet;
    public int bulletSpeed;
    public float coolDownTime;
	public string enemyTag;
    
    GameObject enemy;
    Vector3 enemyVel;
    CharacterController EnemyCharController;


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
                    justHit = Physics.Raycast(transform.position, enemysInRange[i].transform.position - transform.position, out hit);
                }

                else
                {

                    while (enemysInRange.Count > i && enemysInRange[i] == null)
                    {
                        enemysInRange.Remove(enemysInRange[i]);
                    }

                    if(enemysInRange.Count>i)
                        {
                            justHit = Physics.Raycast(transform.position, enemysInRange[i].transform.position - transform.position, out hit);
                        }
                    else
                    {
                        return;

                    }

                }
               
                enemy = enemysInRange[i];
                EnemyCharController = enemy.GetComponent<CharacterController>();

                i++;

            } while (justHit && hit.collider.tag != enemyTag && i < enemysInRange.Count);


            i--;
            Vector3 toTarget = enemy.transform.position - transform.position;
            EnemyVelocity(enemy);
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

            GameObject Bullet = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet.rigidbody.velocity = Shoot;
            Debug.DrawRay(transform.position, Shoot);


        }
    }

    void EnemyVelocity(GameObject enemy)
    {


        enemyVel = EnemyCharController.velocity;

    }
}
