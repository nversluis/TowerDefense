using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    private string name;
    private int tier;
    private float cost;
    private float value;

    public Item(string name, int tier, float cost, float value) {
        this.name = name;
        this.tier = tier;
        this.cost = cost;
        this.value = value;
    }

    public string getName() {
        return name;
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


}
