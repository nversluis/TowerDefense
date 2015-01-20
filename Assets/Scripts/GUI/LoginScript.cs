using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScript : MonoBehaviour {

    public Text notification;
    public InputField login, password;

	void Start () {
        notification.text = "Please log in or register to make full use of the statistics in this game. You can play offline, but then only your local highscore will be available.";
        login.text = "";
        password.text = "";
	}

    public void Login(){
        if(true){
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
        if(true){
            PlayerPrefs.SetString("Login", login.text);
            PlayerPrefs.SetString("Password", password.text);
            PlayerPrefs.SetInt("Online", 1);
            Application.LoadLevel("Main Menu");
        } else {
            notification.text = "This username is already in use. Please use another name.";
            login.text = "";
        }
    }

    public void Offline(){
        PlayerPrefs.SetInt("Online", 0);
    }
	
}
