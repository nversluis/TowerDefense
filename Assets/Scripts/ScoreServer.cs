using UnityEngine;
using System.Collections;

public class ScoreServer : MonoBehaviour {

    ResourceManager resourceManager;
    GameObject ResourceManagerObj;
    static public string naam;


	// Use this for initialization
	void Start () {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        naam = resourceManager.name;
	}
	
	// Update is called once per frame
	void Update () {

	}

    static public void sendScoreToServer(string naam)
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setScore?naam=" + naam + "&score=" + Statistics.score);

        WaitForRequest(www);
    }

    /*public void getScoreFromServer()
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/getScore");

        WaitForRequest(www);



        float counter = 0;
        while (counter < 1)
        {
            counter += Time.deltaTime;
        }


        Debug.Log("data:" + www.text);

    }*/


    static IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
    }  
}
