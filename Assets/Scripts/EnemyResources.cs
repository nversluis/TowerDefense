using UnityEngine;
using System.Collections;

public class EnemyResources : MonoBehaviour {

    public bool walking = true;
    public bool attacking = false;
    public bool isDead = false;
	public float isSlowed;
    public float dieDistance;
    
    public int attackDamage;
    public int totalDamage;
	public int totalGateDamage;

	public GameObject targetBarricade;
    
}
