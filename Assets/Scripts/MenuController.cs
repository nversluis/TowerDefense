using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// The control script for the main menu
public class MenuController : MonoBehaviour {
    // Objects
    public Animator startBtnAnim, quitBtnAnim, optionBtnAnim, editorBtnAnim, loadBtnAnim,optionPnlAnim;
    public GameObject optionPnl;
    public Slider slider1, slider2, slider3, slider4, slider5;
    // Slider values
    float val1, val2, val3, val4, val5;
    float old1, old2, old3, old4, old5;

    void Start() {
        // Menu startup animation
        startBtnAnim.SetBool("Hidden", false);
        quitBtnAnim.SetBool("Hidden", false);
        optionBtnAnim.SetBool("Hidden", false);
        editorBtnAnim.SetBool("Hidden", false);
        loadBtnAnim.SetBool("Hidden", false);
        optionPnlAnim.SetBool("Hidden", true);
        // Options panel is not used; turn it off
        optionPnl.SetActive(false);
        // Load user preferences
        val1 = PlayerPrefs.GetFloat("slider1");
        val2 = PlayerPrefs.GetFloat("slider2");
        val3 = PlayerPrefs.GetFloat("slider3");
        val4 = PlayerPrefs.GetFloat("slider4");
        val5 = PlayerPrefs.GetFloat("slider5");
        // Set sliders to correct values
        slider1.value = val1;
        slider2.value = val2;
        slider3.value = val3;
        slider4.value = val4;
        slider5.value = val5;
    }

    void LateUpdate() {
        // Extract values from sliders as long as the options panel is active
        if(optionPnl.activeSelf == true) {
            val1 = slider1.value;
            val2 = slider2.value;
            val3 = slider3.value;
            val4 = slider4.value;
            val5 = slider5.value;
        }
    }
    // Apply new options
    public void ApplyOptions() {
        // Save preferences
        PlayerPrefs.SetFloat("slider1", val1);
        PlayerPrefs.SetFloat("slider2", val2);
        PlayerPrefs.SetFloat("slider3", val3);
        PlayerPrefs.SetFloat("slider4", val4);
        PlayerPrefs.SetFloat("slider5", val5);
        // And close the screen
        CloseOptionScreen();
    }
    // Open the option screen
    public void OpenOptionScreen() {
        // Option screen transition animation
        optionPnl.SetActive(true);
        startBtnAnim.SetBool("Hidden", true);
        quitBtnAnim.SetBool("Hidden", true);
        optionBtnAnim.SetBool("Hidden", true);
        editorBtnAnim.SetBool("Hidden", true);
        loadBtnAnim.SetBool("Hidden", true);
        optionPnlAnim.SetBool("Hidden", false);
        // Store old slider values in case of cancel
        old1 = val1;
        old2 = val2;
        old3 = val3;
        old4 = val4;
        old5 = val5;
    }
    // Undo the changes made to the options
    public void CancelOptionScreen() {
        // Set old values back to before opening the options
        val1 = old1;
        val2 = old2;
        val3 = old3;
        val4 = old4;
        val5 = old5;
        slider1.value = val1;
        slider2.value = val2;
        slider3.value = val3;
        slider4.value = val4;
        slider5.value = val5;
        // And close the screen
        CloseOptionScreen();
    }
    // Function that closes the option screen
    void CloseOptionScreen() {
        // Return to main menu animation
        startBtnAnim.SetBool("Hidden", false);
        quitBtnAnim.SetBool("Hidden", false);
        optionBtnAnim.SetBool("Hidden", false);
        editorBtnAnim.SetBool("Hidden", false);
        loadBtnAnim.SetBool("Hidden", false);
        optionPnlAnim.SetBool("Hidden", true);
        // Wait until options screen is off-screen before disabling it
        StartCoroutine(InactiveAfter(optionPnl, 1));
    }

    public void LoadGame() {
        Application.LoadLevel("Random Generate Level");
    }

    public void LoadEditor() {
        Application.LoadLevel("Level Editor");
    }

    public void LoadMap() {
        Application.LoadLevel("Map Loader");
    }

    public void QuitGame() {
        Application.Quit();
    }
    // Coroutine that waits to disable an object until after a certain time
    IEnumerator InactiveAfter(GameObject obj, float t) {
        yield return new WaitForSeconds(t);
        obj.SetActive(false);
    }
}
