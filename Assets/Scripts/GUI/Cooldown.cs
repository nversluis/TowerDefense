using UnityEngine;
using UnityEngine.UI;
using System.Collections; 


public class Cooldown : MonoBehaviour {
	float cdPercentage;
	int currentMana;
	int neededMana;
    float cooldownTime;

	private Image image;
    public Text text;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
        text = GetComponent<Text>();
		image.fillClockwise = false;
		cdPercentage = 100;
		currentMana = 30;
		neededMana = 20;
        cooldownTime = 10;
        text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(cdPercentage > 0){
			cdPercentage--;
		}

		if (currentMana < neededMana) {
            text.enabled = false;
			image.color = new Color (1, 1, 1, 0.75f);
			image.fillAmount = 1;
		}
		else{
            text.enabled = true;
            float timeRemaining = cdPercentage * cooldownTime;
            if(timeRemaining < 1) {
                text.text = timeRemaining.ToString("0.00");
            }
            else {
                text.text = Mathf.RoundToInt(timeRemaining).ToString();
            }
            
			image.color = new Color (0, 0, 0, 0.75f);
			image.fillAmount = cdPercentage/100f;
		}
	}
}
