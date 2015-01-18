using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreServer : MonoBehaviour
{

    ResourceManager resourceManager;
    GameObject ResourceManagerObj;
    static public string naam;

    static List<string> score;
    static List<List<string>> hiscores;

    static string scores;
    static string url;

    bool getting = true;

    float counter;

    WWW www;


    // Use this for initialization
    void Start()
    {
        
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
        naam = resourceManager.name;
        counter = 0;

        hiscores = new List<List<string>>();

        string url = "http://drproject.twi.tudelft.nl:8087/getScore";
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    // Update is called once per frame
    void Update()
    {
        // haal scores van server
        if (getting)
        {
            getScoreFromServer();
            if (hiscores.Count > 0)
            {
                getting = false;
            }
            else
            {
                counter += Time.deltaTime;
                if (counter > 10)
                {
                    Debug.Log("could not retrieve hiscores");
                    getting = false;
                }
            }
        }
    }

    static public void sendScoreToServer(string naam)
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setStatistics?naam=" + naam +  "&difficulty=" + ResourceManager.Difficulty + "&score=" + Statistics.score + "&waves=" + Statistics.currentWave
            + "&kills=" + Statistics.kills + "&killstreak=" + Statistics.killStreak + "&headshots=" + Statistics.headshots + "&headshotstreak=" + Statistics.headShotStreak 
            + "&firetrapsbuilt=" + Statistics.fireTrapsBuilt + "&icetrapsbuilt=" + Statistics.iceTrapsBuilt + "&poisontrapsbuilt=" + Statistics.poisonTrapsBuilt + "&magictowersbuilt=" + Statistics.magicTowersBuilt
            + "&arrowtowersbuilt=" + Statistics.arrowTowersBuilt + "&barricadebuilt=" + Statistics.barricadesBuilt);

        WaitForRequest(www);
    }

    public void getScoreFromServer()
    {
        WWW www = new WWW(url);
        WaitForRequest(www);
    }

    static IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null || www.error == "")
        {
            scores = www.text.ToString();
            splitScore();
        }
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

    public List<List<string>> getHiscores(int totRank)
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < totRank; i++)
            {
                hiscoresTotRank.Add(hiscores[i]);
            }
        }
        return hiscoresTotRank;
    }

    public List<List<string>> getHiscores()
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                hiscoresTotRank.Add(hiscores[i]);
            }
        }
        return hiscoresTotRank;
    }

    public void printMatrix(List<List<string>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                Debug.Log(matrix[i][j]);
            }
        }

    }
}
