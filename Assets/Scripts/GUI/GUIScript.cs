using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {
    
    [Header("HP")]
    public RectTransform frontHPBar;
    public RectTransform rearHPBar;

    private float rearBufferedHP = player.getCurrentHP();
    private float frontBufferedHP = player.getCurrentHP();

    [Header("Gold")]
    public Text goldText;

    [Header("Score")]
    public Text scoreText;

    [Header("Stats")]
    public Text StrStat;
    public Text MagStat;
    public Text DefStat;
    public Text AgiStat;

    [Header("Skill icons")]
    public Image skill1;
    public Image skill2;
    public Image skill3;
    public Image skill4;

    private Text skillText1;
    private Text skillText2;
    private Text skillText3;
    private Text skillText4;

    private Text skillCooldown1;
    private Text skillCooldown2;
    private Text skillCooldown3;
    private Text skillCooldown4;

    private List<Image> skillIconList = new List<Image>();
    private List<Text> skillTextList = new List<Text>();
    private List<Text> skillCooldownList = new List<Text>();

    [Header("Tower icons")]
    public Image tower1;
    public Image tower2;
    public Image tower3;
    public Image tower4;
    public Image tower5;
    public Image tower6;
    public Image tower7;

    private Text towerText1;
    private Text towerText2;
    private Text towerText3;
    private Text towerText4;
    private Text towerText5;
    private Text towerText6;
    private Text towerText7;

    private List<Image> towerIconList = new List<Image>();
    private List<Text> towerTextList = new List<Text>();

    [Header("Item icons")]
    public Image item1;
    public Image item2;
    public Image item3;
    public Image item4;

    private List<Image> itemIconList = new List<Image>();

    [Header("Pause menu canvas")]
    public GameObject canvas;
    private bool pause;

    [Header("Result screen canvas")]
    public GameObject result;

    [Header("Crosshair")]
    public GameObject crosshair;
    public Text resultText;
    public Text resultScoreText; 

	[Header("Tower Popup")]
	public GameObject TowerPopup;

    // Scripts
    private GameObject playerObject;
    private GameObject cameraObject;
    private PlayerController playerScript;
    private CameraController cameraScript;

    // Initialize a public variable containing all player data
    public static PlayerData player = new PlayerData();

    // OptionButton variable
    private bool options = true;

	void Start () {
        /* Get private components */

        // Skills

        skillText1 = skill1.transform.FindChild("Key").GetComponent<Text>();
        skillText2 = skill2.transform.FindChild("Key").GetComponent<Text>();
        skillText3 = skill3.transform.FindChild("Key").GetComponent<Text>();
        skillText4 = skill4.transform.FindChild("Key").GetComponent<Text>();

        skillCooldown1 = skill1.transform.FindChild("Cooldown").GetComponent<Text>();
        skillCooldown2 = skill2.transform.FindChild("Cooldown").GetComponent<Text>();
        skillCooldown3 = skill3.transform.FindChild("Cooldown").GetComponent<Text>();
        skillCooldown4 = skill4.transform.FindChild("Cooldown").GetComponent<Text>();

        // Towers

        towerText1 = tower1.GetComponentInChildren<Text>();
        towerText2 = tower2.GetComponentInChildren<Text>();
        towerText3 = tower3.GetComponentInChildren<Text>();
        towerText4 = tower4.GetComponentInChildren<Text>();
        towerText5 = tower5.GetComponentInChildren<Text>();
        towerText6 = tower6.GetComponentInChildren<Text>();
        towerText7 = tower7.GetComponentInChildren<Text>();
        
        // Scripts

        playerObject = GameObject.Find("Player");
        cameraObject = GameObject.Find("Main Camera");

        playerScript = playerObject.GetComponent<PlayerController>();
        cameraScript = cameraObject.GetComponent<CameraController>();

        /* Build lists */

        // Skills

        skillIconList.Add(skill1);
        skillIconList.Add(skill2);
        skillIconList.Add(skill3);
        skillIconList.Add(skill4);

        skillTextList.Add(skillText1);
        skillTextList.Add(skillText2);
        skillTextList.Add(skillText3);
        skillTextList.Add(skillText4);

        skillCooldownList.Add(skillCooldown1);
        skillCooldownList.Add(skillCooldown2);
        skillCooldownList.Add(skillCooldown3);
        skillCooldownList.Add(skillCooldown4);

        // Towers

        towerIconList.Add(tower1);
        towerIconList.Add(tower2);
        towerIconList.Add(tower3);
        towerIconList.Add(tower4);
        towerIconList.Add(tower5);
        towerIconList.Add(tower6);
        towerIconList.Add(tower7);

        towerTextList.Add(towerText1);
        towerTextList.Add(towerText2);
        towerTextList.Add(towerText3);
        towerTextList.Add(towerText4);
        towerTextList.Add(towerText5);
        towerTextList.Add(towerText6);
        towerTextList.Add(towerText7);

        // Items

        itemIconList.Add(item1);
        itemIconList.Add(item2);
        itemIconList.Add(item3);
        itemIconList.Add(item4);

        /* Initialize */

        // Skills 
        foreach(Image im in skillIconList) {
            im.fillClockwise = false;
            im.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 180f / 255f);
        }

        for(int i = 0; i < skillTextList.Count; i++){
            Text tx = skillTextList[i];
            tx.text = (i + 1).ToString();
        }

        foreach(Text tx in skillCooldownList) {
            tx.enabled = false;
        }

        // Towers
        for(int i = 0; i < towerTextList.Count; i++) {
            Text tx = towerTextList[i];
            tx.enabled = true;
            tx.text = (i + 1).ToString();
        }

        // Pause menu
        canvas.SetActive(false);
        pause = false;

        // Result menu
        result.SetActive(false);

        // Crosshair
        crosshair.SetActive(true);

	}
	
	void FixedUpdate () {
        // Update variables that need to be updated frequently
        UpdateFrontHP();
        UpdateRearHP();
        //UpdateCooldowns();
        UpdateScore();
        UpdateGold();
        UpdateStats();
        UpdateTowers();
        //UpdateItems();
	}

    void Update() {
        // Pause menu behaviour
        if(Input.GetKeyDown("escape")) {
            PauseGame();
        }
    }

    void UpdateGold() {
        goldText.text = player.getGold().ToString();
    }

    void UpdateScore() {
        scoreText.text = player.getScore().ToString();
    }

    void UpdateStats() {
        StrStat.text = player.getAttack().ToString();
        MagStat.text = player.getMagic().ToString();
        DefStat.text = player.getArmor().ToString();
        AgiStat.text = player.getAgility().ToString();
    }

    void UpdateCooldowns() {
        List<Skill> skillset = player.getSkills();
        for(int i = 0; i < skillset.Count; i++) {
            Image icon = skillIconList[i];
            Text text = skillTextList[i];
            Skill skill = skillset[i];
            float cdPercentage = skill.getCdPercent();
            float cooldownTime = skill.getCdTime();

            float timeRemaining = cdPercentage * cooldownTime / 100;
            if(timeRemaining < 1) {
                if(timeRemaining == 0) {
                    text.enabled = false;
                }
                else {
                    text.text = timeRemaining.ToString("0.0");
                    text.enabled = true;
                }
            }
            else {
                text.text = Mathf.RoundToInt(timeRemaining).ToString();
                text.enabled = true;
            }
            icon.fillAmount = cdPercentage / 100f;
        }
    }

    void UpdateFrontHP() {
        float currentHP = player.getCurrentHP();
        float maxHP = player.getMaxHP();

        if(frontBufferedHP < currentHP) {
            if(System.Math.Abs(frontBufferedHP - currentHP) < (maxHP / 1000f)) {
                frontBufferedHP = currentHP;
            }
            else {
                frontBufferedHP += (currentHP - frontBufferedHP) / 30;
            }
        }
        else {
            frontBufferedHP = currentHP;
        }
        frontHPBar.localScale = new Vector3((frontBufferedHP / maxHP), 1, 1);
    }

    void UpdateRearHP() {
        float currentHP = player.getCurrentHP();
        float maxHP = player.getMaxHP();


        if(rearBufferedHP > currentHP) {
            if(System.Math.Abs(rearBufferedHP - currentHP) < (maxHP / 1000f)) {
                rearBufferedHP = currentHP;
            }
            else {
                rearBufferedHP += (currentHP - rearBufferedHP) / 30;
            }
        }
        else {
            rearBufferedHP = currentHP;
        }
        rearHPBar.localScale = new Vector3((rearBufferedHP / maxHP), 1, 1);
    }

    public void UpdateTowers() {
        int currentTower = player.getTower();

        for(int i = 0; i < towerIconList.Count; i++) {
            Image tower = towerIconList[i];
            if(i == currentTower){
                tower.color = new Color(0, 0, 0, 0.75f);
            }
            else {
                tower.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void UpdateItems() {
        List<Item> inventory = player.getItems();

        for(int i = 0; i < inventory.Count; i++) {
            Item item = inventory[i];
            Image image = itemIconList[i];

            switch(item.getTier()) {
                case 1:
                    image.color = new Color(1, 0, 0, 1);
                    break;
                case 2:
                    image.color = new Color(0, 1, 0, 1);
                    break;
                case 3:
                    image.color = new Color(0, 0, 1, 1);
                    break;
                case 4:
                    image.color = new Color(1, 1, 0, 1);
                    break;
                default:
                    image.color = new Color(1, 1, 1, 1);
                    break;
            }
        }
    }

    public void PauseGame() {
        if(pause == false) {
            crosshair.SetActive(false);
            playerScript.enabled = false;
            cameraScript.enabled = false;
            Screen.lockCursor = false;
            Screen.showCursor = true;
            canvas.SetActive(true);
            pause = true;
            Time.timeScale = 0;
        }
        else {
            Resume();
        }
    }

    // Functions for the quit and resume buttons
    public void Resume() {
        pause = false;
        canvas.SetActive(false);
        Time.timeScale = 1;
        crosshair.SetActive(true);
        Screen.showCursor = false;
        Screen.lockCursor = true;
        playerScript.enabled = true;
        cameraScript.enabled = true;
    }

    public void Quit() {
        pause = false;
        Time.timeScale = 1;
        Application.LoadLevel("Main Menu");
    }

    public void EndGame(string resultString) {
        result.SetActive(true);
        resultText.text = resultString;
        resultScoreText.text = player.getScore().ToString();
        Screen.showCursor = true;
        Screen.lockCursor = false;
        playerScript.enabled = false;
        cameraScript.enabled = false;
        Time.timeScale = 0;
    }

	public GameObject getPopUpPanel()
	{
		return TowerPopup;
	}

}
