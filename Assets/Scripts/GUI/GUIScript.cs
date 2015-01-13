using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {
    // Initialize a public variable containing all player data
    public static PlayerData player = new PlayerData();
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
    private bool pause;

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

    [Header("Item Shop")]
    public GameObject shopPanel;
    public Image[] shopImageList = new Image[4];
    public Text[] shopTextList = new Text[4];
    public Button[] shopButtonList = new Button[4];
    public Text[] costTextList = new Text[4];


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

        // Scripts

        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraController>();
        goalScript = GameObject.Find("Goal").GetComponent<GoalScript>();
        waveSpawner = GameObject.Find("EnemySpawner(Clone)").GetComponent<WaveSpawner>();

        /* Initialize */

        // Player Data
        skillset = player.getSkills();
        inventory = player.getItems();


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

        // Headshot image
        headshotImage.SetActive(false);

        // Shop
        shopPanel.SetActive(false);

        for(int i = 0; i < shopImageList.Length; i++) {
            Image im = shopImageList[i];
            im.sprite = tier2[i];
        }

        for(int i = 0; i < shopTextList.Length; i++) {
            Text tx = shopTextList[i];
            tx.text = "+" + inventory[i].getValue().ToString();
        }

        for(int i = 0; i < costTextList.Length; i++) {
            Text tx = costTextList[i];
            tx.text = inventory[i].getCost().ToString();
        }

        // GUI
        UpdateStats();
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
    }

    void Update() {
        // Pause menu behaviour
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }

        if(!firstWaveStarted && Input.GetKeyDown(KeyCode.Escape)) {
            firstWaveText.enabled = false;
        }
        else if(!firstWaveStarted) {
            TextColorShift(firstWaveText);
        }

        if(Input.GetKeyDown(KeyCode.Tab)) {
            player.setTowerSelected(!player.getTowerSelected());
        }

        if(Input.GetKeyDown(KeyCode.Backslash)) {
            shopPanel.SetActive(!shopPanel.activeSelf);
            crosshair.SetActive(!crosshair.activeSelf);
            playerScript.enabled = !playerScript.enabled;
            cameraScript.enabled = !cameraScript.enabled;
            Screen.lockCursor = !Screen.lockCursor;
            Screen.showCursor = !Screen.showCursor;
            if(Time.timeScale == 0.000001f) {
                Time.timeScale = 1; ;
            }
            else {
                Time.timeScale = 0.000001f;
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
            tx.text = "+" + inventory[i].getValue().ToString();
        }

        for(int i = 0; i < costTextList.Length; i++) {
            Text tx = costTextList[i];
            tx.text = inventory[i].getCost().ToString();
        }

        for(int i = 0; i < shopButtonList.Length; i++) {
            Button bt = shopButtonList[i];
            if(inventory[i].getTier() < 4 && player.getGold() >= inventory[i].getCost()) {
                bt.interactable = true;
            }
            else {
                bt.interactable = false;
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

    public void UpgradeSword() {
        inventory[0].setTier(inventory[0].getTier() + 1);
        player.setItems(inventory);
        player.setAttack(player.getAttack() + inventory[0].getValue());
        player.removeGold(inventory[0].getCost());
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeWand() {
        inventory[1].setTier(inventory[1].getTier() + 1);
        player.setItems(inventory);
        player.setMagic(player.getMagic() + inventory[1].getValue());
        player.removeGold(inventory[1].getCost());
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeShield() {
        inventory[2].setTier(inventory[2].getTier() + 1);
        player.setItems(inventory);
        player.setArmor(player.getArmor() + inventory[2].getValue());
        player.removeGold(inventory[2].getCost());
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    public void UpgradeBoots() {
        inventory[3].setTier(inventory[3].getTier() + 1);
        player.setItems(inventory);
        player.setAgility(player.getAgility() + inventory[3].getValue());
        player.removeGold(inventory[3].getCost());
        UpdateGold();
        UpdateShop();
        UpdateItems();
        UpdateStats();
    }

    void DisableHeadShot() {
        headshotImage.SetActive(false);
    }

}
