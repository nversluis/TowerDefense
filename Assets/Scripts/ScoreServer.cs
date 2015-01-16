using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreServer : MonoBehaviour {

    ResourceManager resourceManager;
    GameObject ResourceManagerObj;
    static public string naam;

    static List<string> score;
    static List<List<string>> hiscores;

    static string scores;

	// Use this for initialization
	void Start () {
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        naam = resourceManager.name;

        hiscores = new List<List<string>>();

        string url = "http://drproject.twi.tudelft.nl:8087/getScore";
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
	
	// Update is called once per frame
	void Update () {
        getScoreFromServer();
        //splitScore();
	}

    static public void sendScoreToServer(string naam)
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setScore?naam=" + naam + "&score=" + Statistics.score);

        WaitForRequest(www);
    }

    public void getScoreFromServer()
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/getScore");
        WaitForRequest(www);
    }

    static IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        scores = www.text.ToString();
        //Debug.Log(scores);
        splitScore();
    }

    public static void splitScore()
    {
        scores = scores.Replace("Naam: ", "");
        scores = scores.Replace(", Score: ", ",");
        scores = scores.Replace("<br/>", ",");

        string[] split = scores.Split(',');

        for (int i = 0; i < split.Length - 1; i = i + 2)
        {
            score = new List<string>();
            score.Add(split[i]);
            score.Add(split[i + 1]);
            hiscores.Add(score);
        }
    }

}
