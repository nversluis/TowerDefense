using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// The control script for the main menu
public class MenuController : MonoBehaviour {
    // Objects
    public Animator startBtnAnim, quitBtnAnim, optionBtnAnim, editorBtnAnim, loadBtnAnim, scoreBtnAnim, creditsBtnAnim, optionPnlAnim, scorePnlAnim;
    public GameObject optionPnl, scorePnl, creditsPnl;
    public AudioClip click;
    public GameObject mainCamera;
    public Slider[] sliders = new Slider[3];
    public Text[] sliderValues = new Text[3];
    AudioSource cameraAudioSource;
    AudioSource backingAudio;
    AudioClip menuMusic;
    float volume;
    float musicVolume;
    // Slider values
    int val1, val2, val3;
    int old1, old2, old3;
    
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
        // Load user preferences
        val1 = PlayerPrefs.GetInt("BGM");
        val2 = PlayerPrefs.GetInt("SFX");
        val3 = PlayerPrefs.GetInt("Difficulty");
        // Set sliders to correct values
        sliders[0].value = val1;
        sliders[1].value = val2;
        sliders[2].value = val3;
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
                sliderValues[2].text = "Easy";
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

        loadBtnAnim.SetTrigger("GoRight");
        optionBtnAnim.SetTrigger("GoRight");
        editorBtnAnim.SetTrigger("GoRight");
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
