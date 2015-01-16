using UnityEngine;
using System.Collections;

public class TowerStats : MonoBehaviour {

	public int attack;
	public float speed;
	public float specialDamage; //amount of slowness for the ice tower, amount of defence reduced for spear trap etc.
    public int level;
    public int sellCost;
    public int upgradeCost;
	public float speedUpgrade;
    public float attackUpgrade;

	// Use this for initialization
	void Start () {
        if (gameObject.name == "Magic Tower")
        {
            TowerResources tower = GameObject.Find("TowerStats").GetComponent<TowerResources>();
            attack = tower.magicAttack;
            speed = tower.magicSpeed;
            speedUpgrade = tower.magicSpeedUpgrade;

        }

        if (gameObject.name == "Arrow Tower")
        {
            TowerResources tower = GameObject.Find("TowerStats").GetComponent<TowerResources>();
            attack = tower.arrowAttack;
            speed = tower.arrowSpeed;
            speedUpgrade = tower.arrowSpeedUpgrade;

        }

        if (gameObject.name == "Fire Trap")
        {
            TowerResources tower = GameObject.Find("TowerStats").GetComponent<TowerResources>();
            attack = tower.fireAttack;
            speed = tower.fireSpeed;
            speedUpgrade = tower.fireSpeedUpgrade;

        }

        if (gameObject.name == "Poison Trap")
        {
            TowerResources tower = GameObject.Find("TowerStats").GetComponent<TowerResources>();
            attack = tower.poisonAttack;
            speed = tower.poisonSpeed;
            speedUpgrade = tower.poisonSpeedUpgrade;

        }

        if (gameObject.name == "Ice Trap")
        {
            TowerResources tower = GameObject.Find("TowerStats").GetComponent<TowerResources>();
            attack = tower.iceAttack;
            speed = tower.iceSpeed;
            specialDamage = tower.iceSpecialDamage;
            speedUpgrade = tower.iceSpeedUpgrade;

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
