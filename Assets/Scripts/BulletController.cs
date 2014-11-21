using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    Transform Player;
    string hitObject;
    Vector3 PrevItLoc;
    public static float maxBulletDistance = 200;

    void GotThrough()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude))
        {
            Destroy(this.gameObject);
            if (hit.collider.name == "biddaro")
            {
                Destroy(hit.collider.gameObject);
            }
        }

        PrevItLoc = transform.position;

    }

    
	// Use this for initialization
	void Start () {

        Player = GameObject.Find("Player").transform;
        PrevItLoc = transform.position;
	}

    void FixedUpdate()
    {
        GotThrough();
    }
	
	// Update is called once per frame
	void Update () {

        if ((Player.position-transform.position).magnitude > 200)
        {
            Destroy(this.gameObject);
        }
	
	}
}
