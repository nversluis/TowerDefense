using UnityEngine;
using UnityEngine.UI;
using System.Collections; 


public class Cooldown : MonoBehaviour {
	float cdPercentage;
	int currentMana;
	int neededMana;
    float cooldownTime;

	private Image image;
    private Text text;
    private int i;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
		image.fillClockwise = false;
		cdPercentage = 100;
		currentMana = 30;
		neededMana = 20;
        cooldownTime = 4;
        text.enabled = false;
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(cdPercentage > 0){
            if(i == 2) {
                cdPercentage--;
                i = 0;
            }
            i++;
		}

		if (currentMana < neededMana) {
            text.enabled = false;
			image.color = new Color (1, 1, 1, 0.75f);
			image.fillAmount = 1;
		}
		else{
            text.enabled = true;
            float timeRemaining = cdPercentage * cooldownTime / 100;
            if(timeRemaining < 1) {
                if(timeRemaining == 0) {
                    text.enabled = false;
                }
                else {
                    text.text = timeRemaining.ToString("0.0");
                }
            }
            else {
                text.text = Mathf.RoundToInt(timeRemaining).ToString();
            }
            
			image.color = new Color (0, 0, 0, 0.75f);
			image.fillAmount = cdPercentage/100f;
		}
	}
}
