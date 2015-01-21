using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// The control script for the main menu
public class MenuController : MonoBehaviour {
    // Objects
    public Animator startBtnAnim, quitBtnAnim, optionBtnAnim, editorBtnAnim, loadBtnAnim, scoreBtnAnim, creditsBtnAnim, optionPnlAnim, scorePnlAnim, statPnlAnim;
    public GameObject optionPnl, scorePnl, creditsPnl;
    public AudioClip click;
    public GameObject mainCamera;
    public Slider[] sliders = new Slider[3];
    public Slider scoreDifficulty;
    public Text[] sliderValues = new Text[3];
    public GameObject panelPrototype;
    public Sprite logoBG, noLogoBG;
    public Image background;
    public GameObject statsPnl;
    public Text[] statTextList = new Text[12];
    AudioSource cameraAudioSource;
    AudioSource backingAudio;
    AudioClip menuMusic;
    float volume;
    float musicVolume;
    // Slider values
    int val1, val2, val3;
    int old1, old2, old3;
    bool firstload;

    private Text textPrototype;
    private List<Text> rankList;
    private List<Text> nameList;
    private List<Text> hiScoreList;
    private List<GameObject> scorePanelList;
    private List<Button> scoreBtnList;
 
    
    public void ButtonClick()
    {
        cameraAudioSource.PlayOneShot(click, volume);
    }

    void Start() {
        Time.timeScale = 1;
        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        musicVolume = (float)PlayerPrefs.GetInt("BGM") / 100f;
        menuMusic = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().menuMusic;
        backingAudio = GameObject.Find("backingAudio").GetComponent<AudioSource>();
        cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        backingAudio.clip = menuMusic;
        backingAudio.volume = musicVolume * 0.5f;
        firstload = true;
        backingAudio.Play();

        // Set options on first run
        if(!PlayerPrefs.HasKey("BGM")) {
            PlayerPrefs.SetInt("BGM", 100);
        }
        if(!PlayerPrefs.HasKey("SFX")) {
            PlayerPrefs.SetInt("SFX", 100);
        } 
        if(!PlayerPrefs.HasKey("Difficulty")) {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
        if(PlayerPrefs.GetInt("Online") == 0) {
            scoreBtnAnim.GetComponentInParent<Button>().interactable = false;
        }
        else {
            scoreBtnAnim.GetComponentInParent<Button>().interactable = true;
        }
        

        creditsPnl.SetActive(false);

        // Menu startup animation
        startBtnAnim.SetBool("Hidden", false);
        quitBtnAnim.SetBool("Hidden", false);
        optionBtnAnim.SetBool("Hidden", false);
        editorBtnAnim.SetBool("Hidden", false);
        loadBtnAnim.SetBool("Hidden", false);
        scoreBtnAnim.SetBool("Hidden", false);
        creditsBtnAnim.SetBool("Hidden", false);
        optionPnlAnim.SetBool("Hidden", true);
        scorePnlAnim.SetBool("Hidden", true);
        statPnlAnim.SetBool("Hidden", true);
        // Load user preferences
        val1 = PlayerPrefs.GetInt("BGM");
        val2 = PlayerPrefs.GetInt("SFX");
        val3 = PlayerPrefs.GetInt("Difficulty");
        // Set sliders to correct values
        sliders[0].value = val1;
        sliders[1].value = val2;
        sliders[2].value = val3;

        statsPnl.SetActive(false);

        rankList = new List<Text>();
        nameList = new List<Text>();
        hiScoreList = new List<Text>();
        scorePanelList = new List<GameObject>();
        scoreBtnList = new List<Button>();

        Button btn0 = panelPrototype.transform.Find("NameButton").GetComponent<Button>();
        btn0.onClick.AddListener(() => ToStatistics(0));

        scorePanelList.Add(panelPrototype);
        rankList.Add(panelPrototype.transform.Find("Rank").GetComponent<Text>());
        nameList.Add(panelPrototype.transform.Find("NameButton").GetComponentInChildren<Text>());
        hiScoreList.Add(panelPrototype.transform.Find("Score").GetComponent<Text>()); 
        scoreBtnList.Add(btn0);

        for(int i = 1; i < 11; i++) {
            GameObject newPanel = (GameObject)Instantiate(panelPrototype);
            newPanel.transform.SetParent(scorePnl.transform);
            Text rank = newPanel.transform.Find("Rank").GetComponent<Text>();
            Text name = newPanel.transform.Find("NameButton").GetComponentInChildren<Text>();
            Text score = newPanel.transform.Find("Score").GetComponent<Text>();
            Button btn = newPanel.transform.Find("NameButton").GetComponent<Button>();
            RectTransform transform = newPanel.GetComponent<RectTransform>();
            transform.localScale = new Vector3(1, 1, 1);
            if(i == 10) {
                transform.anchoredPosition = scorePanelList[9].GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -100f);
            }
            else {
                transform.anchoredPosition = scorePanelList[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -40f * (float)i);
            }
            btn.onClick.AddListener(() => ToStatistics(i));
            scorePanelList.Add(newPanel);
            rankList.Add(rank);
            nameList.Add(name);
            hiScoreList.Add(score);
            scoreBtnList.Add(btn);
        }
    }

