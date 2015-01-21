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

    float counter;
    int waitTime = 10;

    static WWW wwwScores;
    WWW wwwStatistics;

    // Use this for initialization
    void Start()
    {
        counter = 0;

        hiscores = new List<List<string>>();
        statistics = new List<List<string>>();

        urlScores = "http://drproject.twi.tudelft.nl:8087/getScore";
        wwwScores = new WWW(urlScores);
        StartCoroutine(WaitForRequest(wwwScores));

        urlStatistics = "http://drproject.twi.tudelft.nl:8087/getStatistics";
        wwwStatistics = new WWW(urlStatistics);
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
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setStatistics?naam=" + naam + "&difficulty=" + ResourceManager.Difficulty + "&score=" + Statistics.score + "&waves=" + Statistics.currentWave
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
            // Naam
            stats.Add(split[i]);
            // Difficulty
            stats.Add(split[i + 1]);
            // Score
            stats.Add(split[i + 2]);
            // Waves
            stats.Add(split[i + 3]);
            // Kills
            stats.Add(split[i + 4]);
            // Killstreak
            stats.Add(split[i + 5]);
            // Headshots
            stats.Add(split[i + 6]);
            // Headshotstreak
            stats.Add(split[i + 7]);
            // Firetrapsbuilt
            stats.Add(split[i + 8]);
            // Icetrapsbuilt
            stats.Add(split[i + 9]);
            // Poisontrapsbuilt
            stats.Add(split[i + 10]);
            // Magictowersbuilt
            stats.Add(split[i + 11]);
            // Arrowtowersbuilt
            stats.Add(split[i + 12]);
            // Barricadebuilt
            stats.Add(split[i + 13]);

            statistics.Add(stats);
        }
    }

    public static List<List<string>> getHiscores()
    {
        List<List<string>> hiscoresTotRank = new List<List<string>>();
        List<string> score;

        if (hiscores.Count != 0)
        {
            for (int i = 0; i < hiscores.Count / 2; i++)
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
            for (int i = 0; i < hiscores.Count / 2; i++)
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

    public static int getPositionOnHiscores(string naam)
    {
        if (connected)
        {
            getScoreFromServer();
        }

        for (int i = 0; i < hiscores.Count; i++)
        {
            if (hiscores[i][0] == naam && hiscores[i][2] == "" + Statistics.score)
                return i + 1;
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
            for (int i = 0; i < hiscores.Count / 2; i++)
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
            for (int i = 0; i < hiscores.Count / 2; i++)
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

    public List<List<string>> getStatistics()
    {
        List<List<string>> res = new List<List<string>>();

        if (statistics.Count > 0)
        {
            for (int i = 0; i < statistics.Count / 2; i++)
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
            for (int i = 0; i < statistics.Count / 2; i++)
            {
                if (statistics[i][0] == naam)
                {
                    stats = new List<string>();
                    // Difficulty
                    stats.Add(statistics[i][1]);
                    // Score
                    stats.Add(statistics[i][2]);
                    // Waves
                    stats.Add(statistics[i][3]);
                    // Kills
                    stats.Add(statistics[i][4]);
                    // Killstreak
                    stats.Add(statistics[i][5]);
                    // Headshots
                    stats.Add(statistics[i][6]);
                    // Headshotstreak
                    stats.Add(statistics[i][7]);
                    // Firetrapsbuilt
                    stats.Add(statistics[i][8]);
                    // Icetrapsbuilt
                    stats.Add(statistics[i][9]);
                    // Poisontrapsbuilt
                    stats.Add(statistics[i][10]);
                    // Magictowersbuilt
                    stats.Add(statistics[i][11]);
                    // Arrowtowersbuilt
                    stats.Add(statistics[i][12]);
                    // Barricadebuilt
                    stats.Add(statistics[i][13]);

                    res.Add(stats);
                }
            }
        }
        return res;
    }

    public List<List<string>> getStatisticsDifficulty(int difficulty)
    {
        List<List<string>> res = new List<List<string>>();
        List<string> stats;

        if (statistics.Count > 0)
        {
            for (int i = 0; i < statistics.Count / 2; i++)
            {
                if (statistics[i][1] == "" + difficulty)
                {
                    stats = new List<string>();
                    // Naam
                    stats.Add(statistics[i][0]);
                    // Score
                    stats.Add(statistics[i][2]);
                    // Waves
                    stats.Add(statistics[i][3]);
                    // Kills
                    stats.Add(statistics[i][4]);
                    // Killstreak
                    stats.Add(statistics[i][5]);
                    // Headshots
                    stats.Add(statistics[i][6]);
                    // Headshotstreak
                    stats.Add(statistics[i][7]);
                    // Firetrapsbuilt
                    stats.Add(statistics[i][8]);
                    // Icetrapsbuilt
                    stats.Add(statistics[i][9]);
                    // Poisontrapsbuilt
                    stats.Add(statistics[i][10]);
                    // Magictowersbuilt
                    stats.Add(statistics[i][11]);
                    // Arrowtowersbuilt
                    stats.Add(statistics[i][12]);
                    // Barricadebuilt
                    stats.Add(statistics[i][13]);

                    res.Add(stats);
                }
            }
        }
        return res;
    }

    public List<string> getStatisticsDifficulty(int difficulty, int ranking)
    {
        List<List<string>> stats = getStatisticsDifficulty(difficulty);

        List<string> res = new List<string>();

        if (ranking >= 0)
        {
            // Naam
            res.Add(stats[ranking][0]);
            // Difficulty
            res.Add(stats[ranking][1]);
            // Score
            res.Add(stats[ranking][2]);
            // Waves
            res.Add(stats[ranking][3]);
            // Kills
            res.Add(stats[ranking][4]);
            // Killstreak
            res.Add(stats[ranking][5]);
            // Headshots
            res.Add(stats[ranking][6]);
            // Headshotstreak
            res.Add(stats[ranking][7]);
            // Firetrapsbuilt
            res.Add(stats[ranking][8]);
            // Icetrapsbuilt
            res.Add(stats[ranking][9]);
            // Poisontrapsbuilt
            res.Add(stats[ranking][10]);
            // Magictowersbuilt
            res.Add(stats[ranking][11]);
            // Arrowtowersbuilt
            res.Add(stats[ranking][12]);
            // Barricadebuilt
            res.Add(stats[ranking][13]);
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
            for (int i = 0; i < hiscores.Count / 2; i++)
            {
                if (statistics[i][0] == naam)
                {
                    if (hiscores[i][1] == "" + difficulty)
                    {
                        if (aantal < totRank)
                        {
                            stats = new List<string>();
                            // Score
                            stats.Add(statistics[i][2]);
                            // Waves
                            stats.Add(statistics[i][3]);
                            // Kills
                            stats.Add(statistics[i][4]);
                            // Killstreak
                            stats.Add(statistics[i][5]);
                            // Headshots
                            stats.Add(statistics[i][6]);
                            // Headshotstreak
                            stats.Add(statistics[i][7]);
                            // Firetrapsbuilt
                            stats.Add(statistics[i][8]);
                            // Icetrapsbuilt
                            stats.Add(statistics[i][9]);
                            // Poisontrapsbuilt
                            stats.Add(statistics[i][10]);
                            // Magictowersbuilt
                            stats.Add(statistics[i][11]);
                            // Arrowtowersbuilt
                            stats.Add(statistics[i][12]);
                            // Barricadebuilt
                            stats.Add(statistics[i][13]);

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
