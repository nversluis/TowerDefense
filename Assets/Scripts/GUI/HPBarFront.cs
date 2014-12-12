using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBarFront : MonoBehaviour {
	private float currentHP;
	private float bufferedHP;
	private float maxHP;
    private int i;

	private RectTransform rect;
	
	
	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
        /* DEBUG */
        currentHP = 0f;
        bufferedHP = 0f;
        i = 0;
        /* DEBUG */
	}
	
	// Update is called once per frame
	void Update () {
        /* DEBUG */
        if(i == 100) {
            currentHP = 100f;
            i++;
        }
        else if(i == 400) {
            currentHP = 50f;
            i++;
        }
        else if(i == 600) {
            currentHP = 40f;
            i++;
        }
        else if(i == 700) {
            currentHP = 41f;
        }
        else {
            i++;
        }
		maxHP = 100f;
        /* DEBUG */
		if (bufferedHP < currentHP) {
			if (System.Math.Abs(bufferedHP - currentHP) < (maxHP/100f)) {
				bufferedHP = currentHP;
			}
			else{
				bufferedHP += (currentHP - bufferedHP) / 30;
			}
		}
		else{
			bufferedHP = currentHP;
		}
		rect.localScale = new Vector3((bufferedHP/maxHP),1,1);
	}
}
