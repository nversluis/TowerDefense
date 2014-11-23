using UnityEngine;
using System.Collections;

// Class that controls the player movements
public class PlayerController : MonoBehaviour
{
    // initializing some global variables
    public GameObject camera;
    public GameObject Bullet;

    private float playerSpeed = 0.4f;
    private float BulletSpeed = 3000f;
    private float closestCorrection = 3f;
    private float camAngleX;
    private float camAngleY;
    private float distortion;
    private float turnSpeed = 0.5f;
    private float jumpSpeed = 8;
    private bool jumping;


    // Method for getting player input
    private Vector3 playerInput()
    {
        // initializing the player controller
        CharacterController charController = GetComponent<CharacterController>();

        // determining the camera angle around origin y and the inputs of the user
        camAngleY = camera.transform.rotation.eulerAngles.y;
        float inHorz = Input.GetAxisRaw("Horizontal");
        float inVert = Input.GetAxisRaw("Vertical");

        // Movements of player
        float moveX = Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inVert;
        float moveY = 0f;
        float moveZ = -Mathf.Sin(camAngleY * Mathf.Deg2Rad) * inHorz + Mathf.Cos(camAngleY * Mathf.Deg2Rad) * inVert;

        // creating a movement vector
        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        //return the movement of the player according to camera rotation and input
        return movement;

    }
    
    // Method for moving the player
    private void playerMovement()
    {

        // initializing the player controller
        CharacterController charController = GetComponent<CharacterController>();

        if (charController.velocity != Vector3.zero)
        {
            // setting rotation of player in the direction of the speed of the player
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0f,Quaternion.LookRotation(playerInput()*playerSpeed).eulerAngles.y,0f), turnSpeed);
        }

        // moving the player according to input
        charController.Move(playerInput()*playerSpeed);


    }

    // Method that runs when left button is pressed
    private void OnLeftMouseDown()
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
            yMag = -Mathf.Tan(camAngleX*Mathf.Deg2Rad)*(BulletController.maxBulletDistance+10);
        }

        else
        {
            Vector3 camShootDistance = hit.point - (transform.position + new Vector3((Mathf.Sin(camAngleY * Mathf.Deg2Rad)), 0f, Mathf.Cos(camAngleY * Mathf.Deg2Rad)));
            xzMag = new Vector2(camShootDistance.x, camShootDistance.z).magnitude;
            yMag = camShootDistance.y;
        }

        // correct angle of bullet to where crosshair is
        yAngle = Mathf.Rad2Deg * Mathf.Atan(yMag / xzMag);

        float bulletForceX = Mathf.Sin(camAngleY * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion) * BulletSpeed;
        float bulletForceY = Mathf.Tan(((yAngle) * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion)) * BulletSpeed;
        float bulletForceZ = Mathf.Cos(camAngleY * Mathf.Deg2Rad + Mathf.Deg2Rad * 2f * (Random.value - 0.5f) * distortion) * BulletSpeed;

        // add the force to the bullet
        bullet.rigidbody.AddForce(new Vector3(bulletForceX, bulletForceY, bulletForceZ));
        BulletDistortion();

        // looking in the direction of the camera
        transform.rotation = Quaternion.Euler(0f, camera.transform.rotation.eulerAngles.y,0f);
    }
    
    // Method that distorts the direction of the bullet
    private void BulletDistortion()
    {
        distortion = distortion + 4f;

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
        // if player presses Mouse0
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {   
            // Run this method
            OnLeftMouseDown();
        }

        jumping = Input.GetButtonDown("Jump");  
    }
    // Updates 60 times per second and not per frame
    void FixedUpdate()
    {

        // Move player with this method
        playerMovement();

        if (distortion > 0)
        {
            distortion = distortion / 1.1f;
        }
        else
        {
            distortion = 0;
        }

    }

}
