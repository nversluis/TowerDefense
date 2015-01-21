using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

// Class that controls the player movements
public class PlayerController : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
    GameObject hitParticles;
    GameObject hitExplosionParticles;
    GameObject explosionBullet;

    AudioSource cameraAudio;
    AudioClip sword;
    AudioClip sword2;
    AudioClip sword3;

    AudioClip hitEnemy;
    AudioClip hitEnemy2;
    AudioClip hitEnemy3;

    AudioClip swordSpecial;

    float volume;

    // initializing some global variables
    private GameObject cameraMain;
    private GameObject Bullet;

    private float playerSpeed;
    private float BulletSpeed = 100f;
    private float camAngleY;
    public float distortion;
    private float jumpSpeed = 8.78f;


    public static bool moving;
    public static bool attackingSword1;
    public static bool attackingSword2;
    public static bool attackingSword3;
    public static bool attackMagic1;
    public static bool attackMagic2;
    public static bool idle;
	public static bool jumping;
	public static bool isDancing;

	bool inAir;
	public static bool waveEnded;


    private AudioClip magic;
    AudioClip walking;
    private bool jumped;
    public static Vector3 location;
	GameObject curFloor;


    private LayerMask ignoreMaskBullet = ~((1 << 11) | (1 << 13));
    private LayerMask ignoreMaskTraps = ~(1 << 13);
    private LayerMask enemyMask = (1 << 12);

    public bool coolDownSword1;
    public bool coolDownSword2;
    public bool coolDownMagic1;
    public bool coolDownMagic2;

    float coolDownSword1Time;
    float coolDownSword2Time;
    float coolDownMagic1Time;
    float coolDownMagic2Time;

    Vector3 tijdelijk = new Vector3(0, 1, 0);

    int swordDamage;
    public int magicDamage;
    int specialSwordDamage;
    public int specialMagicDamage;

    private PlayerData player = GUIScript.player;

    bool invoked;
    public static bool started;
	GameObject bulletToInst;
    // Method for getting player input
    private Vector3 playerInput()
    {


        // determining the camera angle around origin y and the inputs of the user
        camAngleY = cameraMain.transform.rotation.eulerAngles.y;
        float inHorz = Input.GetAxisRaw("Horizontal");
        float inVert = Input.GetAxisRaw("Vertical");

        // Movements of player
        float moveX = Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inVert;
        float moveZ = -Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inVert;

        // creating a movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * playerSpeed + new Vector3(0f,rigidbody.velocity.y,0f);

        //return the movement of the player according to camera rotation and input
        return movement;

    }

    public void addMagicDamage(int addDamage)
    {
        float ratio = specialMagicDamage / magicDamage;
        magicDamage += addDamage;
        specialMagicDamage +=(int)( ratio * addDamage);
    }
    public void addSwordDamage(int addDamage)
    {
        float ratio = specialSwordDamage / swordDamage;

        swordDamage += addDamage;
        specialSwordDamage += (int)(ratio * addDamage);
    }

    public int getAtkStat()
    {
        return swordDamage;
    }

    public int getMagStat()
    {
        return magicDamage;
    }

    public float getAgiStat()
    {
        return playerSpeed;
    }

    public void addPlayerSpeed(int addSpeed)
    {
        playerSpeed += addSpeed;
    }

    void setCoolDownSword1false()
    {
        coolDownSword1 = false;
    }
    void setCoolDownSword2false()
    {
        coolDownSword2 = false;
    }

    void setCoolDownMagic1false()
    {
        coolDownMagic1 = false;
    }

    void setCoolDownMagic2false()
    {
        coolDownMagic2 = false;
    }

    // Method for moving the player
    private void playerMovement()
    {
        // Checking if the player is moving
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !jumping && !isDancing)
        {
            idle = false;
            moving = true;

        }
        else if (!OtherAnimationTrue() && !jumping && !isDancing)
        {

            idle = true;
            moving = false;
        }
        else
        {
            idle = false;
            moving = false;
        }

        // moving the player according to input
        rigidbody.velocity = playerInput();
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && rigidbody.velocity.y < 1)
        {
            rigidbody.velocity = rigidbody.velocity + new Vector3(0f, jumpSpeed, 0f);
			jumping = true;
			idle = false;
			moving = false;
			SetAttackAnimationFalse ();
			Invoke ("SetJumpAnimationFalse", .5f);
        }

		if (inAir && isGrounded () && !isDancing) {
			inAir = false;
			jumping = false;
			moving = false;
			idle = true;
		}

    }


    bool isGrounded()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        float colliderCenterLoc = collider.center.y * transform.localScale.y;
        float colliderOriginDistance = (collider.center.y * transform.localScale.y-collider.height/2*transform.localScale.y);
        float distance = colliderCenterLoc - colliderOriginDistance;
        Ray ray = new Ray(new Vector3(collider.center.x * transform.localScale.x, collider.center.y * transform.localScale.y, collider.center.z * transform.localScale.z) + transform.position, new Vector3(0,-1,0));
        return Physics.Raycast(ray, distance+0.5f,ignoreMaskTraps);
    }

    IEnumerator DestroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(particles);
    }

    IEnumerator DestroyParticles2(GameObject particles)
    {
        yield return new WaitForSeconds(3f);
        Destroy(particles);
    }

    IEnumerator HitEnemy(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.2f);
        hit.transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(swordDamage, "physical", true);
        GameObject particles = (GameObject)Instantiate(hitParticles, hit.transform.position, Quaternion.identity);
        particles.transform.localScale = new Vector3(3, 3, 3);
        StartCoroutine(DestroyParticles(particles));
        int random = Random.Range(0, 3);
        
        if (random == 0)
        {
            
            cameraAudio.PlayOneShot(hitEnemy, volume);
        }
        else if (random == 1)
        {
            cameraAudio.PlayOneShot(hitEnemy2, volume);

        }
        else if (random == 2)
        {
            cameraAudio.PlayOneShot(hitEnemy3, volume);

        }
    }

    IEnumerator SpecialHitEnemy(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.2f);

        LayerMask enemys = (1 << 12);
        Collider[] hitCollider = Physics.OverlapSphere(hit.transform.position, 10f, enemys);
        
        foreach (Collider collide in hitCollider)
        {
            EnemyHealth enemyHealth = collide.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                collide.transform.gameObject.GetComponent<EnemyHealth>().TakeDamage((int)(specialSwordDamage / (Vector3.Distance(transform.position, collide.transform.position))), "physical", true);
                Debug.Log(hit.transform.name + " " + (int)(specialSwordDamage / (Vector3.Distance(transform.position, collide.transform.position))));
            }
        }

        GameObject particles = (GameObject)Instantiate(hitExplosionParticles, hit.transform.position, Quaternion.identity);
        StartCoroutine(DestroyParticles2(particles));
        cameraAudio.PlayOneShot(swordSpecial, volume);
        
    }

    void Walking()
    {
        cameraAudio.PlayOneShot(walking, volume);
    }

    // Method that runs when left button is pressed
    private void OnLeftMouseDown()
    {
        
        // if player presses Mouse0
		if (Input.GetKeyDown(KeyCode.Mouse0) && !OtherAnimationTrue())
        {

			if (WeaponController.weapon == 1 && !coolDownSword1 && isGrounded())
            {
                SetAttackAnimationFalse();

                int random = 0;
                if (random == 0)
                {
                    attackingSword1 = true;
                }

                else
                {
                    attackingSword2 = true;
                }

                random = Random.Range(0, 3);
                if (random == 0)
                {

                    cameraAudio.PlayOneShot(sword, volume);
                }
                else if (random == 1)
                {
                    cameraAudio.PlayOneShot(sword2, volume);

                }
                else if (random == 2)
                {
                    cameraAudio.PlayOneShot(sword3, volume);

                }
                coolDownSword1 = true;
                player.getSkills()[0].startCooldown();
				if (moving) {
					Invoke ("SetAttackAnimationFalse", 1f);
				} else {
					Invoke ("SetAttackAnimationFalse", 2f/3f);
				}
                Invoke("setCoolDownSword1false", coolDownSword1Time);

                RaycastHit hit;
                if(Physics.Raycast(transform.position + tijdelijk, transform.forward, out hit, 4f, enemyMask)){
                    StartCoroutine(HitEnemy(hit));
                
                }
            }

			if (WeaponController.weapon == 2 && !coolDownSword2 && isGrounded())
            {
                SetAttackAnimationFalse();
                idle = false;
                attackingSword3 = true;
                coolDownSword2 = true;
                player.getSkills()[1].startCooldown();
                Invoke("SetAttackAnimationFalse", 1f);
                Invoke("setCoolDownSword2false", coolDownSword2Time);

                RaycastHit hit;
                if (Physics.Raycast(transform.position + tijdelijk, transform.forward, out hit, 3f, enemyMask))
                {
                    StartCoroutine(SpecialHitEnemy(hit));

                }

            }

            if (WeaponController.weapon == 3 && !coolDownMagic1)
            {
                SetAttackAnimationFalse();
                coolDownMagic1 = true;
                player.getSkills()[2].startCooldown();
                attackMagic1 = true;
                Invoke("SetAttackAnimationFalse", .1f);
                Invoke("setCoolDownMagic1false", coolDownMagic1Time);

                // determining Angles of the camera with origin
				bulletToInst = Bullet;

			
                // creating a bullet in front of 1 unit away from Player
				Invoke ("InstBullet", 0.2f);
            }

            if (WeaponController.weapon == 4 && !coolDownMagic2)
            {
                SetAttackAnimationFalse();
                coolDownMagic2 = true;
                player.getSkills()[3].startCooldown();
                attackMagic2 = true;
                Invoke("SetAttackAnimationFalse", .1f);
                Invoke("setCoolDownMagic2false", coolDownMagic2Time);


                // initializing correctionAngle and hit

			
                // creating a bullet in front of 1 unit away from Player
				bulletToInst = explosionBullet;
				Invoke ("InstBullet", 0.2f);
            }

        }

    }

    // Method that distorts the direction of the bullet
    private void AddBulletDistortion()
    {
        if (distortion <= 10)
        {
            distortion = distortion + 4f;
        }

    }

    // Decrease bullet distortion over time
    private void DecreaseBulletDistortion()
    {
        if (distortion > 0 )
        {
            distortion = distortion / 1.05f;
        }

    }

    void SetAttackAnimationFalse()
    {
    attackingSword1 = false;
    attackingSword2 = false;
    attackingSword3 = false;

    attackMagic1 = false;
    attackMagic2 = false;
    }

	void SetJumpAnimationFalse()
	{
		inAir = true;
	}

    bool OtherAnimationTrue()
    {
        return attackingSword1 || attackingSword2 || attackMagic1 || attackMagic2 || attackingSword3;
    } 

	void InstBullet()
	{
		// initializing correctionAngle and hit
		Vector3 camShootDistance;
		RaycastHit hit;
		Vector3 bulletPos = gameObject.transform.Find ("BulletPosition").transform.position;
		GameObject bullet = (GameObject)Instantiate(bulletToInst, bulletPos, Quaternion.identity);
		AudioSource bulletAudio = bullet.GetComponent<AudioSource>();

		// Casting a ray and storing information to hit
		if (!Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out hit, Mathf.Infinity, ignoreMaskBullet))
		{
			camShootDistance = transform.forward;

		}
		else
		{
			camShootDistance = hit.point - (bulletPos);
		}

		// add the force to the bullet
		bullet.rigidbody.velocity = camShootDistance.normalized * BulletSpeed;
		AddBulletDistortion();

		// looking in the direction of the camera
		bulletAudio.PlayOneShot(magic, 10f * volume);
	}
    // Use this for initialization
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
        // Do not display cursor
        Screen.showCursor = false;
        cameraMain = GameObject.Find("Main Camera");
        cameraAudio = cameraMain.GetComponent<AudioSource>();
		Bullet = resourceManager.magicBullet;
		magic = resourceManager.magicBulletSound;
        coolDownMagic1Time = resourceManager.coolDownMagic1Time[ResourceManager.Difficulty];
        player.getSkills()[2].setCdTime(coolDownMagic1Time);
        coolDownMagic2Time = resourceManager.coolDownMagic2Time[ResourceManager.Difficulty];
        player.getSkills()[3].setCdTime(coolDownMagic2Time);
        coolDownSword1Time = resourceManager.coolDownSword1Time[ResourceManager.Difficulty];
        player.getSkills()[0].setCdTime(coolDownSword1Time);
        coolDownSword2Time = resourceManager.coolDownSword2Time[ResourceManager.Difficulty];
        player.getSkills()[1].setCdTime(coolDownSword2Time);
        swordDamage = resourceManager.startSwordDamage[ResourceManager.Difficulty];
        magicDamage = resourceManager.startMagicDamage[ResourceManager.Difficulty];
        specialSwordDamage = resourceManager.specialSwordDamage[ResourceManager.Difficulty];
        specialMagicDamage = resourceManager.specialMagicDamage[ResourceManager.Difficulty];
        playerSpeed = resourceManager.speed[ResourceManager.Difficulty];
        hitParticles = resourceManager.hitParticles;
        sword = resourceManager.sword;
        sword2 = resourceManager.sword2;
        sword3 = resourceManager.sword3;
        volume = (float)PlayerPrefs.GetInt("SFX")/100f;
        walking = resourceManager.walking;
        started = false;

        hitEnemy = resourceManager.hitEnemy;
        hitEnemy2 = resourceManager.hitEnemy2;
        hitEnemy3 = resourceManager.hitEnemy3;

        swordSpecial = resourceManager.swordSpecial;

        hitExplosionParticles = resourceManager.hitExplosionParticles;

        explosionBullet = resourceManager.ExplosionMagic;
        
    }

    // Update void which updates every frame
    void Update()
	{
        if (started)
        {
            // Run this method
            OnLeftMouseDown();
            location = transform.position;
            Jumping();

            if (moving && !invoked)
            {
                InvokeRepeating("Walking", 0f, 1.097f);
                invoked = true;
            }
            if (!moving)
            {
                invoked = false;
                CancelInvoke("Walking");
            }

            if (Input.GetKeyDown(KeyCode.Period))
            {
                if (Time.timeScale < 1.6)
                {

                    Time.timeScale += 0.2f;
                }
                else
                {
                    Time.timeScale = 2;
                }
            }

            if (Input.GetKeyDown(KeyCode.Comma))
            {
                if (Time.timeScale > 1.2 && Time.timeScale <1.8)
                {
                    Time.timeScale -= 0.2f;
                }
                else if (Time.timeScale <1.2)
                {
                    Time.timeScale = 1;
                }
                else if (Time.timeScale > 1.8)
                {
                    Time.timeScale = 1.6f;
                }
            }
        }

		if (waveEnded) {
			waveEnded = false;
			isDancing = true;
			idle = false;
			moving = false;
			jumping = false;
			Invoke ("StopDancing", 3.4f);
		}
    }

	void StopDancing(){
		isDancing = false;
	}

	void checkFloor(){
		RaycastHit hit;
		GameObject res = curFloor;
		//Ray ray = new Ray(transform.position, -Vector3.up, out hit);
        if (Physics.Raycast(transform.position + tijdelijk, -Vector3.up, out hit))
        {
			if (hit.transform.name.Contains ("loor")) {
				curFloor = hit.transform.gameObject;
				if (res != null && res != curFloor) {
					res.GetComponent<FloorScript> ().hasEnemy = false;
					if(curFloor.transform.childCount==2)
					WallScript.DestroyHotSpots ();
				}
				FloorScript floor = hit.transform.GetComponent<FloorScript> ();
				floor.hasEnemy = true;
			}
		}
	}

    // Updates 60 times per second and not per frame
    void FixedUpdate()
    {
		// Debug.Log (idle);
		checkFloor ();
        // Move player with this method
        playerMovement();

        // Decrease bullet distortion
        DecreaseBulletDistortion();

        transform.rotation = Quaternion.Euler(0f, cameraMain.transform.rotation.eulerAngles.y, 0f);


    }


}