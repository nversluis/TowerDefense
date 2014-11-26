using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscoTowerScript : MonoBehaviour {

    private List<GameObject> enemysInRange;
    public GameObject bullet;
    public int bulletSpeed;
    public float shootSpeed;
    private Vector3 previous;
    private Vector3 current;
    private Vector3 speed;
    float begin;


    void Start()
    {
        enemysInRange = new List<GameObject>();
        InvokeRepeating("Shooting", 0f, shootSpeed);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Bidarro"))
        {
            enemysInRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == ("Bidarro"))
        {
            enemysInRange.Remove(col.gameObject);
        }
    }

    void Update()
    {
        if (enemysInRange.Contains(BulletController.hitObject))
        {
            enemysInRange.Remove(BulletController.hitObject);
        }
    }

    void Shooting()
    {
        if (enemysInRange.Contains(BulletController.hitObject))
        {
            enemysInRange.Remove(BulletController.hitObject);
        }
        if (enemysInRange.Count > 0) 
        {
            RaycastHit hit;
            int i = 0;
            bool justHit;
            do{
                if (enemysInRange.Contains(BulletController.hitObject))
                {
                    enemysInRange.Remove(BulletController.hitObject);
                    return;
                }

                if (enemysInRange.Count == 0)
                {
                    return;
                }
                Debug.Log(i);
                justHit = Physics.Raycast(transform.position, enemysInRange[i].transform.position - transform.position,out hit);

                i++;

            } while(justHit && hit.collider.tag != "Bidarro" && i<enemysInRange.Count);
            i--;
            current = enemysInRange[i].transform.position;
            float eind = Time.realtimeSinceStartup;
            speed = (current - previous);
            Debug.DrawRay(transform.position, (hit.point - transform.position) + speed);
            GameObject Bullet = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 dir = (hit.point - transform.position) + (speed)*(hit.point -transform.position).magnitude/2;
            
            Bullet.rigidbody.AddForce(dir.normalized*bulletSpeed);
            Debug.Log(dir);

            previous = enemysInRange[i].transform.position;
            begin = Time.realtimeSinceStartup;
               
        }
    }
}
