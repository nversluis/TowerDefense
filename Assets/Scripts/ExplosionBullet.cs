using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExplosionBullet : MonoBehaviour
{
    private int damagePerShot = 20000;
    Transform Player;
    Vector3 PrevItLoc;
    public static float maxBulletDistance = 200;
    public static GameObject hitObject;
    GameObject Explosion;
    LayerMask ignoreMask = ~((1 << 13) | (1 << 2));
    LayerMask enemys = (1 << 12);
    bool headshot;

    void GotThrough()
    {

        RaycastHit hit;
        if (Physics.Raycast(PrevItLoc, transform.position - PrevItLoc, out hit, (PrevItLoc - transform.position).magnitude + 0.2f, ignoreMask))
        {
            Debug.Log(hit.transform.name);
            GameObject boom = (GameObject)Instantiate(Explosion, PrevItLoc, Quaternion.identity);
            Collider[] hitCollider = Physics.OverlapSphere(transform.position, 10f, enemys);
            foreach (Collider collide in hitCollider)
            {
                EnemyHealth enemyHealth = collide.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    
                    enemyHealth.TakeDamage((int)(damagePerShot/(Vector3.Distance(transform.position, collide.transform.position))), "magic", true);
                    Debug.Log(Vector3.Distance(transform.position, collide.transform.position));
                    if (hit.collider.Equals(hit.collider.GetComponent<BoxCollider>()))
                    {
                        enemyHealth.HeadShot();
                    }
                }
            }

            Destroy(this.gameObject);

        }
        PrevItLoc = transform.position;
    }




    // Use this for initialization
    void Start()
    {

        Player = GameObject.Find("Player").transform;
        PrevItLoc = transform.position;
        Explosion = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().Explosion;

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
