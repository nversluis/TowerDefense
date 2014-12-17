using UnityEngine;
using System.Collections;

public class Item {
    /* PARAMETERS */
    private string type;
    private int tier;
    private float cost;
    private float value;

    /* CONSTRUCTORS */
    public Item(string type, int tier, float cost, float value) {
        this.type = type;
        this.tier = tier;
        this.cost = cost;
        this.value = value;
    }

    public Item() {
        type = "none";
        tier = 1;
        cost = 0;
        value = 0;
    }

    /* GETTERS */
    public string getType() {
        return type;
    }

    public int getTier() {
        return tier;
    }

    public float getCost() {
        return cost;
    }

    public float getValue() {
        return value;
    }

    /* SETTERS */
    public void setType(string type) {
        this.type = type;
    }

    public void setTier(int tier) {
        this.tier = tier;
    }

    public void setCost(float cost) {
        this.cost = cost;
    }

    public void setValue(float value) {
        this.value = value;
    }


}
