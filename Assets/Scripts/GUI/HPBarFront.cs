using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBarFront : MonoBehaviour {
	private float currentHP;
	private float bufferedHP;
	private float maxHP;
    private int i;

	public RectTransform rect;
	
	
	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
        currentHP = 0f;
        bufferedHP = 0f;
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(i == 1) {
            currentHP = 100f;
            i++;
        }
        else if(i == 300) {
            currentHP = 50f;
        }
        else {
            i++;
        }
		maxHP = 100f;
		if (bufferedHP < currentHP) {
			if (System.Math.Abs(bufferedHP - currentHP) < (maxHP/100f)) {
				bufferedHP = currentHP;
			}
			else{
				bufferedHP += (currentHP - bufferedHP) / 60;
			}
		}
		else{
			bufferedHP = currentHP;
		}
		rect.localScale = new Vector3((bufferedHP/maxHP),1,1);
	}
}