    void FixedUpdate() {
        UpdateSliderVals();
        musicVolume = (float)val1/100f;
        backingAudio.volume = musicVolume * 0.5f;
        if(Input.GetKey(KeyCode.Space)) {
            creditsPnl.SetActive(false);
        }
    }

    void UpdateSliderVals() {
        // Extract values from sliders as long as the options panel is active
        if(!optionPnlAnim.GetBool("Hidden")) {
            val1 = (int)sliders[0].value;
            val2 = (int)sliders[1].value;
            val3 = (int)sliders[2].value;
        }
        sliderValues[0].text = val1.ToString();
        sliderValues[1].text = val2.ToString();
        // Difficulty slider
        switch(val3) {
            case 0:
                sliderValues[2].text = "Beginner";
                break;
            case 1:
                sliderValues[2].text = "Average";
                break;
            case 2:
                sliderValues[2].text = "Expert";
                break;
            case 3:
                sliderValues[2].text = "Godlike";
                break;
        }
    }

    // Apply new options
    public void ApplyOptions() {
        // Save preferences
        PlayerPrefs.SetInt("BGM", val1);
        PlayerPrefs.SetInt("SFX", val2);
        PlayerPrefs.SetInt("Difficulty", val3);

        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        musicVolume = (float)PlayerPrefs.GetInt("BGM") / 100f;
        backingAudio.volume = musicVolume * 0.5f;

		ResourceManager.Difficulty = val3;
        // And close the screen
        CloseOptionScreen();
    }
    // Open the option screen
    public void OpenOptionScreen() {
        // Option screen transition animation
        startBtnAnim.SetBool("Hidden", true);
        quitBtnAnim.SetBool("Hidden", true);
        optionBtnAnim.SetBool("Hidden", true);
        editorBtnAnim.SetBool("Hidden", true);
        loadBtnAnim.SetBool("Hidden", true);
        scoreBtnAnim.SetBool("Hidden", true);
        creditsBtnAnim.SetBool("Hidden", true);
        optionPnlAnim.SetBool("Hidden", false);
        scorePnlAnim.SetBool("Hidden", true);
        statPnlAnim.SetBool("Hidden", true);

        loadBtnAnim.SetTrigger("GoLeft");
        optionBtnAnim.SetTrigger("GoLeft");
        editorBtnAnim.SetTrigger("GoLeft");

        // Store old slider values in case of cancel
        old1 = val1;
        old2 = val2;
        old3 = val3;
    }

    public void OpenScoreScreen() {
        // Score screen transition animation
        startBtnAnim.SetBool("Hidden", true);
        quitBtnAnim.SetBool("Hidden", true);
        optionBtnAnim.SetBool("Hidden", true);
        editorBtnAnim.SetBool("Hidden", true);
        scoreBtnAnim.SetBool("Hidden", true);
        loadBtnAnim.SetBool("Hidden", true);
        creditsBtnAnim.SetBool("Hidden", true);
        optionPnlAnim.SetBool("Hidden", true);
        scorePnlAnim.SetBool("Hidden", false);
        statPnlAnim.SetBool("Hidden", false);

        loadBtnAnim.SetTrigger("GoRight");
        optionBtnAnim.SetTrigger("GoRight");
        editorBtnAnim.SetTrigger("GoRight");

        background.sprite = noLogoBG;
        updateHiScores();
    }

