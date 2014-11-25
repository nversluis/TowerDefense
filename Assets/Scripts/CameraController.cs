using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // initializing mouse position floats
    private float x;
    private float y;

    // initializing camera properties
    public float camDis = 6;
    private float camOffset = 2;
    private float distanceOffset;
    private float scrollSpeed = 4;
    private float maxCamHeight = 10;
    private float minCamHeight = 0;
    private float mouseSpeed = 2;

    // initializing Player gameobject
    public GameObject Player;

    // initializing raycasthit
    RaycastHit hit;

    // creating properties for determining which object is pointed to
    public static GameObject hitObject;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Method for determining mouse input to calculate the camera position
    private void CamMov()
    {
        // change camera height according to input from mouse scrollwheel and setting limits
        camDis = Mathf.Clamp(camDis-Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed, minCamHeight, maxCamHeight);

        // determining input from mouse axis
        float mousex = Input.GetAxisRaw("Mouse X") * mouseSpeed;
        float mousey = Input.GetAxisRaw("Mouse Y") * mouseSpeed;

        // updating x and y with mouse movement. Limiting the y movement.
        x = x + mousex;
        y = Mathf.Clamp(y - mousey,-90,90);

        // calculating the orbit around the player and setting rotation of camera
        Quaternion camRot = Quaternion.Euler(y, x, 0f);
        transform.rotation = camRot;

        // calculating position of camera and setting position of camera
        Vector3 probPosition = camRot * new Vector3(0f, 0f, -camDis) + Player.transform.position + new Vector3(0f,camOffset,0f);
        // Checking for collision from camera with objects with method
        CamCollide(probPosition);

        // calculating position of camera and setting position of camera
        Vector3 position = camRot * new Vector3(0f, 0f, -camDis + distanceOffset) + Player.transform.position + new Vector3(0f, camOffset, 0f);

        transform.position = position;
    }

    // method for determining if the camera got through an object
    private void CamCollide(Vector3 Position)
    {
        // create a vector from the player to the camera
        Vector3 relativePos = Position  - (Player.transform.position);

        RaycastHit[] hits;

        Debug.DrawRay(Player.transform.position , relativePos);


        // casting a ray from player to camera and checking if it hit something
        if (Physics.Raycast(Player.transform.position , relativePos, out hit, camDis + 0.5f))
        {
            hits = Physics.RaycastAll(Player.transform.position, relativePos, camDis + 0.5f);
            // setting an offset of the camera so it doesnt go through a wall
            distanceOffset = camDis - hit.distance + 0.8f;
            distanceOffset = Mathf.Clamp(distanceOffset, 0f, camDis);
        }
         // else a normal distance offset
        else
        {
            distanceOffset = 0f;
        }

    }

    void LateUpdate()
    {
        // Moving camera with method
        CamMov();

    }

	// Update is called once per frame
	void FixedUpdate () {



        // casting a ray to see what object is in front of the camera
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            hitObject = hit.collider.gameObject;
        }

		//Set cursor to center of screen
		Screen.lockCursor = true;

	}

	// Return the object which the camera
	public static GameObject getHitObject(){
		return hitObject;
	}
}
