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
    public Image[] skillIconList = new Image[4];

    private List<Text> skillTextList = new List<Text>();
    private List<Text> skillCooldownList = new List<Text>();

    [Header("Tower icons")]
    public Image[] towerIconList = new Image[7];

    private List<Text> towerTextList = new List<Text>();

    [Header("Item icons")]
    public Image[] itemIconList = new Image[4];

    public Sprite[] tier1 = new Sprite[4];
    public Sprite[] tier2 = new Sprite[4];
    public Sprite[] tier3 = new Sprite[4];
    public Sprite[] tier4 = new Sprite[4]; 

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

    [Header("Enemy Popup")]
    public Image enemyFace;
    public Sprite[] enemyFaces = new Sprite[3];
    public Image HP;
    public Text enemyText;
    public GameObject enemyPanel;

    private GameObject camera;
    private RectTransform rect;
    private LayerMask enemyMask = ((1 << 12) | (1 << 10));
    private RaycastHit hit;

    private float currentHP;
    private float maxHP;

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

        foreach(Image im in skillIconList) {
            skillTextList.Add(im.transform.FindChild("Key").GetComponent<Text>());
            skillCooldownList.Add(im.transform.FindChild("Cooldown").GetComponent<Text>());
        }

        // Towers

        foreach(Image im in towerIconList) {
            towerTextList.Add(im.GetComponentInChildren<Text>());
        }
        
        // Scripts

        playerObject = GameObject.Find("Player");
        cameraObject = GameObject.Find("Main Camera");

        playerScript = playerObject.GetComponent<PlayerController>();
        cameraScript = cameraObject.GetComponent<CameraController>();

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

        // Enemy Popup
        enemyPanel.SetActive(false);
        rect = HP.GetComponent<RectTransform>();
        camera = GameObject.Find("Main Camera");
        currentHP = 100;
        maxHP = 100;

	}
	
	void FixedUpdate () {
        // Update variables that need to be updated frequently
        UpdateFrontHP();
        UpdateRearHP();
        UpdateCooldowns();
        UpdateScore();
        UpdateGold();
        UpdateStats();
        UpdateTowers();
        UpdateItems();
        UpdateEnemyStats();
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

    void UpdateEnemyStats() {
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, enemyMask) && hit.transform.tag == "Enemy") {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            currentHP = enemyHealth.currentHealth;
            maxHP = enemyHealth.startingHealth;
            enemyText.text = hit.transform.name;
            rect.localScale = new Vector3((currentHP / maxHP), 1, 1);
            enemyPanel.SetActive(true);
        }
        else {
            enemyPanel.SetActive(false);
        }
    }

    public void UpdateTowers() {
        int currentTower = player.getTower();

        for(int i = 0; i < towerIconList.Length; i++) {
            Image tower = towerIconList[i];
            if(i == currentTower){
                tower.color = new Color(1, 1, 1, 0.75f);
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
                    image.sprite = tier1[i];
                    break;
                case 2:
                    image.sprite = tier2[i];
                    break;
                case 3:
                    image.sprite = tier3[i];
                    break;
                case 4:
                    image.sprite = tier4[i];
                    break;
                default:
                    image.sprite = tier1[i];
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
