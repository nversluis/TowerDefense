using UnityEngine;
using System.Collections;

public class Skill {
    /* PARAMETERS */
    private int id;
    private float cdPercent;
    private float cdTime;
    private string name;

    private float timeStamp;

    /* CONSTRUCTORS */
    public Skill(int id, float cdP, float cdT) {
        this.id = id;
        cdPercent = cdP;
        cdTime = cdT;
        timeStamp = 0;
    }

    public Skill() {
        id = 0;
        cdPercent = 0;
        cdTime = 5;
        timeStamp = 0;
    }

    /* GETTERS */
    public int getID() {
        return id;
    }

    public float getCdPercent() {
        return cdPercent;
    }

    public float getCdTime() {
        return cdTime;
    }

    /* SETTERS */
    public void setID(int id){
        this.id = id;
    }

    public void setCdPercent(float cdP) {
        cdPercent = cdP;
    }

    public void setCdTime(float cdT) {
        cdTime = cdT;
    }

    /* UPDATER */
    public void startCooldown() {
        timeStamp = Time.time + cdTime;
    }

    public void UpdateCooldown() {
        if(Time.time < timeStamp) {
            cdPercent = (timeStamp - Time.time) / cdTime;
        }
        else {
            cdPercent = 0;
        }
    }
}
