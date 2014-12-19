using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {
    /* PARAMETERS */
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

    /* CONSTRUCTORS */
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

    public PlayerData() {
        currentHP = 100;
        maxHP = 100;
        gold = 1337;
        score = 9001;
        attackStat = 10;
        magicStat = 10;
        armorStat = 10;
        agilityStat = 10;
        selectedTower = 0;
        skillset = new List<Skill>();
        inventory = new List<Item>();

        for(int i = 0; i < 4; i++) {
            skillset.Add(new Skill());
            inventory.Add(new Item());
        }
    }

    /* GETTERS */
    public float getCurrentHP() {
        return currentHP;
    }

    public float getMaxHP() {
        return maxHP;
    }

    public float getGold() {
        return gold;
    }

    public float getScore() {
        return score;
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

    public int getTower() {
        return selectedTower;
    }

    public List<Skill> getSkills() {
        return skillset;
    }

    public List<Item> getItems() {
        return inventory;
    }

    /* SETTERS */
    public void setCurrentHP(float HP) {
        currentHP = HP;
    }

    public void setMaxHP(float HP) {
         maxHP = HP;
    }

    public void setGold(float gold) {
        this.gold = gold;
    }

    public void setScore(float score) {
        this.score = score;
    }

    public void setAttack(float statValue) {
        attackStat = statValue;
    }

    public void setMagic(float statValue) {
        magicStat = statValue;
    }

    public void setArmor(float statValue) {
        armorStat = statValue;
    }

    public void setAgility(float statValue) {
        agilityStat = statValue;
    }

    public void setTower(int towerNumber) {
        selectedTower = towerNumber;
    }

    public void addToSkills(Skill skill) {
        skillset.Add(skill);
    }

    public void getItems(Item item) {
        inventory.Add(item);
    }
}
