using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScript : MonoBehaviour {

    public Text notification;
    public Button loginBtn, passBtn;
    public InputField login, password;
    private bool onlineOnce, offlineOnce;

    void Start() {
        Time.timeScale = 1;
        onlineOnce = false;
        offlineOnce = false;
    }

	void Update () {
        if(ScoreServer.connected) {
            if(!onlineOnce) {
                notification.text = "Please log in or register to make full use of the statistics in this game. You can play offline, but then only your local highscore will be available.";
                loginBtn.interactable = true;
                passBtn.interactable = true;
                onlineOnce = true;
            }
        }
        else {
            if(!offlineOnce) {
                notification.text = "No connection to the server. Please check your internet connection or play offline.";
                loginBtn.interactable = false;
                passBtn.interactable = false;
                offlineOnce = false;
            }
        }
	}

    public void Login(){
        if(login.text != "" && password.text != "" && AccountServer.usernamePasswordMatch(login.text, password.text)){
            PlayerPrefs.SetString("Login", login.text);
            PlayerPrefs.SetString("Password", password.text);
            PlayerPrefs.SetInt("Online", 1);
            Application.LoadLevel("Main Menu");
        } else {
            notification.text = "This username/password combination is incorrect. Please try again.";
            password.text = "";
        }
    }

    public void Register(){
        if(login.text !=  "" && password.text != "" && AccountServer.register(login.text, password.text)) {
            PlayerPrefs.SetString("Login", login.text);
            PlayerPrefs.SetString("Password", password.text);
            PlayerPrefs.SetInt("Online", 1);
            Application.LoadLevel("Main Menu");
        } else {
            if(login.text != "" && password.text != ""){
                notification.text = "This username is already in use. Please use another name.";
                login.text = "";
            } 
            else {
                notification.text = "Please enter both a username and password.";
            }
        }
    }

    public void Offline(){
        PlayerPrefs.SetInt("Online", 0);
        Application.LoadLevel("Main Menu");
    }
	
}
