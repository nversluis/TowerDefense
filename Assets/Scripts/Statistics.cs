using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statistics : MonoBehaviour {

    public static int kills;
    public static int guyantKills;
    public static int gwarfKills;
    public static int grobbleKills;
    public static int headshots;
    public static int killStreak;
    public static int headShotStreak;
    public static int playerKills;
    public static int towerKills;

    public static int killsScore;
    public static int guyantKillsScore;
    public static int gwarfKillsScore;
    public static int grobbleKillsScore;
    public static int headshotsScore;
    public static int killStreakScore;
    public static int headShotStreakScore;
    public static int playerKillsScore;
    public static int towerKillsScore;

    public static float lastKillTime = 0;
    public static int score;
    public static int currentWave;

    public static int currentKillStreak;
    public static int currentHeadShotStreak;

	public static int fireTrapsBuilt;
	public static int iceTrapsBuilt;
	public static int poisonTrapsBuilt;
	public static int magicTowersBuilt;
	public static int arrowTowersBuilt;
	public static int barricadesBuilt;


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
        //currentWave = resourceManager.currentWave;

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

        //Debug.Log("score = " + score);
        return score;
    }





    void Update()
    {
        Statistics.score = Score();
        Statistics.currentWave = resourceManager.currentWave;
    }

}
