﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {
    // Initialize a public variable containing all player data
    public static PlayerData player = new PlayerData();
    public static bool paused;

    private PlayerHealth playerHealth;
    private PlayerController playerController;

    private List<Item> inventory;
    private List<Skill> skillset;

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
    private bool pauseMenu;

    [Header("Result screen canvas")]
    public GameObject result;
    public Image resultImage;
    public Image scoreBar;
    public Text hiScoreText;
    public Text resultScoreText;
    public Sprite[] resultSprites = new Sprite[2];

    private float bufferedScore;
    private int online = PlayerPrefs.GetInt("Online");

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Tower Popup")]
    public GameObject TowerPopup;

    [Header("Enemy Popup")]
    public Image enemyFace;
    public Sprite[] enemyFaces = new Sprite[3];
    public Image HP;
    public Text enemyText;
    public GameObject enemyPanel;

    [Header("Tower panel")]
    public GameObject towerPanel;
    public Text towerName;
    public Text attack;
    public Text speed;
    public Text special;
    public Text sell;
    public Text upgrade;
    public Text attackU;
    public Text speedU;
    public Text specialU;
    public Text description;
    public Image TowerIM;
    public Sprite[] TowerSprites = new Sprite[7];


    private GameObject cameraMain;
    private RectTransform rect;
    private LayerMask enemyMask = ((1 << 12) | (1 << 10) | (1 << 8));
    private RaycastHit hit;
    private TowerResources towerResources;

    private float currentHP;
    private float maxHP;
    private bool disabledOnce;

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

    [Header("Notifications")]
    public GameObject notificationImage;
    public Sprite[] notificationSprites = new Sprite[3];

    [Header("Item Shop")]
    public GameObject shopPanel;
    public Image[] shopImageList = new Image[4];
    public Text[] shopTextList = new Text[4];
    public Text[] shopCurrentList = new Text[4];
    public Button[] shopButtonList = new Button[4];
    public Text[] costTextList = new Text[4];

    private bool pauseShop;

    [Header("Wave Countdown")]
    public GameObject countdownPanel;
    public Image countNumber;
    public Sprite[] countSpriteList = new Sprite[10];

    private Animator countAnimator;

    [Header("Click sound")]
    public AudioClip click;
    AudioClip countSound;
    AudioClip goSound;

    AudioSource cameraAudioSource;

    public Sprite[] arrows;
    public GameObject arrow;

    // Scripts
    private PlayerController playerScript;
    private CameraController cameraScript;
    private GoalScript goalScript;
    private WaveSpawner waveSpawner;

    private GameObject ResourceManagerObj;
    private ResourceManager resourceManager;

    float volume;
    float timescale;
    void Start() {
        if(!PlayerPrefs.HasKey("hiScore")) {
            PlayerPrefs.SetFloat("hiScore", 0);
        }

        bufferedScore = 0;

        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;

        /* Get private components */
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();

        goSound = resourceManager.goSound;
        countSound = resourceManager.countSound;

        // Camera Auiodsource
        cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();


        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Skills

        foreach(Image im in skillIconList) {
            skillTextList.Add(im.transform.FindChild("Key").GetComponent<Text>());
            skillCooldownList.Add(im.transform.FindChild("Cooldown").GetComponent<Text>());
            skillSelectList.Add(im.transform.parent.FindChild("Select").GetComponent<Image>());
        }

        // Towers

        foreach(Image im in towerIconList) {
            towerTextList.Add(im.GetComponentInChildren<Text>());
            towerSelectList.Add(im.transform.FindChild("Select").GetComponent<Image>());
        }

        towerResources = GameObject.Find("TowerStats").GetComponent<TowerResources>();

        // Gate HP Bars

        frontGateHPBar = frontGateHP.GetComponent<RectTransform>();
        rearGateHPBar = rearGateHP.GetComponent<RectTransform>();

        // Countdown
        countAnimator = countNumber.GetComponent<Animator>();

        // Scripts

        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraController>();
        goalScript = GameObject.Find("Goal").GetComponent<GoalScript>();
        waveSpawner = GameObject.Find("EnemySpawner(Clone)").GetComponent<WaveSpawner>();


        /* Initialize */

        // Player Data
        player.setGold(resourceManager.startGold[ResourceManager.Difficulty]);
        player.setAttack(resourceManager.startSwordDamage[ResourceManager.Difficulty]);
        player.setMagic(resourceManager.startMagicDamage[ResourceManager.Difficulty]);
        player.setArmor(10);
        player.setAgility(10);
        player.setTower(0);
        player.setSkill(0);
        player.setTowerSelected(false);
        List<Item> newItems = new List<Item>();
        // Sword
        newItems.Add(new Item(1, new float[3] { 500f, 1000f, 3000f }, new int[3] { (int)(0.5 * player.getAttack()), (int)(1 * player.getAttack()), (int)(2 * player.getAttack()) }));
        // Wand
        newItems.Add(new Item(1, new float[3] { 500f, 1000f, 3000f }, new int[3] { (int)(0.5 * player.getMagic()), (int)(1 * player.getMagic()), (int)(2 * player.getMagic()) }));
        // Shield
        newItems.Add(new Item(1, new float[3] { 500f, 1000f, 3000f }, new int[3] { (int)(0.5 * player.getArmor()), (int)(1 * player.getArmor()), (int)(2 * player.getArmor()) }));
        // Boots
        newItems.Add(new Item(1, new float[3] { 300f, 750f, 2000f }, new int[3] { (int)(0.1 * player.getAgility()), (int)(0.3 * player.getAgility()), (int)(0.5 * player.getAgility()) }));
        player.setItems(newItems);

        skillset = player.getSkills();
        foreach(Skill sk in skillset){
            sk.setCdPercent(0);
        }
        player.setSkills(skillset);
        inventory = player.getItems();

        // Skills 
        foreach(Image im in skillIconList) {
            im.fillClockwise = false;
            im.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 180f / 255f);
        }

        for(int i = 0; i < skillTextList.Count; i++) {
            Text tx = skillTextList[i];
            tx.enabled = true;
            tx.text = (i + 1).ToString();
        }

        foreach(Text tx in skillCooldownList) {
            tx.enabled = false;
        }

        foreach(Image im in skillSelectList) {
            im.enabled = false;
        }

        // Towers
        for(int i = 0; i < towerTextList.Count; i++) {
            Text tx = towerTextList[i];
            tx.enabled = true;
            tx.text = (i + 1).ToString();
        }

        foreach(Image im in towerSelectList) {
            im.enabled = false;
        }

        disabledOnce = false;

        // Pause menu
        canvas.SetActive(false);
        pauseMenu = false;

        // Result menu
        result.SetActive(false);

        // Crosshair
        crosshair.SetActive(true);

        // Enemy Popup
        enemyPanel.SetActive(false);
        rect = HP.GetComponent<RectTransform>();
        cameraMain = GameObject.Find("Main Camera");

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

        // Headshot image
        notificationImage.SetActive(false);

        // Shop
        shopPanel.SetActive(false);
        pauseShop = false;

        for(int i = 0; i < shopImageList.Length; i++) {
            Image im = shopImageList[i];
            im.sprite = tier2[i];
        }

        for(int i = 0; i < shopTextList.Length; i++) {
            Text tx = shopTextList[i];
            tx.text = "+" + inventory[i].getValue().ToString();
        }

        for(int i = 0; i < shopCurrentList.Length; i++) {
            Text tx = shopCurrentList[i];
            switch(i) {
                case 0:
                    tx.text = playerController.getAtkStat().ToString();
                    break;
                case 1:
                    tx.text = playerController.getMagStat().ToString();
                    break;
                case 2:
                    tx.text = playerHealth.getDefStat().ToString();
                    break;
                case 3:
                    tx.text = playerController.getAgiStat().ToString();
                    break;

            }
        }

        for(int i = 0; i < costTextList.Length; i++) {
            Text tx = costTextList[i];
            tx.text = inventory[i].getCost().ToString();
        }

        // Countdown
        countdownPanel.SetActive(false);
        countNumber.sprite = countSpriteList[0];

        // GUI
        paused = false;

        UpdateStats();
        UpdateShop();
        UpdateItems();
        ResetCooldowns();
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
        UpdateSelection();
        UpdateEnemyStats();
        UpdateWaveText();
        UpdateShop();
        UpdateBuildTower();

        scoreText.text = Statistics.Score().ToString();

        if(online == 0 && Statistics.Score() > PlayerPrefs.GetFloat("hiScore")) {
            PlayerPrefs.SetFloat("hiScore", Statistics.Score());
        }
    }

    void Update() {
        // Pause menu behaviour
        if(pauseShop == false && Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }

        if(!firstWaveStarted && Input.GetKeyDown(KeyCode.Return)) {
            WaveCountdown();
            firstWaveText.enabled = false;
            firstWaveStarted = true;
        }
        else if(!firstWaveStarted) {
            firstWaveText.enabled = true;
            TextColorShift(firstWaveText);
        }

        if(Input.GetKeyDown(KeyCode.Tab)) {
            player.setTowerSelected(!player.getTowerSelected());
        }

        if(pauseMenu == false && Input.GetKeyDown(KeyCode.E)) {
            shopPanel.SetActive(!shopPanel.activeSelf);
            crosshair.SetActive(!crosshair.activeSelf);
            playerScript.enabled = !playerScript.enabled;
            cameraScript.enabled = !cameraScript.enabled;
            Screen.lockCursor = !Screen.lockCursor;
            Screen.showCursor = !Screen.showCursor;
            if(Time.timeScale == 0) {
                Time.timeScale = timescale; ;
                paused = false;
                pauseShop = false;
            }
            else {
                paused = true;
                pauseShop = true;
                timescale = Time.timeScale;
                Time.timeScale = 0;
            }

        }

        if (Time.timeScale != 0)
        {
            if (Time.timeScale < 1.9)
            {
                int i = (int)Mathf.Round(((Time.timeScale - 1) * 5));
                arrow.GetComponent<Image>().sprite = arrows[i];

            }
            else
            {
                int i = 4;
                arrow.GetComponent<Image>().sprite = arrows[i];


            }

        }
    }

    void ResetCooldowns() {
        foreach(Image im in skillIconList) {
            im.fillAmount = 0;
        }
        foreach(Text tx in skillCooldownList) {
            tx.text = "";
        }
    }

    void UpdateGold() {
        goldText.text = player.getGold().ToString();
    }

    void UpdateScore() {
        scoreText.text = player.getScore().ToString();
    }

    void UpdateStats() {
        StrStat.text = "Tier " + inventory[0].getTier().ToString();
        MagStat.text = "Tier " + inventory[1].getTier().ToString();
        DefStat.text = "Tier " + inventory[2].getTier().ToString();
        AgiStat.text = "Tier " + inventory[3].getTier().ToString();
    }

    void UpdateCooldowns() {
        List<Skill> skillset = player.getSkills();
        for(int i = 0; i < skillset.Count; i++) {
            Image icon = skillIconList[i];
            Text text = skillCooldownList[i];
            Skill skill = skillset[i];
            skill.UpdateCooldown();
            float cdPercentage = skill.getCdPercent();
            float cooldownTime = skill.getCdTime();


            float timeRemaining = cdPercentage * cooldownTime;
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
            icon.fillAmount = cdPercentage;
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
        if(Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out hit, Mathf.Infinity, enemyMask)) {
            TowerStats stats = hit.transform.GetComponentInChildren<TowerStats>();
            if(hit.transform.tag == "Enemy") {
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
                towerPanel.SetActive(false);
            }
            else if(stats != null && WeaponController.weapon == 50) {
                enemyPanel.SetActive(false);
                towerPanel.SetActive(true);
                GameObject tower = stats.transform.gameObject;
                string levelText;

                if(stats.level < 5) {
                    levelText = "(Level: " + stats.level + ")";
                }
                else {
                    levelText = "(Level: 5, MAXED)";
                }

                towerName.text = tower.name.Replace("(Clone)", levelText);
                attack.text = "Attack: " + stats.attack;
                speed.text = "Speed: " + stats.speed;
                if(towerName.text.Contains("Ice")) {
                    special.text = "Slowing with: " + stats.specialDamage;
                    specialU.text = "↑" + GameObject.Find("TowerStats").GetComponent<TowerResources>().iceSpecialDamage;
                    TowerIM.sprite = TowerSprites[4];
                }
                else {
                    special.text = "";
                    specialU.text = "";
                }
                if(towerName.text.Contains("Magic")) {
                    TowerIM.sprite = TowerSprites[0];
                }
                else if(towerName.text.Contains("Arrow")) {
                    TowerIM.sprite = TowerSprites[1];
                }
                else if(towerName.text.Contains("Fire")) {
                    TowerIM.sprite = TowerSprites[2];
                }
                else if(towerName.text.Contains("Poison")) {
                    TowerIM.sprite = TowerSprites[3];
                }
                sell.text = "Sell(+" + stats.sellCost + ")";
                upgrade.text = "Upgrade(-" + stats.upgradeCost + ")";
                attackU.text = "↑" + stats.attackUpgrade;
                speedU.text = "↑" + stats.speedUpgrade;

            }
            else if(hit.transform.name.Contains("arricade") && WeaponController.weapon == 50) {
                enemyPanel.SetActive(false);
                towerPanel.SetActive(true);
                GameObject tower = hit.transform.gameObject;
                barricade bar = tower.GetComponent<barricade>();

                string levelText;
                if(bar.level < 5) {
                    levelText = "(Level: " + bar.level + ")";
                }
                else {
                    levelText = "(Level: 5, MAXED)";
                }
                towerName.text = tower.name.Replace("(Clone)", levelText);
                attack.text = "Health: " + bar.health;
                speed.text = "Maximum Health: " + bar.maxHealth;
                speedU.text = "↑" + (resourceManager.barricadeHealth);
                attackU.text = "↑" + (bar.maxHealth - bar.health);
                special.text = "";
                specialU.text = "";
                TowerIM.sprite = TowerSprites[5];
                sell.text = "Sell(+" + bar.totalCost / 2 + ")";
                if(bar.maxHealth != bar.health)
                    upgrade.text = "Repair(-" + (bar.maxHealth - bar.health) + ")";
                else
                    upgrade.text = "Upgrade(-" + bar.cost * bar.maxHealth / resourceManager.barricadeHealth + ")";
            }
            else {
                enemyPanel.SetActive(false);
                towerPanel.SetActive(false);
            }
        }
    }

    void UpdateWaveText() {
        int waveNo = waveSpawner.GetCurrentWave();
        int maxWaveNo = waveSpawner.GetMaxWave();
        waveText.text = waveNo.ToString() + " of " + maxWaveNo.ToString();
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

    void UpdateShop() {
        for(int i = 0; i < shopImageList.Length; i++) {
            Image im = shopImageList[i];
            Item it = inventory[i];
            int tier = it.getTier();

            switch(tier) {
                case 1:
                    im.sprite = tier2[i];
                    break;
                case 2:
                    im.sprite = tier3[i];
                    break;
                case 3:
                    im.sprite = tier4[i];
                    break;
                default:
                    im.sprite = tier4[i];
                    im.color = new Color(0.5f, 0.5f, 0.5f, 1);
                    costTextList[i].text = "Max level";
                    break;
            }
        }


        for(int i = 0; i < shopTextList.Length; i++) {
            Text tx = shopTextList[i];
            if(inventory[i].getTier() < 4) {
                tx.text = "+" + inventory[i].getValue()[inventory[i].getTier() - 1].ToString();
            }
            else {
                tx.text = "MAX";
                tx.color = Color.blue;
            }
        }

        for(int i = 0; i < costTextList.Length; i++) {
            Text tx = costTextList[i];
            if(inventory[i].getTier() < 4) {
                tx.text = inventory[i].getCost()[inventory[i].getTier() - 1].ToString();
            }
            else {
                tx.text = "MAX";
                tx.color = Color.blue;
            }
        }

        for(int i = 0; i < shopButtonList.Length; i++) {
            Button bt = shopButtonList[i];
            if(inventory[i].getTier() < 4 && player.getGold() >= inventory[i].getCost()[inventory[i].getTier() - 1]) {
                bt.interactable = true;
            }
            else {
                bt.interactable = false;
            }
        }

        for(int i = 0; i < shopCurrentList.Length; i++) {
            Text tx = shopCurrentList[i];
            switch(i) {
                case 0:
                    tx.text = playerController.getAtkStat().ToString();
                    break;
                case 1:
                    tx.text = playerController.getMagStat().ToString();
                    break;
                case 2:
                    tx.text = playerHealth.getDefStat().ToString();
                    break;
                case 3:
                    tx.text = playerController.getAgiStat().ToString();
                    break;

            }
        }

    }

    public void ButtonClick() {
        cameraAudioSource.PlayOneShot(click, volume);
    }

    public void UpdateSelection() {
        bool towerSelected = player.getTowerSelected();
        int currentTower = player.getTower();
        int currentSkill = player.getSkill();

        if(towerSelected) {
            for(int i = 0; i < towerIconList.Length; i++) {
                //Image tower = towerIconList[i];
                if(i == currentTower) {
                    towerSelectList[i].enabled = true;
                }
                else {
                    towerSelectList[i].enabled = false;
                }
            }
            foreach(Image im in skillSelectList) {
                im.enabled = false;
            }
        }
        else {
            for(int i = 0; i < skillIconList.Length; i++) {
                //Image skill = skillIconList[i];
                if(i == currentSkill) {
                    skillSelectList[i].enabled = true;
                }
                else {
                    skillSelectList[i].enabled = false;
                }
            }
            foreach(Image im in towerSelectList) {
                im.enabled = false;
            }
        }


    }

    public void UpdateItems() {
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
        if(pauseMenu == false) {
            crosshair.SetActive(false);
            playerScript.enabled = false;
            cameraScript.enabled = false;
            Screen.lockCursor = false;
            Screen.showCursor = true;
            canvas.SetActive(true);
            paused = true;
            pauseMenu = true;
            timescale = Time.timeScale;
            Time.timeScale = 0;
        }
        else {
            Resume();
        }
    }

    // Functions for the quit and resume buttons
    public void Resume() {
        paused = false;
        pauseMenu = false;
        canvas.SetActive(false);
        Time.timeScale = timescale;
        crosshair.SetActive(true);
        Screen.showCursor = false;
        Screen.lockCursor = true;
        playerScript.enabled = true;
        cameraScript.enabled = true;
    }

    public void Quit() {
        Time.timeScale = timescale;
        paused = false;
        Application.LoadLevel("Main Menu");
    }

    public void QuitAfterEnd() {
        ScoreServer.sendScoreToServer(PlayerPrefs.GetString("Login"));
        Time.timeScale = timescale;
        paused = false;
        Application.LoadLevel("Main Menu");
    }

    public void EndGame(string reason = "none") {
        int hiScore;
        scoreText.text = "";
        if(online == 1) {
            try {
                hiScore = int.Parse(ScoreServer.getHiscores(PlayerPrefs.GetInt("Difficulty"))[0][1]);
            }
            catch {
                hiScore = 0;
            }
        }
        else {
            hiScore = (int)PlayerPrefs.GetFloat("hiScore");
        }
        hiScoreText.text = hiScore.ToString();
        result.SetActive(true);
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
        float countScore = Statistics.score;
        StartCoroutine(ScoreCounter(countScore, hiScore, scoreBar, resultScoreText));
        timescale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;
        pauseMenu = true;
        pauseShop = true;
    }

    public GameObject getPopUpPanel() {
        return TowerPopup;
    }

    public void Notification(string reason) {
        Image image = notificationImage.GetComponent<Image>();
        switch(reason) {
            case "Headshot":
                image.sprite = notificationSprites[0];
                break;
            case "NoGold":
                image.sprite = notificationSprites[1];
                break;
            case "LastWave":
                image.sprite = notificationSprites[2];
                break;
            default:
                image.sprite = null;
                break;
        }
        notificationImage.SetActive(true);
        Invoke("DisableNotification", 1.5f);
    }

    public void UpgradeSword() {
        int currentTier = inventory[0].getTier();
        int currentIndex = currentTier - 1;
        inventory[0].setTier(currentTier + 1);
        player.setItems(inventory);
        playerController.addSwordDamage(inventory[0].getValue()[currentIndex]);
        player.removeGold(inventory[0].getCost()[currentIndex]);
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeWand() {
        int currentTier = inventory[1].getTier();
        int currentIndex = currentTier - 1;
        inventory[1].setTier(currentTier + 1);
        player.setItems(inventory);
        playerController.addMagicDamage(inventory[1].getValue()[currentIndex]);
        player.removeGold(inventory[1].getCost()[currentIndex]);
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeShield() {
        int currentTier = inventory[2].getTier();
        int currentIndex = currentTier - 1;
        inventory[2].setTier(currentTier + 1);
        player.setItems(inventory);
        playerHealth.addPlayerDefense(inventory[2].getValue()[currentIndex]);
        player.removeGold(inventory[2].getCost()[currentIndex]);
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeBoots() {
        int currentTier = inventory[3].getTier();
        int currentIndex = currentTier - 1;
        inventory[3].setTier(currentTier + 1);
        player.setItems(inventory);
        playerController.addPlayerSpeed(inventory[3].getValue()[currentIndex]);
        player.removeGold(inventory[3].getCost()[currentIndex]);
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void WaveCountdown(int time = 5) {
        countdownPanel.SetActive(true);
        StartCoroutine(ImageFlyIn(countSpriteList, time));
    }

    public void UpdateBuildTower() {
        if(player.getTowerSelected()) {
            switch(player.getTower()) {
                case 0:
                    towerName.text = "Magic Tower";
                    attack.text = "Attack Damage: " + towerResources.magicAttack.ToString();
                    speed.text = "Attack Speed: " + towerResources.magicSpeed.ToString();
                    special.text = "";
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costMagicTower[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "A simple tower that shoots magical orbs at enemies, dealing magic damage.\nPlace it on a wall.";
                    TowerIM.sprite = TowerSprites[0];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                case 1:
                    towerName.text = "Spear Tower";
                    attack.text = "Attack Damage: " + towerResources.arrowAttack.ToString();
                    speed.text = "Attack Speed: " + towerResources.arrowSpeed.ToString();
                    special.text = "";
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costArrowTower[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "A simple tower that shoots arrow at enemies, dealing physical damage.\nPlace it on a wall.";
                    TowerIM.sprite = TowerSprites[1];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                case 2:
                    towerName.text = "Fire Trap";
                    attack.text = "Attack Damage: " + towerResources.fireAttack.ToString();
                    speed.text = "Attack Speed: " + towerResources.fireAttack.ToString();
                    special.text = "";
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costFireTrap[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "A trap that makes fire erupt from the ground as soon as enemies step on it, dealing magic damage as long as they stand on it.\nPlace it on the floor.";
                    TowerIM.sprite = TowerSprites[2];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                case 3:
                    towerName.text = "Poison Trap";
                    attack.text = "Attack Damage: " + towerResources.poisonAttack.ToString();
                    speed.text = "Attack Speed: " + towerResources.poisonAttack.ToString();
                    special.text = "";
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costPoisonTrap[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "A trap with deadly venom, poisoning enemies the instant they step on it, dealing magic damage over time.\nPlace it on the floor.";
                    TowerIM.sprite = TowerSprites[3];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                case 4:
                    towerName.text = "Ice Trap";
                    attack.text = "Attack Damage: " + towerResources.iceAttack.ToString();
                    speed.text = "Attack Speed: " + towerResources.iceAttack.ToString();
                    special.text = "Slowing Effect: " + towerResources.iceSpecialDamage.ToString();
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costIceTrap[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "An ice cold trap, damaging and slowing down all enemies that touch it.\nPlace it on the floor.";
                    TowerIM.sprite = TowerSprites[4];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                case 5:
                    towerName.text = "Barricade";
                    attack.text = "Health: " + resourceManager.barricadeHealth.ToString();
                    speed.text = "";
                    special.text = "";
                    specialU.text = "";
                    sell.text = "";
                    upgrade.text = "Build(-" + resourceManager.costMagicTower[ResourceManager.Difficulty].ToString() + ")";
                    attackU.text = "";
                    speedU.text = "";
                    description.text = "A wall with spikes so that enemies cannot climb it. They can, however, try to smash it. Use it to stall enemies.\nPlace it on the floor.";
                    TowerIM.sprite = TowerSprites[5];
                    disabledOnce = false;
                    enemyPanel.SetActive(false);
                    towerPanel.SetActive(true);
                    break;
                default:
                    if(!disabledOnce) {
                        description.text = "";
                        towerPanel.SetActive(false);
                        disabledOnce = true;
                    }
                    break;
            }
        }
    }

    IEnumerator ImageFlyIn(Sprite[] spLst, int time) {
        for(int i = 0; i <= time; i++) {
            countNumber.sprite = spLst[time - i];
            countAnimator.SetTrigger("Counting");
            yield return new WaitForSeconds(0.45f);

            if(time - i > 0) {
                cameraAudioSource.PlayOneShot(countSound, volume * 2);
            }
            else {
                cameraAudioSource.PlayOneShot(goSound, volume * 2);
            }
            yield return new WaitForSeconds(0.55f);

        }
        countdownPanel.SetActive(false);
    }

    IEnumerator ScoreCounter(float score, int hiScore, Image img, Text txt) {
        while(bufferedScore != score) {
            if(bufferedScore < score) {
                bufferedScore += (score - bufferedScore) / 60f;
                if(Mathf.Abs(score - bufferedScore) < score / 60f) {
                    bufferedScore = score;
                }
            }
            txt.text = "" + (int)bufferedScore;
            if(bufferedScore < hiScore) {
                img.rectTransform.localScale = new Vector3((float)(bufferedScore / hiScore), 1, 1);
            }
            else {
                img.rectTransform.localScale = new Vector3(1, 1, 1);
            }
            yield return StartCoroutine(WaitForRealSeconds(1 / 1000f));
        }
    }

    IEnumerator WaitForRealSeconds(float time) {
        float start = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < start + time) {
            yield return null;
        }
    }

    void DisableNotification() {
        notificationImage.SetActive(false);
    }

}
