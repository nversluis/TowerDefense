using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

// Class that controls the player movements
public class PlayerController : MonoBehaviour
{
    // initializing some global variables
    public GameObject camera;
    public GameObject Bullet;

    private float playerSpeed = 0.4f;
    private float BulletSpeed = 30f;
    private float camAngleX;
    private float camAngleY;
    private float distortion;
    private float turnSpeed = 0.5f;
    private float jumpSpeed = 1.5f;
    private float gravity = 2f;
    private float moveY;
    private CharacterController charController;
    public static bool moving;
    public AudioClip magic;
    public static Vector3 location;


    // Method for getting player input
    private Vector3 playerInput()
    {
        // initializing the player controller
        charController = GetComponent<CharacterController>();

        // determining the camera angle around origin y and the inputs of the user
        camAngleY = camera.transform.rotation.eulerAngles.y;
        float inHorz = Input.GetAxisRaw("Horizontal");
        float inVert = Input.GetAxisRaw("Vertical");

        // Movements of player
        float moveX = Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inVert;
        moveY = Ymovement(moveY);
        float moveZ = -Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inVert;

        // Jumping movements of player

        // creating a movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        movement = movement + new Vector3(0, moveY, 0);


        //return the movement of the player according to camera rotation and input
        return movement;

    }

    // Method for moving the player
    private void playerMovement()
    {
        // initializing the player controller
        CharacterController charController = GetComponent<CharacterController>();
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // setting rotation of player in the direction of the speed of the player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, Quaternion.LookRotation(playerInput() * playerSpeed).eulerAngles.y, 0f), turnSpeed);
            moving = true;
        }
        else
            moving = false;

        // moving the player according to input
        charController.Move(playerInput() * playerSpeed);
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
                float yAngle;
                float xzMag;
                float yMag;
                RaycastHit hit;

                // creating a bullet in front of 1 unit away from Player
                GameObject bullet = (GameObject)Instantiate(Bullet, transform.position + new Vector3((Mathf.Sin(camAngleY * Mathf.Deg2Rad)), 0f, Mathf.Cos(camAngleY * Mathf.Deg2Rad)), Quaternion.identity);

                // Casting a ray and storing information to hit
                if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
                {
                    xzMag = BulletController.maxBulletDistance;
                    yMag = -Mathf.Tan(camAngleX * Mathf.Deg2Rad) * (BulletController.maxBulletDistance + 10);
                }
                else
                {
                    Vector3 camShootDistance = hit.point - (transform.position + new Vector3((Mathf.Sin(camAngleY * Mathf.Deg2Rad)), 0f, Mathf.Cos(camAngleY * Mathf.Deg2Rad)));
                    xzMag = new Vector2(camShootDistance.x, camShootDistance.z).magnitude;
                    yMag = camShootDistance.y;
                }

                // correct angle of bullet to where crosshair is
                yAngle = Mathf.Rad2Deg * Mathf.Atan(yMag / xzMag);

                float bulletForceX = Mathf.Sin(camAngleY * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion);
                float bulletForceY = Mathf.Tan(((yAngle) * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion));
                float bulletForceZ = Mathf.Cos(camAngleY * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion);

                Vector3 bulletForce = new Vector3(bulletForceX, bulletForceY, bulletForceZ).normalized;

                // add the force to the bullet
                bullet.rigidbody.velocity = bulletForce * BulletSpeed;
                AddBulletDistortion();

                // looking in the direction of the camera
                transform.rotation = Quaternion.Euler(0f, camera.transform.rotation.eulerAngles.y, 0f);
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

    private float Ymovement(float moveY)
    {

        if (charController.isGrounded && moveY<=0)
        {
            moveY = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveY = moveY + jumpSpeed;

            }
        }

        moveY = moveY - gravity*Time.fixedDeltaTime;

        return moveY;

    }

    // Use this for initialization
    void Start()
    {
        // Do not display cursor
        Screen.showCursor = false;
        camera = GameObject.Find("Main Camera");

    }

    // Update void which updates every frame
    void Update()
    {
        // Run this method
        OnLeftMouseDown();
        location = transform.position;
        
        //GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(enemys.Length);

    }

    // Updates 60 times per second and not per frame
    void FixedUpdate()
    {
        // Move player with this method
        playerMovement();

        // Decrease bullet distortion
        DecreaseBulletDistortion();

    }
}
    