    // Undo the changes made to the options
    public void CancelOptionScreen() {
        // Set old values back to before opening the options
        val1 = old1;
        val2 = old2;
        val3 = old3;

        sliders[0].value = val1;
        sliders[1].value = val2;
        sliders[2].value = val3;
        // And close the screen
        CloseOptionScreen();
    }
    // Function that closes the option screen
    public void CloseOptionScreen() {
        // Return to main menu animation
        startBtnAnim.SetBool("Hidden", false);
        quitBtnAnim.SetBool("Hidden", false);
        optionBtnAnim.SetBool("Hidden", false);
        editorBtnAnim.SetBool("Hidden", false);
        loadBtnAnim.SetBool("Hidden", false);
        scoreBtnAnim.SetBool("Hidden", false);
        creditsBtnAnim.SetBool("Hidden", false);
        optionPnlAnim.SetBool("Hidden", true);
        scorePnlAnim.SetBool("Hidden", true);
        statPnlAnim.SetBool("Hidden", true);
    }

    public void CloseScoreScreen() {
        // Return to main menu animation
        CloseOptionScreen();
        background.sprite = logoBG;
    }

    public void updateHiScores() {
        if(firstload) {
            scoreDifficulty.value = val3;
            firstload = false;
        }
        List<List<string>> HiScores = ScoreServer.getHiscores((int)scoreDifficulty.value);
        bool playerInList = false;
        for(int i = 0; i < 10; i++) {
            try {
                rankList[i].text = (i + 1).ToString() + ":";
                nameList[i].text = HiScores[i][0];
                hiScoreList[i].text = HiScores[i][1];
                if(HiScores[i][0] == PlayerPrefs.GetString("Login")) {
                    playerInList = true;
                }
                scorePanelList[i].SetActive(true);
            }
            catch {
                scorePanelList[i].SetActive(false);
            }
        }
        if(playerInList) {
            scorePanelList[10].SetActive(false);
        }
        else {
            int index = ScoreServer.getPositionOnHiscores((int)scoreDifficulty.value, PlayerPrefs.GetString("Login"));
            if(index != 0) {
                scorePanelList[10].SetActive(true);
                rankList[10].text = ScoreServer.getPositionOnHiscores((int)scoreDifficulty.value, PlayerPrefs.GetString("Login")).ToString() + ":";
                nameList[10].text = PlayerPrefs.GetString("Login");
                hiScoreList[10].text = HiScores[ScoreServer.getPositionOnHiscores((int)scoreDifficulty.value, PlayerPrefs.GetString("Login"))][1];
            }
            else {
                scorePanelList[10].SetActive(false);
            }
        }

        Text diffText = scoreDifficulty.transform.Find("Value").GetComponent<Text>();

        switch((int)scoreDifficulty.value) {
            case 0:
                diffText.text = "Beginner";
                break;
            case 1:
                diffText.text = "Average";
                break;
            case 2:
                diffText.text = "Expert";
                break;
            case 3:
                diffText.text = "Godlike";
                break;
        }

    }

    void UpdateStatistics(int i) {
        List<string> stats = ScoreServer.getStatisticsDifficulty((int)scoreDifficulty.value,i);
        for(int j = 0; j < stats.Count; j++) {
            statTextList[j].text = stats[j];
        }
    }

    public void ToStatistics(int index){
        scorePnl.SetActive(false);
        statsPnl.SetActive(true);
        UpdateStatistics(index);
    }

    public void BackToScores() {
        scorePnl.SetActive(true);
        statsPnl.SetActive(false);
    }

    public void LoadGameAudio() {
        Invoke("LoadGame", 0);
    }

    public void LoadGame()
    {
        Application.LoadLevel("Random Generate Level");
    }

    public void LoadEditor() {
        Application.LoadLevel("Level Editor");
    }

    public void LoadMap() {
        Application.LoadLevel("LoadScreen");
    }

    public void CreditsScreen() {
        creditsPnl.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
