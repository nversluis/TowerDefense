using UnityEngine;
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
    public Text resultScoreText;
    public Sprite[] resultSprites = new Sprite[2];

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


    private GameObject camera;
    private RectTransform rect;
    private LayerMask enemyMask = ((1 << 12) | (1 << 10) | (1 << 8));
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

    [Header("Score Submit")]
    public InputField nameInput;

    [Header("Click sound")]
    public AudioClip click;
    AudioClip countSound;
    AudioClip goSound;

    AudioSource cameraAudioSource;

    // Scripts
    private PlayerController playerScript;
    private CameraController cameraScript;
    private GoalScript goalScript;
    private WaveSpawner waveSpawner;

    private GameObject ResourceManagerObj;
    private ResourceManager resourceManager;

    float volume;
    void Start() {

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
        player.setGold(resourceManager.startGold);
        player.setAttack(resourceManager.startSwordDamage);
        player.setMagic(resourceManager.startMagicDamage);
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
        camera = GameObject.Find("Main Camera");

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
                Time.timeScale = 1; ;
                paused = false;
                pauseShop = false;
            }
            else {
                paused = true;
                pauseShop = true;
                Time.timeScale = 0;
            }

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
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, enemyMask)) {
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

				if (stats.level < 5) {
					levelText = "(Level: " + stats.level + ")";
				} else {
					levelText = "(Level: 5, MAXED)";
				}

				towerName.text = tower.name.Replace("(Clone)", levelText);
                attack.text = "Attack: " + stats.attack;
                speed.text = "Speed: " + stats.speed;
                if(towerName.text.Contains("Ice")) {
                    special.text = "Slowing with: " + stats.specialDamage;
                    specialU.text = "↑" + GameObject.Find("TowerStats").GetComponent<TowerResources>().iceSpecialDamage;
                }
                else {
                    special.text = "";
                    specialU.text = "";
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
                towerName.text = tower.name.Replace("(Clone)", "");
                attack.text = "Health: " + bar.health;
                speed.text = "Maximum Health: " + bar.maxHealth;
                speedU.text = "↑" + (resourceManager.barricadeHealth);
                attackU.text = "↑" + (bar.maxHealth - bar.health);
                special.text = "";
                specialU.text = "";
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
        cameraAudioSource.PlayOneShot(click,volume);
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
            foreach(Image im in skillSelectList) {
                im.enabled = false;
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
        Time.timeScale = 1;
        crosshair.SetActive(true);
        Screen.showCursor = false;
        Screen.lockCursor = true;
        playerScript.enabled = true;
        cameraScript.enabled = true;
    }

    public void Quit() {
        Time.timeScale = 1;
        paused = false;
        Application.LoadLevel("Main Menu");
    }

    public void QuitAfterEnd() {
        ScoreServer.sendScoreToServer(nameInput.text);
        Time.timeScale = 1;
        paused = false;
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
        int currentIndex = inventory[0].getTier() - 1;
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
        int currentIndex = inventory[1].getTier() - 1;
        inventory[1].setTier(inventory[1].getTier() + 1);
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
        int currentIndex = inventory[2].getTier() - 1;
        inventory[2].setTier(inventory[2].getTier() + 1);
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
        int currentIndex = inventory[3].getTier() - 1;
        inventory[3].setTier(inventory[3].getTier() + 1);
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
        /*
        if(player.getTowerSelected()) {
            enemyPanel.SetActive(false);
            towerPanel.SetActive(true);
            GameObject tower = stats.transform.gameObject;

            towerName.text = tower.name.Replace("(Clone)", "");
            attack.text = "Attack: " + stats.attack;
            speed.text = "Speed: " + stats.speed;
            if(towerName.text.Contains("Ice")) {
                special.text = "Slowing with: " + stats.specialDamage;
                specialU.enabled = false;
            }
            else {
                special.text = "";
                specialU.enabled = false;
            }
            sell.enabled = false;
            upgrade.text = "Build(-" + stats.buildCost + ")";
            attackU.text = "↑" + stats.attackUpgrade;
            speedU.text = "↑" + stats.speedUpgrade;
        }
         */
    }

    IEnumerator ImageFlyIn(Sprite[] spLst, int time) {
        for(int i = 0; i <= time; i++) {
            countNumber.sprite = spLst[time - i];
            countAnimator.SetTrigger("Counting");
            yield return new WaitForSeconds(0.45f);

            if (time - i > 0)
            {
                cameraAudioSource.PlayOneShot(countSound, volume * 2);
            }
            else
            {
                cameraAudioSource.PlayOneShot(goSound, volume * 2);
            }
            yield return new WaitForSeconds(0.55f);

        }
        countdownPanel.SetActive(false);
    }

    void DisableNotification() {
        notificationImage.SetActive(false);
    }

}
