using UnityEngine;
using System.Collections;

public class DiscoTowerScript : MonoBehaviour {


    public bool curTargeting = false;
    private GameObject target;
    public GameObject magicBullet;
    public int counter;

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Bidarro" && !curTargeting)
        {
            target = other.gameObject;
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        target = other.gameObject;
        if (curTargeting)
        {
     
        }
    }

  

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bidarro" && curTargeting)
        {

            curTargeting = false;
        }
    }

	// Use this for initialization
	void Start () {

        InvokeRepeating("Shooting", 3f, 1f);
	}

    void Shooting()
    {
        GameObject bullet = (GameObject)Instantiate(magicBullet, transform.position, Quaternion.identity);
        Debug.DrawLine(target.transform.position, transform.position);
        bullet.rigidbody.AddForce((target.transform.position - transform.position).normalized * 100);
    }
}
