using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
    /* PARAMETERS */
    private int id;
    private float cdPercent;
    private float cdTime;
    private string name;

    /* CONSTRUCTORS */
    public Skill(int id, float cdP, float cdT) {
        this.id = id;
        cdPercent = cdP;
        cdTime = cdT;
    }

    public Skill() {
        id = 0;
        cdPercent = 0;
        cdTime = 5;
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
}
