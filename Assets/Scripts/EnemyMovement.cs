using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{
    public Transform gate;
    public Transform player;
    public Transform tower;
    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;

    public int randomInt;
    public float gateDistance;
    public float playerDistance;
    public float towerDistance;

    NavMeshAgent nav;

    void Awake()
    {
        //gate = GameObject.FindGameObjectWithTag("Gate").transform;
        player = GameObject.Find("Player").transform;
        //tower = GameObject.FindGameObjectWithTag("Tower").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () 
    {
        nav.speed = 20;
	}
	
	// Update is called once per frame
	void Update () {

        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        //if (playerHealth.currentHealth > 0)
       // {
            //gateDistance = Vector3.Distance(gate.transform.position, this.transform.position);
            playerDistance = Vector3.Distance(player.transform.position, this.transform.position);
            //towerDistance = Vector3.Distance(tower.transform.position, this.transform.position);

            nav.SetDestination(player.position);
       // }
        //else
        //{
        //    nav.enabled = false;
        //}
	}
}