using UnityEngine;
using System.Collections;

public class Item {
    /* PARAMETERS */
    private string type;
    private int tier;
    private float[] cost;
    private float[] value;

    /* CONSTRUCTORS */
    public Item(string type, int tier, float[] cost, float[] value) {
        this.type = type;
        this.tier = tier;
        this.cost = cost;
        this.value = value;
    }

    public Item() {
        type = "none";
        tier = 1;
        cost = new float[5] {1000f, 5000f, 10000f, 75000f};
        value = new float[5] {5f, 20f, 75f, 150f};
    }

    /* GETTERS */
    public string getType() {
        return type;
    }

    public int getTier() {
        return tier;
    }

    public float[] getCost() {
        return cost;
    }

    public float[] getValue() {
        return value;
    }

    /* SETTERS */
    public void setType(string type) {
        this.type = type;
    }

    public void setTier(int tier) {
        this.tier = tier;
    }

    public void setCost(float[] cost) {
        this.cost = cost;
    }

    public void setValue(float[] value) {
        this.value = value;
    }


}
