using UnityEngine;
using System.Collections;

public class GwarfAttack : MonoBehaviour
{
    private float timeBetweenAttacks = 2.917f / 2f;
    public int attackDamage;
    public float playerDistance;
    public int damageMultiplier = 20;

    public int totalDamage;

    public GameObject player;
    PlayerHealth playerHealth;
    EnemyResources enemyResources;
    EnemyStats enemyStats;
    bool playerInRange;
    public bool invoked = false;

    public float bulletSpeed;
    public GameObject bullet;
    public AudioClip magic;

    LayerMask allowMask = 1 << 10;

	Vector3 endLoc = new Vector3();

	GwarfScript gs;

    void Start()
    {
        totalDamage = 0;

        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        enemyStats = GetComponent<EnemyStats>();
        enemyResources = GetComponent<EnemyResources>();
        attackDamage = enemyStats.attack * damageMultiplier;
		gs = gameObject.GetComponent<GwarfScript> ();

    }

    void Attack()
    {
        Vector3 playerloc = player.transform.position + new Vector3(0, 2, 0);
        Vector3 toTarget = playerloc - transform.position;
        Vector3 playerVel = player.rigidbody.velocity;

        float a = Vector3.Dot(playerVel, playerVel) - (bulletSpeed * bulletSpeed);
        float b = 2 * Vector3.Dot(playerVel, toTarget);
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

        Vector3 target = playerloc + playerVel * t;
        Vector3 shootDir = (target - transform.position).normalized;
        Vector3 Shoot = shootDir * bulletSpeed;

        if (!Physics.Raycast(transform.position, target - transform.position, (target - transform.position).magnitude,allowMask))
        {
            GameObject Bullet = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet.rigidbody.velocity = Shoot;
            audio.PlayOneShot(magic, 15f);
        }

    }

    void Update()
    {
		if (enemyResources.attacking && playerHealth.currentHealth > 0 && !gs.attackingGoal)
        {
            if (!invoked)
            {
                InvokeRepeating("Attack", timeBetweenAttacks / 1.5f, timeBetweenAttacks);
                invoked = true;
            }

        }
        else
        {
            CancelInvoke("Attack");
            invoked = false;

        }
    }
}
