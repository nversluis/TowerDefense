using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

// Class that controls the player movements
public class PlayerController : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;

    // initializing some global variables
    private GameObject camera;
    private GameObject Bullet;

    private float playerSpeed = 15f;
    private float BulletSpeed = 100f;
    private float camAngleX;
    private float camAngleY;
    public float distortion;
    private float turnSpeed = 0.5f;
    private float jumpSpeed = 8.78f;
    private float moveY=0;
    public static bool moving;
    private AudioClip magic;
    private bool jumped;
    public static Vector3 location;
    Vector3 startPosition;

    private LayerMask ignoreMaskBullet = ~((1 << 11) | (1 << 13));
    private LayerMask ignoreMaskTraps = ~(1 << 13);

    // Method for getting player input
    private Vector3 playerInput()
    {

        // determining the camera angle around origin y and the inputs of the user
        camAngleY = camera.transform.rotation.eulerAngles.y;
        float inHorz = Input.GetAxisRaw("Horizontal");
        float inVert = Input.GetAxisRaw("Vertical");

        // Movements of player
        float moveX = Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inVert;
        float moveZ = -Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inVert;

        // Jumping movements of player

        // creating a movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * playerSpeed + new Vector3(0f,rigidbody.velocity.y,0f);

        //return the movement of the player according to camera rotation and input
        return movement;

    }

    // Method for moving the player
    private void playerMovement()
    {
        // setting rotation of player in the direction of the speed of the player
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            moving = true;
        }
        else
            moving = false;

        // moving the player according to input
        rigidbody.velocity = playerInput();
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && rigidbody.velocity.y < 1)
        {
            rigidbody.velocity = rigidbody.velocity + new Vector3(0f, jumpSpeed, 0f);
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


    // Method that runs when left button is pressed
    private void OnLeftMouseDown()
    {
        // if player presses Mouse0
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (WeaponController.weapon == 1)
            {
                // determining Angles of the camera with origin
                camAngleX = camera.transform.rotation.eulerAngles.x;
                camAngleY = camera.transform.rotation.eulerAngles.y;

                // initializing correctionAngle and hit
                Vector3 camShootDistance;
                RaycastHit hit;

                Vector3 tijdelijk = new Vector3(0, 1, 0);
			
                // creating a bullet in front of 1 unit away from Player
                GameObject bullet = (GameObject)Instantiate(Bullet, transform.position + new Vector3((Mathf.Sin(camAngleY * Mathf.Deg2Rad)), 0f, Mathf.Cos(camAngleY * Mathf.Deg2Rad)) + tijdelijk, Quaternion.identity);

                // Casting a ray and storing information to hit
                if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, ignoreMaskBullet))
                {
                    camShootDistance = transform.forward;
                    Debug.DrawRay(transform.position + tijdelijk, camera.transform.forward*15);

                }
                else
                {

                    camShootDistance = hit.point - (transform.position + tijdelijk + new Vector3(Mathf.Sin(camAngleY * Mathf.Deg2Rad), 0f, Mathf.Cos(camAngleY * Mathf.Deg2Rad)));
                    camShootDistance = camShootDistance + ((new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)).normalized * distortion) * camShootDistance.magnitude) / 80f; ;
                }

                // add the force to the bullet
                bullet.rigidbody.velocity = camShootDistance.normalized * BulletSpeed;
                AddBulletDistortion();
               
                // looking in the direction of the camera
                audio.PlayOneShot(magic,15f);
            }

            if (WeaponController.weapon == 2)
            {

                if (CameraController.hitObject.name == "Cube1")
                {
                    GameObject tower = (GameObject)Instantiate(WeaponController.curTower, CameraController.hit.point + new Vector3(0f, 7.413f - 0.5f, 0f), Quaternion.Euler(0f,90f,0f));
                    tower.gameObject.transform.localScale = new Vector3(1f, 14f, 1f);
                }

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
            distortion = distortion / 1.1f;
        }

    }

    // Use this for initialization
    void Start()
    {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
        // Do not display cursor
        Screen.showCursor = false;
        camera = GameObject.Find("Main Camera");
        startPosition = transform.position;
		Bullet = resourceManager.magicBullet;
		magic = resourceManager.magicBulletSound;

        

    }

    // Update void which updates every frame
    void Update()
    {
        // Run this method
        OnLeftMouseDown();
        location = transform.position;
        Jumping();

        //GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        //Debug.Log(enemys.Length);
        //Debug.Log(isGrounded());
    }

    // Updates 60 times per second and not per frame
    void FixedUpdate()
    {
        // Move player with this method
        playerMovement();

        // Decrease bullet distortion
        DecreaseBulletDistortion();

        transform.rotation = Quaternion.Euler(0f, camera.transform.rotation.eulerAngles.y, 0f);


    }
}