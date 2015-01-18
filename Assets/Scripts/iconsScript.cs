using UnityEngine;
using System.Collections;

public class iconsScript : MonoBehaviour {

    GameObject player;
	
	// Update is called once per frame
	void Update () {
        //change the layer of the minimapplane of this floor if the player is too far, so it wont show on the minimap
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        else if ((player.transform.position - transform.position).magnitude >= 70)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = 9;
        }
	}
}
