using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreServer : MonoBehaviour
{

    static List<string> score;
    static List<string> stats;
    static List<List<string>> hiscores; // bevat ook de difficulty
    static List<List<string>> statistics;

    static string scores;
    static string statistieken;
    static string urlScores;
    static string urlStatistics;

    public static bool getting = true;
    public static bool connected;

    bool activated;

    float counter;
    int waitTime = 10;

    static WWW wwwScores;
    WWW wwwStatistics;

    int test;

    // Use this for initialization
    void Start()
    {
        hiscores = new List<List<string>>();
        statistics = new List<List<string>>();

        counter = 0;

        urlScores = "http://drproject.twi.tudelft.nl:8087/getScore";
        wwwScores = new WWW(urlScores);

        urlStatistics = "http://drproject.twi.tudelft.nl:8087/getStatistics";
        wwwStatistics = new WWW(urlStatistics);

        StartCoroutine(WaitForRequest(wwwScores));
        StartCoroutine(WaitForRequestStatistics(wwwStatistics));

    }

    // Update is called once per frame
    void Update()
    {
        // haal scores van server
        if (getting)
        {
            getScoreFromServer();
            getStatisticsFromServer();
            if (hiscores.Count > 0 && statistics.Count > 0)
            {
                connected = true;
                getting = false;
            }
            else
            {
                counter += Time.deltaTime;
                if (counter > waitTime)
                {
                    Debug.Log("could not retrieve hiscores");
                    connected = false;
                    getting = false;
                }
            }
        }
    }

    static public void sendScoreToServer(string naam)
    {
       new WWW("http://drproject.twi.tudelft.nl:8087/setStatistics?naam=" + naam + "&difficulty=" + ResourceManager.Difficulty + "&score=" + Statistics.score + "&waves=" + Statistics.currentWave
            + "&kills=" + Statistics.kills + "&killstreak=" + Statistics.killStreak + "&headshots=" + Statistics.headshots + "&headshotstreak=" + Statistics.headShotStreak
            + "&firetrapsbuilt=" + Statistics.fireTrapsBuilt + "&icetrapsbuilt=" + Statistics.iceTrapsBuilt + "&poisontrapsbuilt=" + Statistics.poisonTrapsBuilt + "&magictowersbuilt=" + Statistics.magicTowersBuilt
            + "&arrowtowersbuilt=" + Statistics.arrowTowersBuilt + "&barricadebuilt=" + Statistics.barricadesBuilt);

        //WaitForRequest(www);
    }

    public static void getScoreFromServer()
    {
        //WWW www = new WWW(urlScores);
        WaitForRequest(wwwScores);
    }

    public void getStatisticsFromServer()
    {
        //WWW www = new WWW(urlStatistics);
        WaitForRequestStatistics(wwwStatistics);
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

    static IEnumerator WaitForRequestStatistics(WWW www)
    {
        yield return www;
        if (www.error == null || www.error == "")
        {
            statistieken = www.text.ToString();
            splitStatistics();
        }
    }

    public static void splitScore()
    {
        scores = scores.Replace("Naam: ", "");
        scores = scores.Replace(", Difficulty: ", ",");
        scores = scores.Replace(", Score: ", ",");
        scores = scores.Replace("<br/>", ",");

        string[] split = scores.Split(',');

        for (int i = 0; i < split.Length - 2; i = i + 3)
        {
            score = new List<string>();
            // Naam
            score.Add(split[i]);
            // Difficulty
            score.Add(split[i + 1]);
            // Score
            score.Add(split[i + 2]);

            hiscores.Add(score);
        }
    }

    public static void splitStatistics()
    {
        statistieken = statistieken.Replace("Naam: ", "");
        statistieken = statistieken.Replace(", Difficulty: ", ",");
        statistieken = statistieken.Replace(", Score: ", ",");
        statistieken = statistieken.Replace(", Wave: ", ",");
        statistieken = statistieken.Replace(", Kills: ", ",");
        statistieken = statistieken.Replace(", Killstreak: ", ",");
        statistieken = statistieken.Replace(", Headshots: ", ",");
        statistieken = statistieken.Replace(", Headshotstreak: ", ",");
        statistieken = statistieken.Replace(", Firetrapsbuilt: ", ",");
        statistieken = statistieken.Replace(", Icetrapsbuilt: ", ",");
        statistieken = statistieken.Replace(", Poisontrapsbuilt: ", ",");
        statistieken = statistieken.Replace(", Magictowersbuilt: ", ",");
        statistieken = statistieken.Replace(", Arrowtowersbuilt: ", ",");
        statistieken = statistieken.Replace(", Barricadebuilt: ", ",");
        statistieken = statistieken.Replace("<br/>", ",");

        string[] split = statistieken.Split(',');

        for (int i = 0; i < split.Length - 13; i = i + 14)
        {
            stats = new List<string>();

            for (int j = 0; j < 14; j++)
            {
                stats.Add(split[i + j]);
            }
            statistics.Add(stats); 
        }
    }

    public static List<List<string>> getHiscores()
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();
        List<string> score;

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                score = new List<string>();
                // voeg naam toe
                score.Add(hiscores[i][0]);
                // voeg score toe
                score.Add(hiscores[i][2]);
                hiscoresTotRank.Add(score);
            }
        }
        return hiscoresTotRank;
    }

    public static List<List<string>> getHiscores(int difficulty)
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();
        List<string> score;

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                if (hiscores[i][1] == "" + difficulty)
                {
                    score = new List<string>();
                    // voeg naam toe
                    score.Add(hiscores[i][0]);
                    // voeg score toe
                    score.Add(hiscores[i][2]);
                    hiscoresTotRank.Add(score);
                }
            }
        }
        return hiscoresTotRank;
    }

    public static int getPositionOnHiscores(int difficulty, string naam)
    {
        for (int i = 0; i < getHiscores(difficulty).Count; i++)
        {
           //Debug.Log("hiscores[" + i + "][0] = " + hiscores[i][0]);
           //Debug.Log("hiscores[" + i + "][0] = " + hiscores[i][1]);
           if (getHiscores(difficulty)[i][0] == naam)
           {
               //Debug.Log(i);
               //Debug.Log(hiscores[i][0]);
               return i + 1;
           }
        }
        return 0;
    }
        
    

    public List<List<string>> getHiscoresDifficultyTotRank(int difficulty, int totRank)
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();
        List<string> score;
        int grootte = 0;
        int aantal = 0;

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                if (hiscores[i][1] == "" + difficulty)
                {
                    grootte++;
                }
            }
        }

        if (totRank > grootte)
        {
            totRank = grootte;
        }

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                if (hiscores[i][1] == "" + difficulty)
                {
                    if (aantal < totRank)
                    {
                        score = new List<string>();
                        // voeg naam toe
                        score.Add(hiscores[i][0]);
                        // voeg score toe
                        score.Add(hiscores[i][2]);
                        hiscoresTotRank.Add(score);
                        aantal++;
                    }
                }
            }
        }
        return hiscoresTotRank;
    }

    public static List<List<string>> getStatistics()
    {
        List<List<string>> res = new List<List<string>>();

        if (statistics.Count > 0)
        {
            for (int i = 0; i < statistics.Count; i++)
            {
                res.Add(statistics[i]);
            }
        }
        return res;
    }

    public List<List<string>> getStatisticsNaam(string naam)
    {
        List<List<string>> res = new List<List<string>>();
        List<string> stats;

        if (statistics.Count > 0)
        {
            for (int i = 0; i < statistics.Count; i++)
            {
                if (statistics[i][0] == naam)
                {
                    stats = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        stats.Add(statistics[i][j]);
                    }
                    res.Add(stats);
                }
            }
        }
        return res;
    }

    public static List<List<string>> getStatisticsDifficulty(int difficulty)
    {
        List<List<string>> res = new List<List<string>>();
        List<string> stats;

        if (statistics.Count > 0)
        {
            for (int i = 0; i < statistics.Count; i++)
            {
                if (statistics[i][1] == "" + difficulty)
                {
                    stats = new List<string>();
                    for (int j = 2; j < 14; j++)
                    {
                        stats.Add(statistics[i][j]);
                    }

                    res.Add(stats);
                }
            }
        }
        return res;
    }

    public static List<string> getStatisticsDifficulty(int difficulty, int ranking)
    {
        List<List<string>> stats = getStatisticsDifficulty(difficulty);

        List<string> res = new List<string>();

        if (ranking >= 0)
        {
            for (int i = 0; i < 12; i++)
            {
                res.Add(stats[ranking][i]);
            }
        }
        return res;
    }


    public List<List<string>> getStatisticsNaamDifficultyTotRank(string naam, int difficulty, int totRank)
    {
        List<List<string>> res = new List<List<string>>();
        List<string> stats;
        int grootte = 0;
        int aantal = 0;

        if (statistics.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                if (statistics[i][0] == naam)
                {
                    if (statistics[i][1] == "" + difficulty)
                    {
                        grootte++;
                    }
                }
            }
        }

        if (totRank > grootte)
        {
            totRank = grootte;
        }

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count; i++)
            {
                if (statistics[i][0] == naam)
                {
                    if (hiscores[i][1] == "" + difficulty)
                    {
                        if (aantal < totRank)
                        {
                            stats = new List<string>();
                            for (int j = 2; j < 14; j++)
                            {
                                stats.Add(statistics[i][j]);
                            }
                            res.Add(stats);
                            aantal++;
                        }
                    }
                }
            }
        }
        return res;
    }

    public void printMatrix(List<List<string>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                Debug.Log("matrix[" + i + "][" + j + "] = " + matrix[i][j]);
            }
        }

    }
}
