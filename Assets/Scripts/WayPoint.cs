﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint {
    private Vector3 position;
    private WayPoint previous;
    private List<WayPoint> destinations;
    private string state;
    private float g_cost = 0;
    private float f_cost = 0;
    private float penalty = 0;
	private Vector3 barricade;
	private int barricadeCount = 0;

    public WayPoint(Vector3 pos, string stt = "unexplored") {
        position = pos;
        state = stt;
        destinations = new List<WayPoint>();
    }

    public List<WayPoint> getDestinations() {
        return destinations;
    }

    public Vector3 getPosition() {
        return position;
    }

    public WayPoint getPrevious() {
        return previous;
    }

    public string getState() {
        return state;
    }

    public float getGCost() {
        return g_cost;
    }

    public float getFCost() {
        return f_cost;
    }

    public float getPenalty() {
        return penalty;
    }

    public void setPrevious(WayPoint wp) {
        previous = wp;
    }

    public void setState(string stt) {
        state = stt;
    }

    public void setGCost(float cst) {
        g_cost = cst;
    }

    public void setFCost(float cst) {
        f_cost = cst;
    }

    public void setPenalty(float pnlt) {
        penalty = pnlt;
    }

    public void AddNode(WayPoint node) {
        destinations.Add(node);
    }

	public void setBarricade(Vector3 obj)
	{
		if (barricade != obj) {
			barricade = obj;
			barricadeCount += 1;
		}
		if (barricadeCount >= 2)
			barricadeCount = 2;
	}

	public Vector3 getBarricade()
	{
		return barricade;
	}

	public void removeBarricade()
	{
		barricade = new Vector3 ();
		barricadeCount -= 1;
	}

	public int getBarCount()
	{
		return barricadeCount;
	}
}
