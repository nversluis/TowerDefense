using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {
    private float currentHP;
    private float maxHP;
    private float gold;
    private float score;
    private float attackStat;
    private float magicStat;
    private float armorStat;
    private float agilityStat;
    private int selectedTower;
    private List<Skill> skillset;
    private List<Item> inventory;

    public PlayerData(float currHP, float mHP, float gold, float score, float ad, float ap, float armor, float agi, int tower, List<Skill> skills, List<Item> items) {
        currentHP = currHP;
        maxHP = mHP;
        this.gold = gold;
        this.score = score;
        attackStat = ad;
        magicStat = ap;
        armorStat = armor;
        agilityStat = agi;
        selectedTower = tower;
        skillset = skills;
        inventory = items;
    }

    public float getCurrentHP(){
        return currentHP;
    }

    public float getMaxHP() {
        return maxHP;
    }

    public float getGold() {
        return gold;
    }

    public float getAttack() {
        return attackStat;
    }

    public float getMagic() {
        return magicStat;
    }

    public float getArmor() {
        return armorStat;
    }

    public float getAgility() {
        return agilityStat;
    }

    public List<Skill> getSkills() {
        return skillset;
    }

    public List<Item> getItems() {
        return inventory;
    }
}
