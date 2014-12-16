using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
    private int id;
    private float cdPercent;
    private float cdTime;
    private string name;

    public Skill(int id, float cdP, float cdT, string name) {
        this.id = id;
        cdPercent = cdP;
        cdTime = cdT;
        this.name = name;
    }

    public int getID() {
        return id;
    }

    public float getCdPercent() {
        return cdPercent;
    }

    public float getCdTime() {
        return cdTime;
    }

}
