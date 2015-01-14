using UnityEngine;
using System.Collections;

public class Item {
    /* PARAMETERS */
    private string type;
    private int tier;
    private float[] cost;
    private int[] value;

    /* CONSTRUCTORS */
    public Item(int tier, float[] cost, int[] value) {
        this.tier = tier;
        this.cost = cost;
        this.value = value;
    }

    public Item() {
        tier = 1;
        cost = new float[3] {1000f, 5000f, 25000f};
        value = new int[3] {5, 20, 75};
    }

    /* GETTERS */
    public int getTier() {
        return tier;
    }

    public float[] getCost() {
        return cost;
    }

    public int[] getValue() {
        return value;
    }

    /* SETTERS */
    public void setTier(int tier) {
        this.tier = tier;
    }

    public void setCost(float[] cost) {
        this.cost = cost;
    }

    public void setValue(int[] value) {
        this.value = value;
    }


}
