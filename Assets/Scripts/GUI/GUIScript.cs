using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {
    // Initialize a public variable containing all player data
    public static PlayerData player = new PlayerData();

    [Header("Player HP")]
    public RectTransform frontPlayerHPBar;
    public RectTransform rearPlayerHPBar;

    private float rPlayerBufferedHP;
    private float fPlayerBufferedHP;

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
    private List<Image> skillSelectList = new List<Image>();

    [Header("Tower icons")]
    public Image[] towerIconList = new Image[7];

    private List<Text> towerTextList = new List<Text>();
    private List<Image> towerSelectList = new List<Image>();

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
    public Image resultImage;
    public Sprite[] resultSprites = new Sprite[2];

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

    [Header("Wave progress and Gate HP")]
    public Text waveText;
    public Image frontGateHP;
    public Image rearGateHP;
    public Sprite[] HPSprites = new Sprite[3];

    private RectTransform frontGateHPBar;
    private RectTransform rearGateHPBar;
    private float fBufferedGateHP;
    private float rBufferedGateHP;

    [Header("First Wave Text")]
    public Text firstWaveText;

    private bool firstWaveStarted;
    private string shiftDir;

    [Header("Headshot Image")]
    public GameObject headshotImage;

    [Header("Click sound")]
    public AudioClip click;

    AudioSource cameraAudioSource;

    // Scripts
    private PlayerController playerScript;
    private CameraController cameraScript;
    private GoalScript goalScript;
    private WaveSpawner waveSpawner;

    void Start() {
        /* Get private components */

        // Camera Auiodsource
        cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();



        // Skills

        foreach(Image im in skillIconList) {
            skillTextList.Add(im.transform.FindChild("Key").GetComponent<Text>());
            skillCooldownList.Add(im.transform.FindChild("Cooldown").GetComponent<Text>());
            skillSelectList.Add(im.transform.FindChild("Select").GetComponent<Image>());
        }

        // Towers

        foreach(Image im in towerIconList) {
            towerTextList.Add(im.GetComponentInChildren<Text>());
            towerSelectList.Add(im.transform.FindChild("Select").GetComponent<Image>());
        }

        // Gate HP Bars

        frontGateHPBar = frontGateHP.GetComponent<RectTransform>();
        rearGateHPBar = rearGateHP.GetComponent<RectTransform>();

        // Scripts

        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraController>();
        goalScript = GameObject.Find("Goal").GetComponent<GoalScript>();
        waveSpawner = GameObject.Find("EnemySpawner(Clone)").GetComponent<WaveSpawner>();

        /* Initialize */

        // Skills 
        foreach(Image im in skillIconList) {
            im.fillClockwise = false;
            im.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 180f / 255f);
        }

        for(int i = 0; i < skillTextList.Count; i++) {
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

        // Buffered HP's
        fPlayerBufferedHP = 0;
        rPlayerBufferedHP = 0;
        fBufferedGateHP = 0;
        rBufferedGateHP = 0;

        // Starting Text
        firstWaveText.enabled = true;
        firstWaveText.color = new Color(1, 1, 1, 1);
        firstWaveStarted = false;
        shiftDir = "down";
    }

    void FixedUpdate() {
        // Update variables that need to be updated frequently
        // Player HP
        UpdateFrontHP(player.getCurrentHP(), player.getMaxHP(), ref fPlayerBufferedHP, frontPlayerHPBar);
        UpdateRearHP(player.getCurrentHP(), player.getMaxHP(), ref rPlayerBufferedHP, rearPlayerHPBar);
        // Gate HP
        UpdateFrontHP(goalScript.getLives(), goalScript.getMaxLives(), ref fBufferedGateHP, frontGateHPBar);
        UpdateRearHP(goalScript.getLives(), goalScript.getMaxLives(), ref rBufferedGateHP, rearGateHPBar);
        // UI Components
        UpdateGateHPColor();
        UpdateCooldowns();
        UpdateScore();
        UpdateGold();
        UpdateStats();
        UpdateSelection();
        UpdateItems();
        UpdateEnemyStats();
        UpdateWaveText();
    }

    void Update() {
        // Pause menu behaviour
        if(Input.GetKeyDown("escape")) {
            PauseGame();
        }

        if(!firstWaveStarted && Input.GetKeyDown("return")) {
            firstWaveText.enabled = false;
        }
        else if(!firstWaveStarted) {
            TextColorShift(firstWaveText);
        }

        if(Input.GetKeyDown("tab")) {
            player.setTowerSelected(!player.getTowerSelected());
        }

        scoreText.text = Statistics.Score().ToString();
        resultScoreText.text = Statistics.Score().ToString();
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

    void UpdateFrontHP(float currentHP, float maxHP, ref float bufferedHP, RectTransform frontBar) {

        if(bufferedHP < currentHP) {
            if(System.Math.Abs(bufferedHP - currentHP) < (maxHP / 1000f)) {
                bufferedHP = currentHP;
            }
            else {
                bufferedHP += (currentHP - bufferedHP) / 30;
            }
        }
        else {
            bufferedHP = currentHP;
        }
        frontBar.localScale = new Vector3((bufferedHP / maxHP), 1, 1);
    }

    void UpdateRearHP(float currentHP, float maxHP, ref float bufferedHP, RectTransform rearBar) {

        if(bufferedHP > currentHP) {
            if(System.Math.Abs(bufferedHP - currentHP) < (maxHP / 1000f)) {
                bufferedHP = currentHP;
            }
            else {
                bufferedHP += (currentHP - bufferedHP) / 30;
            }
        }
        else {
            bufferedHP = currentHP;
        }
        rearBar.localScale = new Vector3((bufferedHP / maxHP), 1, 1);
    }

    void UpdateEnemyStats() {
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, enemyMask) && hit.transform.tag == "Enemy") {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            currentHP = enemyHealth.currentHealth;
            maxHP = enemyHealth.startingHealth;
            enemyText.text = hit.transform.name;
            switch(hit.transform.name) {
                case "Guyant":
                    enemyFace.sprite = enemyFaces[0];
                    break;
                case "Gwarf":
                    enemyFace.sprite = enemyFaces[1];
                    break;
                case "Grobble":
                    enemyFace.sprite = enemyFaces[2];
                    break;
            }
            rect.localScale = new Vector3((currentHP / maxHP), 1, 1);
            enemyPanel.SetActive(true);
        }
        else {
            enemyPanel.SetActive(false);
        }
    }

    void UpdateWaveText() {
        waveText.text = waveSpawner.GetCurrentWave().ToString() + " of " + waveSpawner.GetMaxWave().ToString();
    }

    void UpdateGateHPColor() {
        float HPPercent = goalScript.getLives() / goalScript.getMaxLives();
        if(HPPercent >= 0.66f) {
            frontGateHP.sprite = HPSprites[0];
        }
        else if(HPPercent >= 0.33f) {
            frontGateHP.sprite = HPSprites[1];
        }
        else {
            frontGateHP.sprite = HPSprites[2];
        }
    }

    void TextColorShift(Text text) {

        if(shiftDir.Equals("down")) {
            if(text.color.r > 0.01f) {
                text.color = text.color - new Color(0.01f, 0, 0, 0);
            }
            else if(text.color.g > 0.01f) {
                text.color = text.color - new Color(0, 0.01f, 0, 0);
            }
            else if(text.color.b > 0.01f) {
                text.color = text.color - new Color(0, 0, 0.01f, 0);
            }
            else {
                shiftDir = "up";
            }
        }
        else {
            if(text.color.r < 1) {
                text.color = text.color + new Color(0.01f, 0, 0, 0);
            }
            else if(text.color.g < 1) {
                text.color = text.color + new Color(0, 0.01f, 0, 0);
            }
            else if(text.color.b < 1) {
                text.color = text.color + new Color(0, 0, 0.01f, 0);
            }
            else {
                shiftDir = "down";
            }
        }
    }

    public void ButtonClick() {
        cameraAudioSource.PlayOneShot(click);
    }

    public void UpdateSelection() {
        bool towerSelected = player.getTowerSelected();
        int currentTower = player.getTower();
        int currentSkill = player.getSkill();

        if(towerSelected) {
            for(int i = 0; i < towerIconList.Length; i++) {
                Image tower = towerIconList[i];
                if(i == currentTower) {
                    towerSelectList[i].enabled = true;
                }
                else {
                    towerSelectList[i].enabled = false;
                }
            }
        }
        else {
            for(int i = 0; i < skillIconList.Length; i++) {
                Image skill = skillIconList[i];
                if(i == currentSkill) {
                    skillSelectList[i].enabled = true;
                }
                else {
                    skillSelectList[i].enabled = false;
                }
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

    public void EndGame(string reason = "none") {
        result.SetActive(true);
        resultScoreText.text = player.getScore().ToString();
        Screen.showCursor = true;
        Screen.lockCursor = false;
        playerScript.enabled = false;
        cameraScript.enabled = false;
        if(reason.Equals("Player")) {
            resultImage.sprite = resultSprites[1];
            frontPlayerHPBar.localScale = new Vector3(0, 1, 1);
            rearPlayerHPBar.localScale = new Vector3(0, 1, 1);
        }
        else if(reason.Equals("Gate")) {
            resultImage.sprite = resultSprites[1];
            frontGateHPBar.localScale = new Vector3(0, 1, 1);
            rearGateHPBar.localScale = new Vector3(0, 1, 1);
        }
        else {
            resultImage.sprite = resultSprites[0];
        }
        Time.timeScale = 0;
    }

    public GameObject getPopUpPanel() {
        return TowerPopup;
    }

    public void HeadShot() {
        headshotImage.SetActive(true);
        Invoke("DisableHeadShot", 1.5f);
    }

    void DisableHeadShot() {
        headshotImage.SetActive(false);
    }

}
