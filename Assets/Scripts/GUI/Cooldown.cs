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
    void Start() {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        image.fillClockwise = false;
        image.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 180f / 255f);
        text.enabled = false;
        /* DEBUG */
        cdPercentage = 100;
        cooldownTime = 4;
        i = 0;
        /* DEBUG */
    }

    // Update is called once per frame
    void Update() {
        /* DEBUG */
        if(cdPercentage > 0) {
            if(i == 2) {
                cdPercentage--;
                i = 0;
            }
            i++;
        }
        /* DEBUG */
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
        image.fillAmount = cdPercentage / 100f;
    }
}
