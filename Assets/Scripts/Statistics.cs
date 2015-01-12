using UnityEngine;
using System.Collections;

public class Statistics : MonoBehaviour {

    static int kills;
    static int guyantKills;
    static int gwarfKills;
    static int grobbleKills;
    static int headshots;
    static int killStreak;
    static int headShotStreak;
    static int playerKills;
    static int towerKills;

    static int killsScore;
    static int guyantKillsScore;
    static int gwarfKillsScore;
    static int grobbleKillsScore;
    static int headshotsScore;
    static int killStreakScore;
    static int headShotStreakScore;
    static int playerKillsScore;
    static int towerKillsScore;

    static float lastKillTime = 0;

    static int currentKillStreak;
    static int currentHeadShotStreak;

    ResourceManager resourceManager;

    void Start()
    {
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        killsScore = resourceManager.killsScore;
        guyantKillsScore = resourceManager.guyantKillsScore;
        gwarfKillsScore = resourceManager.gwarfKillsScore;
        grobbleKillsScore = resourceManager.grobbleKillsScore;
        headshotsScore = resourceManager.headshotsScore;
        killStreakScore = resourceManager.killStreakScore;
        headShotStreakScore = resourceManager.headshotsScore;
        playerKillsScore = resourceManager.playerKillsScore;
        towerKillsScore = resourceManager.towerKillsScore;

        StartStatistics();
    }

    public static void StartStatistics(){
        kills = 0;
        guyantKills = 0;
        gwarfKills = 0;
        grobbleKills = 0;
        headshots = 0;
        killStreak = 0;
        headShotStreak = 0;
        playerKills = 0;
        towerKills = 0;
    }

    public static void Kill(int enemyType, bool killedByPlayer, bool headShot){

        AddKill();

        if (enemyType == 0)
        {
            AddGuyantKill();
        }

        else if (enemyType == 1)
        {
            AddGwarfKill();
        }

        else 
        {
            AddGrobbleKill();
        }

        if (killedByPlayer)
        {
            AddPlayerKill();
        }
        else
        {
            AddTowerKill();
        }

        if(headShot){
            AddHeadShotKill();
        }

        if (Time.realtimeSinceStartup - lastKillTime < 1 || currentKillStreak == 0)
        {
            currentKillStreak += 1;
            if (currentKillStreak > killStreak)
            {
                killStreak += 1;
            }

            if (headShot)
            {
                currentHeadShotStreak += 1;

                if (currentHeadShotStreak > headShotStreak)
                {
                    headShotStreak += 1;
                }
            }
            else
            {
                currentHeadShotStreak = 0;
            }
            
        }
        else
        {
            currentHeadShotStreak = 0;
            currentKillStreak = 0;
        }

        lastKillTime = Time.realtimeSinceStartup;

    }

    static void AddKill(){
        kills += 1;
    }

    static void AddGuyantKill()
    {
        guyantKills += 1;
    }

    static void AddGwarfKill()
    {
        gwarfKills += 1;
    }
    static void AddGrobbleKill()
    {
        grobbleKills += 1;
    }

    static void AddHeadShotKill()
    {
        headshots += 1;
    }

    static void AddPlayerKill()
    {
        playerKills += 1;
    }
    static void AddTowerKill()
    {
        towerKills += 1;
    }

    public static int Score()
    {
        int score = 0;
        score += kills * killsScore;
        score += guyantKills * guyantKillsScore;
        score += gwarfKills * gwarfKillsScore;
        score += grobbleKills * grobbleKillsScore;
        score += headshots * headshotsScore;
        score += killStreak * killStreakScore;
        score += headShotStreak * headShotStreakScore;
        score += playerKills * playerKillsScore;
        score += towerKills * towerKillsScore;

        return score;
    }

    void Update()
    {
    }

}
